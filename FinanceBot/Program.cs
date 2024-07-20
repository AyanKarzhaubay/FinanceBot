using FinanceBot;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private static ITelegramBotClient botClient;
    private static BotDbContext dbContext;
    private static MessageHandler messageHandler;
    static void Main()
    {
        string token = "5910511903:AAEb-KNwMYqJb-5AXPbPbC0gB_GcSjX0EBE";
        Console.Title = "TelegramBot";
        Console.OutputEncoding = System.Text.Encoding.UTF8;


        botClient = new TelegramBotClient(token);
        dbContext = new BotDbContext();
        messageHandler = new MessageHandler(botClient, dbContext);

        
        var cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // получать все типы обновлений
        };
        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token
        );

        Console.WriteLine("Bot is running...");
        Console.ReadLine();

        cts.Cancel();
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not null)
        {
            await messageHandler.HandleMessageAsync(update.Message);
        }
    }

    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Error: {exception.Message}");
        return Task.CompletedTask;
    }
}
