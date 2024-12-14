using Microsoft.AspNetCore.Mvc;
using PersonalExpenseTracker.Exceptions;
using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Services;

namespace PersonalExpenseTracker.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class ExpenseController : ControllerBase
        {
            private readonly ExpenseManager _expenseManager;

            public ExpenseController(ExpenseManager expenseManager)
            {
                _expenseManager = expenseManager;
            }

            // api/expense
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
                    return BadRequest(ex.Message);
                }
            }


            // api/expense
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
                    return BadRequest(ex.Message);
                }
            }


            // api/expense/{id}
            [HttpDelete("{id}")]
            public IActionResult DeleteExpense(Guid id)
            {
                try
                {
                    var success = _expenseManager.DeleteExpense(id);
                    if (success)
                        return Ok($"Expense with ID {id} was deleted successfully!");
                    else
                        return NotFound($"Expense with ID {id} was not found!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        [HttpGet("byCategory")]
        public IActionResult GetExpensesByCategory()
        {
            try
            {
                var groupedExpenses = _expenseManager.GetExpensesByCategory();
                return Ok(groupedExpenses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpGet("/TotalAmountOfCategory")]
        public IActionResult GetTotalAmountOfCategory()
        {
            try
            {
                var totals = _expenseManager.GetTotalAmountOfCategory();
                return Ok(totals);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("/totalAmountOfExpenses")]
        public IActionResult GetTotalAmount()
        {
            try
            {
                var total = _expenseManager.GetTotalAmount();
                return Ok(new { TotalAmount = total });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpGet("filterByDateRange")]
        public ActionResult<List<Expense>> FilterExpensesByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Call the service to filter expenses
                var filteredExpenses = _expenseManager.FilterExpensesByDateRange(startDate, endDate);

                // Return the filtered expenses or a message if none found
                if (filteredExpenses.Count == 0)
                {
                    return NotFound(new { message = "No expenses found within the given date range!" });
                }

                return Ok(filteredExpenses);
            }
            catch (ArgumentException ex)
            {
                // Handle invalid date range exception
                return BadRequest(new { message = ex.Message });
            }
            catch (EmptyListException)
            {
                // Handle empty list exception
                return NotFound(new { message = "No expenses found!" });
            }
        }
    }

}

