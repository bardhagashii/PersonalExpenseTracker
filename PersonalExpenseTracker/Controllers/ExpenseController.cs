using Microsoft.AspNetCore.Mvc;
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
            public IActionResult DeleteExpense(int id)
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

        // GET: api/expense/category/{category}
     /*   [HttpGet("expenses/by-category")]
        public IActionResult GetExpensesGroupedByCategory()
        {
            var groupedExpenses = _expenseManager.ExpensesByCategory();  // Assuming this method in your ExpenseManager class
            return Ok(groupedExpenses);  // Returns grouped expenses as JSON
        }

        // GET: api/expense/total
        [HttpGet("total")]
            public IActionResult GetTotalAmount()
            {
                try
                {
                    var total = _expenseManager.GetOverallTotal(); // Add method in ExpenseManager
                    return Ok($"Overall total expense: {total:C}");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            // GET: api/expense/total/category
            [HttpGet("total/category")]
            public IActionResult GetTotalAmountByCategory()
            {
                try
                {
                    var totals = _expenseManager.GetCategoryTotals(); // Add method in ExpenseManager
                    return Ok(totals);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }*/
        }
}
