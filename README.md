# PersonalExpenseTracker
The Personal Expense Tracker is a C# Web API application designed to help users manage their personal expenses. Users can add, categorize, delete, and view their expenses, as well as calculate total amounts per category. The app supports file-based persistence (via JSON serialization) and can load saved expenses on startup. Additionally, it includes features for filtering expenses by date range and supports in-memory storage for testing purposes.


-Instructions to Run the Application:

Clone or download the repository.
Open the solution in Visual Studio.
Build and run the project.
Use a tool like Postman or Swagger to interact with the API.
The application also includes unit tests that can be run with XUnit.



-How the Requirements Are Addressed
The following methods in the ExpenseManager and ExpenseController classes handle specific functional requirements:

-Add Expense-
AddExpense: Adds a new expense and validates input.
ExpenseController.AddExpense: Endpoint (POST /api/expenses) for adding expenses.
View All Expenses:


-View expenses-

GetAllExpenses: Retrieves all expenses.
ExpenseController.GetAllExpenses: Endpoint (GET /api/expenses) to view all expenses.

Group Expenses by Category:
GetExpensesByCategory: Groups expenses by category.
ExpenseController.GetExpensesByCategory: Endpoint (GET /api/expenses/category) to view grouped expenses.

Calculate Total Expenses by Category:
GetTotalAmountOfCategory: Calculates the total for each category.
ExpenseController.GetTotalAmountOfCategory: Endpoint (GET /api/expenses/category/totalAmount) to view category totals.

Calculate Overall Total Expense:
GetTotalAmount: Sums all expenses.
ExpenseController.GetTotalAmount: Endpoint (GET /api/expenses/totalAmount) to view overall totals.

-Delete Expense-

DeleteExpense: Deletes an expense by its ID.
ExpenseController.DeleteExpense: Endpoint (DELETE /api/expenses/{id}) to delete an expense.


Bonus Features Implemented
Filter Expenses by Date Range:
FilterExpensesByDateRange: Filters expenses by date range.
ExpenseController.FilterExpensesByDateRange: Endpoint (GET /api/expenses/filterByDate) for filtering expenses.

Save Expenses to a File Using JSON Serialization
ExpenseManager (SaveExpense and LoadExpense methods) Saves expenses to a JSON file, allowing them to persist across application runs.

Unit Test Project with Test Cases:
The project includes unit tests (PersonalExpenseTracker.Tests) that verify functionality, including adding expenses, filtering by date, and deleting expenses.


Assumptions and Limitations
The application uses in-memory storage by default, with optional JSON file saving/loading. It assumes valid user inputs and does not implement authentication or roles. No database is used, and the app currently supports only basic date range filtering. Additionally, it does not support recurring expenses or advanced reporting.


Challenges
Setting up the test project was challenging due to data handling. I used in-memory storage to resolve this and ensure tests ran correctly.
Implementing error handling for invalid inputs required careful consideration to avoid app crashes. Custom exceptions were used to manage these errors.
