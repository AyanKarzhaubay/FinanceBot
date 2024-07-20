# FinanceBot
FinanceBot is a Telegram bot designed to help users manage and track their expenses. It allows users to record purchases, categorize expenses, and retrieve information on their spending habits.

## Features

- Register and manage users via Telegram.
- Record purchases with details such as name, price, category, and description.
- Retrieve a list of purchases and calculate total spending within a specific date range.
- Optionally filter purchases by category.

## Technologies Used

- C#
- .NET
- Entity Framework Core
- MS SQL Server
- Telegram.Bot library

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
- [MS SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or a similar database setup.
- A Telegram bot token. You can obtain this by creating a bot via the [BotFather](https://core.telegram.org/bots#6-botfather).

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/FinanceBot.git
   cd FinanceBot
   Set up the database:

2. Update the connection string in BotDbContext.cs to match your database configuration.
Apply the migrations to create the database schema:
   ```bash
   dotnet ef database update
3. Run the bot:
   ```bash
   dotnet run

##Usage
1. Start the bot by running the application.
2. Interact with the bot through Telegram:
   Register by sending /start to the bot.
   Add a purchase by sending a message in the format:
   ```bash
   /add PurchaseName 123.45 Category Description
Retrieve purchases by sending a message in the format:
   ```yaml
   /purchases 2024-01-01 2024-01-31
```
Retrieve total spending by sending a message in the format:
   ```yaml
   /total 2024-01-01 2024-01-31
   ```
Example Commands
   ```yaml
   /add Coffee 4.50 Food Morning coffee
   /purchases 2024-01-01 2024-01-31
   /total 2024-01-01 2024-01-31
   ```

License
This project is licensed under the MIT License - see the LICENSE file for details.
