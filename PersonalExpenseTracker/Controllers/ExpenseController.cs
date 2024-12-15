using Microsoft.AspNetCore.Mvc;
using PersonalExpenseTracker.Exceptions;
using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Services;

namespace PersonalExpenseTracker.Controllers
{
    [Route("api/expenses")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseManager _expenseManager;

        public ExpenseController(ExpenseManager expenseManager)
        {
            _expenseManager = expenseManager;
        }

        // Helper method for handling responses
        private IActionResult HandleException(Exception ex)
        {
            switch (ex)
            {
                case EmptyListException:
                    return NotFound(new { message = ex.Message });
                case ArgumentException:
                    return BadRequest(new { message = ex.Message });
                default:
                    return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllExpenses()
        {
            try
            {
                var expenses = _expenseManager.GetAllExpenses();
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        public IActionResult AddExpense([FromBody] Expense expense)
        {
            try
            {
                _expenseManager.AddExpense(expense.Description, expense.Category, expense.Amount, expense.Date);
                return Ok("Expense added successfully.");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteExpense(Guid id)
        {
            // Check if the ID is empty or invalid 
            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Invalid expense ID." });
            }

            try
            {
                var success = _expenseManager.DeleteExpense(id);
                if (success)
                {
                    return Ok($"Expense with ID {id} was deleted successfully!");
                }
                else
                {
                    return NotFound($"Expense with ID {id} was not found!");
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("category")]
        public IActionResult GetExpensesByCategory()
        {
            try
            {
                var groupedExpenses = _expenseManager.GetExpensesByCategory();
                return Ok(groupedExpenses);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("category/totalAmount")]
        public IActionResult GetTotalAmountOfCategory()
        {
            try
            {
                var totals = _expenseManager.GetTotalAmountOfCategory();
                return Ok(totals);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("totalAmount")]
        public IActionResult GetTotalAmount()
        {
            try
            {
                var total = _expenseManager.GetTotalAmount();
                return Ok(new { TotalAmount = total });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }




        [HttpGet("filterByDate")]
        public ActionResult<List<Expense>> FilterExpensesByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                // call the service to filter expenses
                var filteredExpenses = _expenseManager.FilterExpensesByDateRange(startDate, endDate);

                // return the filtered expenses or a message if none found
                if (filteredExpenses.Count == 0)
                {
                    return NotFound(new { message = "No expenses found within the given date range!" });
                }

                return Ok(filteredExpenses);
            }
            catch (ArgumentException ex)
            {
                // handle invalid date range exception
                return BadRequest(new { message = ex.Message });
            }
            catch (EmptyListException)
            {
                // handle empty list exception
                return NotFound(new { message = "No expenses found!" });
            }
        }
    }
}
