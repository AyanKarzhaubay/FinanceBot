using FinanceBot;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
namespace FinanceBot
{
    public class MessageHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BotDbContext _dbContext;
        private readonly PurchaseQueryHandler _purchaseQueryHandler;

        public MessageHandler(ITelegramBotClient botClient, BotDbContext dbContext)
        {
            _botClient = botClient;
            _dbContext = dbContext;
            _purchaseQueryHandler = new PurchaseQueryHandler(dbContext);
        }

        public async Task HandleMessageAsync(Message message)
        {
            if (message.Type != MessageType.Text)
                return;

            var text = message.Text;

            if (text.StartsWith("/start"))
            {
                await HandleStartCommand(message);
            }
            else if (text == "/help")
            {
                await HandleHelpCommand(message);
            }
            else if (text.StartsWith("/list"))
            {
                await HandleListCommand(message);
            }
            else if (text.StartsWith("/total"))
            {
                await HandleTotalCommand(message);
            }
            else
            {
                await HandlePurchaseMessage(message);
            }
        }

        private async Task HandleStartCommand(Message message)
        {
            var telegramId = message.From.Id;
            var user = _dbContext.Users.FirstOrDefault(u => u.TelegramId == telegramId);

            if (user == null)
            {
                user = new User
                {
                    TelegramId = telegramId,
                    Username = message.From.Username,
                    RegistrationDate = DateTime.Now
                };
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                await _botClient.SendTextMessageAsync(message.Chat, "Welcome! To log your purchase, use the format: Name.Price.Category.Description");
            }
            else
            {
                await _botClient.SendTextMessageAsync(message.Chat, "You are already registered. To log your purchase, use the format: Name.Price.Category.Description");
            }
        }

        private async Task HandleHelpCommand(Message message)
        {
            var helpText =
                "/start - Register or start using the bot\n" +
                "/help - Show this help message\n" +
                "/list - Get a list of your purchases\n" +
                "/total - Get the total amount spent\n" +
                "You can combine the list and total commands with the words day, month, year, or a specific period of time (dd.mm.yyyy-dd.mm.yyyy). You can also add a category.\n" +
                "To log your purchase, use the format: Name.Price.Category.Description";

            await _botClient.SendTextMessageAsync(message.Chat, helpText);
        }
        private async Task HandleListCommand(Message message)
        {
            var user = GetUserFromMessage(message);
            if (user == null)
            {
                await _botClient.SendTextMessageAsync(message.Chat, "User not found. Please use /start to register.");
                return;
            }

            var (startDate, endDate, category) = ParseListCommandArgs(message.Text);
            var purchases = await _purchaseQueryHandler.GetPurchasesAsync(user.Id, startDate, endDate, category);

            var response = FormatPurchases(purchases);
            await _botClient.SendTextMessageAsync(message.Chat, response);
        }
        private async Task HandleTotalCommand(Message message)
        {
            var user = GetUserFromMessage(message);
            if (user == null)
            {
                await _botClient.SendTextMessageAsync(message.Chat, "User not found. Please use /start to register.");
                return;
            }

            var (startDate, endDate, category) = ParseListCommandArgs(message.Text);
            var totalSpent = await _purchaseQueryHandler.GetTotalSpentAsync(user.Id, startDate, endDate, category);

            var response = $"Total spent from {startDate.ToShortDateString()} to {endDate.ToShortDateString()}: {totalSpent:C}";
            await _botClient.SendTextMessageAsync(message.Chat, response);
        }
        private async Task HandlePurchaseMessage(Message message)
        {
            var telegramId = message.From.Id;
            var user = _dbContext.Users.FirstOrDefault(u => u.TelegramId == telegramId);

            string errorMessage = null;
            if(user is null)
                await _botClient.SendTextMessageAsync(message.Chat, "User not found!");
            if (user != null && TryParsePurchase(message.Text, user.Id, out errorMessage))
            {
                await _botClient.SendTextMessageAsync(message.Chat, "Purchase logged successfully!");
            }
            else
            {
                await _botClient.SendTextMessageAsync(message.Chat, 
                    errorMessage ?? "Invalid purchase format. Use: Name.Price.Category.Description\nUse /help for more information.");
            }
        }
        private bool TryParsePurchase(string text, int userId, out string errorMessage)
        {
            errorMessage = null;

            var parts = text.Split('.', 4);

            if (parts.Length < 3)
            {
                errorMessage = "Invalid format. Use: Name.Price.Category.Description\nUse /help for more information.";
                return false;
            }

            var name = parts[0];
            if (!decimal.TryParse(parts[1], out var price))
            {
                errorMessage = "Invalid price format.\nUse /help for more information.";
                return false;
            }

            var category = parts[2];
            var description = parts.Length > 3 ? parts[3] : null;

            var purchase = new Purchase
            {
                UserId = userId,
                Name = name,
                Price = price,
                Category = category,
                Description = description,
                Date = DateTime.Now
            };

            _dbContext.Purchases.Add(purchase);
            _dbContext.SaveChanges();

            return true;
        }
        private User GetUserFromMessage(Message message)
        {
            var telegramId = message.From.Id;
            return _dbContext.Users.FirstOrDefault(u => u.TelegramId == telegramId);
        }

        private (DateTime startDate, DateTime endDate, string category) ParseListCommandArgs(string text)
        {
            var commandParts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;
            string category = null;

            if (commandParts.Length > 1)
            {
                if (DateParser.TryParseDateRange(commandParts[1], out startDate, out endDate))
                {
                    if (commandParts.Length > 2)
                    {
                        category = commandParts[2];
                    }
                }
            }

            return (startDate, endDate, category);
        }

        private string FormatPurchases(List<Purchase> purchases)
        {
            return purchases.Any()
                ? string.Join("\n", purchases.Select(p => $"{p.Date.ToShortDateString()}: {p.Name} - {p.Price:C} ({p.Category})"))
                : "No purchases found.";
        }
    }
}