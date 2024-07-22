namespace FinanceBot
{
    public class Purchase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
    }
}