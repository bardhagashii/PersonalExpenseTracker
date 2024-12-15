using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Services;
using PersonalExpenseTracker.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace PersonalExpenseTracker.Tests
{
    public class TestCases
    {
        private ExpenseManager _expenseManager;

        // Constructor: Initialize with in-memory storage
        public TestCases()
        {
            _expenseManager = new ExpenseManager(useInMemory: true); // Use in-memory storage for tests
        }

        // Test: Add an expense
        [Fact]
        public void Test_AddExpense()
        {
            // Act: Add an expense
            _expenseManager.AddExpense("Dinner", "Food", 50);

            // Assert: Verify if the expense is added correctly
            var expenses = _expenseManager.GetAllExpenses();
            Assert.Single(expenses);  // Only one expense should be present
            Assert.Equal("Dinner", expenses.First().Description);
            Assert.Equal("Food", expenses.First().Category);
            Assert.Equal(50, expenses.First().Amount);

            // Clean up
            _expenseManager.ClearExpenses();
        }

        // 2nd test case: Filter expenses by date range
        [Fact]
        public void FilterExpensesByDateRange_ReturnsFilteredExpenses()
        {
            _expenseManager = new ExpenseManager();
            var currentDate = DateTime.Now;

            // Adding 3 expenses with different dates
            _expenseManager.AddExpense("Dinner", "Food", 50.00m, currentDate.AddDays(-5));  // 5 days ago
            _expenseManager.AddExpense("Uber Ride", "Transport", 20.00m, currentDate.AddDays(-2));  // 2 days ago
            _expenseManager.AddExpense("Groceries", "Food", 30.00m, currentDate.AddDays(1));  // 1 day from now

            var startDate = currentDate.AddDays(-3);  // Start date: 3 days ago
            var endDate = currentDate;  // End date: today

            // Act
            var filteredExpenses = _expenseManager.FilterExpensesByDateRange(startDate, endDate);

            // Assert
            Assert.Equal(2, filteredExpenses.Count); // 2 should be in range
        }


        // 3rd test case: try to delete an expense with invalid input
        [Fact]
        public void Test_DeleteExpense_ThrowsExpenseNotFoundException_WhenExpenseNotFound()
        {
            _expenseManager = new ExpenseManager();
            var nonExistentId = Guid.NewGuid();  // Generate a new GUID that doesn't exist in the list

            // try to delete an expense with a non-existent ID and expect an exception
            var exception = Assert.Throws<ExpenseNotFoundException>(() =>
                _expenseManager.DeleteExpense(nonExistentId));  // This will throw the exception

            // check if the exception message matches the expected message
            Assert.Equal($"Expense with ID {nonExistentId} not found.", exception.Message);
        }

    }
}
