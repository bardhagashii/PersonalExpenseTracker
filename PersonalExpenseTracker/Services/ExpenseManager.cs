using Newtonsoft.Json;
using PersonalExpenseTracker.Exceptions;
using PersonalExpenseTracker.Model;

namespace PersonalExpenseTracker.Services
{
    public class ExpenseManager
    {
        private List<Expense> expenses = new List<Expense>();

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
                Id = expenses.Count + 1,
                Description = description,
                Category = category,
                Amount = amount,
                Date = date ?? DateTime.Now  // Default to today's date if not provided
            };

            expenses.Add(expense);
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
        public void ExpensesByCategory()
        {
            if (!expenses.Any()) 
            {
               throw new EmptyListException();
            }

            var groupedExpenses = expenses
                .GroupBy(e => e.Category)       //group expenses by category
                .ToList();

            foreach (var group in groupedExpenses)
            {
                Console.WriteLine($"Category: {group.Key}\n-----------------------------"); 

                Console.WriteLine("ID\tDescription\t\tAmount\tDate");
                Console.WriteLine("-----------------------------");

                foreach (var expense in group)
                {
                    Console.WriteLine($"{expense.Id}\t{expense.Description}\t\t{expense.Amount}\t{expense.Date.ToShortDateString()}");
                }

                Console.WriteLine();
            }
        }


        //Calculate and display the total amount spent in each category
        public void CategoryTotalAmount()
        {
            if (!expenses.Any())  // Check if the expenses list is empty
            {
                throw new EmptyListException();
            }

            var groupedExpenses = expenses
                .GroupBy(e => e.Category)
                .ToList();

            foreach (var group in groupedExpenses)
            {
                decimal totalAmount = group.Sum(e => e.Amount);  
                Console.WriteLine($"Total amount spent for {group.Key}: {totalAmount:C}\n");
            }
        }


        //Calculate and display the overall total expense
        public void TotalAmountOfExpenses()
        {
            if (!expenses.Any()) 
            {
                throw new EmptyListException();
            }

            decimal expensesTotal = expenses.Sum(e => e.Amount);
            Console.WriteLine($"Overall total expense: {expensesTotal:C}");
        }


        //Delete an expense by providing its ID.
        public bool DeleteExpense(int id)
        {
            if (id <=0)      // check if ID is negative or zero
            {
                throw new InvalidIdException();     //handle invalid input exception
            }
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
    }
}
