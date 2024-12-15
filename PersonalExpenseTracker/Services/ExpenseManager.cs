using Newtonsoft.Json;
using PersonalExpenseTracker.Exceptions;
using PersonalExpenseTracker.Model;
using System.Security.Cryptography.X509Certificates;

namespace PersonalExpenseTracker.Services
{
    public class ExpenseManager
    {
        private List<Expense> expenses = new List<Expense>();
        private readonly string filePath;  // File path to save expenses
        private bool useInMemory;

        // Constructor to load expenses when the expense manager is instantiated
        public ExpenseManager(string filePath = "expenses.json", bool useInMemory = false)
        {
            this.filePath = filePath;
            this.useInMemory = useInMemory;
            expenses = useInMemory ? new List<Expense>() : LoadExpenses();
        }

        // centralized empty list check
        private void EnsureExpensesExist()
        {
            if (!expenses.Any())
            {
                throw new EmptyListException();
            }
        }

        // centralized logging
        private void LogMessage(string message)
        {
            Console.WriteLine(message);
        }

        // centralized exception handling
        private void TryExecuteAction(Action action, string errorMessage)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{errorMessage}: {ex.Message}");
            }
        }

        public void ClearExpenses() => expenses.Clear();  // Used for test cases

        // Save expenses to a file
        public void SaveExpenses()
        {
            TryExecuteAction(() =>
            {
                var json = JsonConvert.SerializeObject(expenses, Formatting.Indented);
                File.WriteAllText(filePath, json);
                LogMessage("Expenses saved to file.");
            }, "Error saving expenses");
        }

        // Load expenses from a JSON file
        private List<Expense> LoadExpenses()
        {
            try
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Expense>>(json) ?? new List<Expense>();
            }
            catch (FileNotFoundException)
            {
                return new List<Expense>();  // Return empty list if file doesn't exist
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading expenses: {ex.Message}");
                return new List<Expense>();
            }
        }

        // Add a new expense
        public void AddExpense(string description, string category, decimal amount, DateTime? date = null)
        {
            if (string.IsNullOrEmpty(description))
                throw new EmptyDescriptionException();
            if (string.IsNullOrEmpty(category))
                throw new EmptyCategoryException();
            if (amount <= 0)
                throw new InvalidAmountException();

            var expense = new Expense
            {
                Description = description,
                Category = category,
                Amount = amount,
                Date = date ?? DateTime.Now  // Default to today's date if not provided
            };

            expenses.Add(expense);
            SaveExpenses();  // Save expenses to file after adding new expense
        }

        // View all expenses
        public List<Expense> GetAllExpenses()
        {
            EnsureExpensesExist();
            return expenses;
        }

        // Display all expenses in a tabular format grouped by category
        public Dictionary<string, List<Expense>> GetExpensesByCategory()
        {
            EnsureExpensesExist();
            var groupedExpenses = GroupExpensesByCategory();
            DisplayGroupedExpenses(groupedExpenses);
            return groupedExpenses;
        }

        // group expenses by category
        private Dictionary<string, List<Expense>> GroupExpensesByCategory()
        {
            return expenses.GroupBy(e => e.Category)
                           .ToDictionary(g => g.Key, g => g.ToList());
        }

        // Display grouped expenses in a tabular format
        private void DisplayGroupedExpenses(Dictionary<string, List<Expense>> groupedExpenses)
        {
            foreach (var group in groupedExpenses)
            {
                Console.WriteLine($"Category: {group.Key}\n-----------------------------");
                Console.WriteLine("ID\tDescription\t\tAmount\tDate");
                Console.WriteLine("-----------------------------");

                foreach (var expense in group.Value)
                {
                    Console.WriteLine($"{expense.Id}\t{expense.Description}\t\t{expense.Amount}\t{expense.Date.ToShortDateString()}");
                }
                Console.WriteLine();
            }
        }

        // Calculate and display the total amount spent in each category
        public Dictionary<string, decimal> GetTotalAmountOfCategory()
        {
            EnsureExpensesExist();

            var groupedExpenses = expenses
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

            foreach (var group in groupedExpenses)
            {
                Console.WriteLine($"Total amount spent for {group.Key}: {group.Value:C}");
            }

            return groupedExpenses;
        }

        // Calculate and display the overall total expense
        public decimal GetTotalAmount()
        {
            EnsureExpensesExist();
            decimal expensesTotal = expenses.Sum(e => e.Amount);
            Console.WriteLine($"Overall total expense: {expensesTotal:C}");

            return expensesTotal;
        }

        // Delete an expense by providing its ID
        public bool DeleteExpense(Guid id)
        {
            var expenseToDelete = expenses.FirstOrDefault(e => e.Id == id);

            if (expenseToDelete == null)
            {
                throw new ExpenseNotFoundException($"Expense with ID {id} not found.");
            }

            expenses.Remove(expenseToDelete);
            LogMessage($"Expense with ID {id} has been deleted.");
            return true;
        }

        // Filter expenses by date range
        public List<Expense> FilterExpensesByDateRange(DateTime startDate, DateTime endDate)
        {
            EnsureExpensesExist();

            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be later than the end date!");
            }

            var filteredExpenses = expenses.Where(e => e.Date >= startDate && e.Date <= endDate).ToList();

            if (!filteredExpenses.Any())
            {
                Console.WriteLine("No expenses found within the given date range!");
            }
            else
            {
                Console.WriteLine($"Expenses from {startDate.ToShortDateString()} to {endDate.ToShortDateString()}:");
                Console.WriteLine("ID\tDescription\t\tAmount\tDate");
                foreach (var expense in filteredExpenses)
                {
                    Console.WriteLine($"{expense.Id}\t{expense.Description}\t\t{expense.Amount:C}\t{expense.Date.ToShortDateString()}");
                }
            }

            return filteredExpenses;
        }
    }
}
