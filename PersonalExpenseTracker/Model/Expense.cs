namespace PersonalExpenseTracker.Model
{
    public class Expense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Today; // Default to today's date
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }

    }
}
