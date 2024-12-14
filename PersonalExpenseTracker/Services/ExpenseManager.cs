using Newtonsoft.Json;
using PersonalExpenseTracker.Exceptions;
using PersonalExpenseTracker.Model;
using System.Security.Cryptography.X509Certificates;

namespace PersonalExpenseTracker.Services
{
    public class ExpenseManager
    {
        private List<Expense> expenses = new List<Expense>();
        private readonly string filePath = "expenses.json";  // file path to save expenses

        // add constructor to load expenses when the expense manager is instantiated
        public ExpenseManager()
        {
            expenses = LoadExpenses();  // load expenses from file
        }

        // save expenses to a JSON file
        public void SaveExpenses()
        {
            try
            {
                var json = JsonConvert.SerializeObject(expenses, Formatting.Indented); // Convert list to JSON
                File.WriteAllText(filePath, json); // Save JSON to file
                Console.WriteLine("Expenses saved to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving expenses: {ex.Message}");
            }
        }
        // Load expenses from a JSON file
        private List<Expense> LoadExpenses()
        {
            try
            {
                if (File.Exists(filePath))  // check if the file exists
                {
                    var json = File.ReadAllText(filePath);  // read JSON content from the file
                    return JsonConvert.DeserializeObject<List<Expense>>(json) ?? new List<Expense>(); // Deserialize back to List<Expense>
                }
                return new List<Expense>();  // return empty list if file doesn't exist
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading expenses: {ex.Message}");
                return new List<Expense>();  // eturn empty list on error
            }
        }


        // Adding operation
        public void AddExpense(string description, string category, decimal amount, DateTime? date = null)
        {
            // Validate description of expense
            if (string.IsNullOrEmpty(description))
            {
                throw new EmptyDescriptionException();
            }

            // Validate category of expense
            if (string.IsNullOrEmpty(category))
            {
                throw new EmptyCategoryException();
            }


            // Validate amount of expense
            if (amount <= 0)
            {
                throw new InvalidAmountException();
            }

            var expense = new Expense
            {
                Description = description,
                Category = category,  // Store the Category enum value directly
                Amount = amount,
                Date = date ?? DateTime.Now  // Default to today's date if not provided
            };

            expenses.Add(expense);
            SaveExpenses();  // save expenses to file after adding new expense
        }

        //View all expenses
        public List<Expense> GetAllExpenses()
        {
            if (!expenses.Any())  // Check if the expenses list is empty
            {
                throw new EmptyListException();  // Handle the empty list scenario
            }

            return expenses;  // Return the list of expenses
        }


        //Display all expenses in a tabular format grouped by category.
        public Dictionary<string, List<Expense>> GetExpensesByCategory()
        {
            if (!expenses.Any())
            {
                throw new EmptyListException(); 
            }
            // Group expenses by category once and return it
            var groupedExpenses = expenses
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Display expenses in tabular format grouped by category
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
            return groupedExpenses;
        }


        //Calculate and display the total amount spent in each category
        public Dictionary<string, decimal> GetTotalAmountOfCategory()
        {
            if (!expenses.Any())  // Check if the expenses list is empty
            {
                throw new EmptyListException();
            }
            var groupedExpenses = expenses.GroupBy(e => e.Category).ToList();
            foreach (var group in groupedExpenses)
            {
                decimal totalAmount = group.Sum(e => e.Amount);
                Console.WriteLine($"Total amount spent for {group.Key}: {totalAmount:C}\n");

            }
            return expenses.GroupBy(e => e.Category).ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

        }


        //Calculate and display the overall total expense
        public decimal GetTotalAmount()
        {
            if (!expenses.Any())
            {
                throw new EmptyListException();
            }
            decimal expensesTotal = expenses.Sum(e => e.Amount);
            Console.WriteLine($"Overall total expense: {expensesTotal:C}");

            return expenses.Sum(e => e.Amount);
        }


        //Delete an expense by providing its ID.
        public bool DeleteExpense(Guid id)
        {
            var expenseToDelete = expenses.FirstOrDefault(e => e.Id == id);

            if (expenseToDelete != null)         //check for null input
            {
                expenses.Remove(expenseToDelete);
                Console.WriteLine($"Expense with ID {id} has been deleted.");
                return true;
            }
            else
            {
                Console.WriteLine($"Expense with ID {id} not found.");
                return false;
            }
        }


        //Add a feature to filter expenses by date range.
        public List<Expense> FilterExpensesByDateRange(DateTime startDate, DateTime endDate)
        {
            if (!expenses.Any())
            {
                throw new EmptyListException();
            }
            if (startDate > endDate)     //check for invalid input
            {
                throw new ArgumentException("Start date cannot be later than the end date!");
            }

            var filteredExpenses = expenses.Where(e => e.Date >= startDate && e.Date <= endDate).ToList(); //filter expenses withing the given range

            if (!filteredExpenses.Any())   //checks if there are any expenses within the given date
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
