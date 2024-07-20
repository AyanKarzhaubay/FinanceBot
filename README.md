# FinanceBot
Description
FinanceBot is a Telegram bot for personal finance management. The bot allows users to register, add and view their purchases, and get statistics on their expenses.

Requirements
.NET 8.0
SQL Server
Visual Studio 2022 or later
Installation
Step 1: Clone the repository
bash
git clone https://github.com/yourusername/FinanceBot.git
cd FinanceBot
Step 2: Configure the database
Update the connection string in the BotDbContext.cs file if necessary:

csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer(@"Data Source=YOUR_SERVER_NAME;Initial Catalog=FinanceBotDB;Integrated Security=True;TrustServerCertificate=True");
}
Step 3: Apply migrations
Open the Package Manager Console and run the following commands:

powershell
Add-Migration InitialCreate
Update-Database
These commands will create and apply the necessary migrations, setting up the database tables.

Running the Application
Step 1: Set up the Telegram token
Open the Program.cs file and insert your Telegram token:

csharp
string token = "YOUR_TELEGRAM_BOT_TOKEN";
Step 2: Run the application
Run the application through Visual Studio or the command line:

bash
dotnet run
You should see the message Bot is running..., indicating that the bot has started successfully.

Usage
Register a user
Send the /start command in the chat with the bot to register. The bot will automatically save your Telegram ID and registration date.

Add a purchase
Send a message in the format:

bash
/addpurchase {name} {price} {category} {description}
Example:

bash
/addpurchase Coffee 250.50 Food "Morning coffee"
View purchases
Send a message in the format:

sql
/getpurchases {start date} {end date} {category (optional)}
Example:

yaml
/getpurchases 2023-07-01 2023-07-20
/getpurchases 2023-07-01 2023-07-20 Food
Get total spent
Send a message in the format:

sql
/totalspent {start date} {end date} {category (optional)}
Example:

yaml
/totalspent 2023-07-01 2023-07-20
/totalspent 2023-07-01 2023-07-20 Food
Logging
The application uses Microsoft.Extensions.Logging for logging. Logs are written to the console to help track the bot's actions and possible errors.

Making Changes
If you want to make changes to the code, be sure to create and apply new migrations if you change the database structure:

powershell
Add-Migration YourMigrationName
Update-Database
Support
If you encounter issues or have questions, please create an issue on GitHub.

License
This project is licensed under the MIT License.
