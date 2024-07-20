using System.ComponentModel.DataAnnotations;
namespace FinanceBot
{
    public class User
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public string Username { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}