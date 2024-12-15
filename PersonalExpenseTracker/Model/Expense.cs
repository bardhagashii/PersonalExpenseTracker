﻿namespace PersonalExpenseTracker.Model
{
    public class Expense
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; } = DateTime.Today; // Default to today's date
        public string Description { get; set; }
        public string Category { get; set; } 
        public decimal Amount { get; set; }

    }
}
