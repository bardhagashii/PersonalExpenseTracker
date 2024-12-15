namespace PersonalExpenseTracker.Exceptions
{
    public class InvalidAmountException : Exception
    {
        public InvalidAmountException()
            : base("Amount cannot be 0 or negative!") { }

        public InvalidAmountException(string message)
            : base(message) { }

        public InvalidAmountException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class EmptyDescriptionException : Exception
    {
        public EmptyDescriptionException()
            : base("Description cannot be empty!") { }

        public EmptyDescriptionException(string message)
            : base(message) { }

        public EmptyDescriptionException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class EmptyCategoryException : Exception
    {
        public EmptyCategoryException()
            : base("Category cannot be empty!") { }

        public EmptyCategoryException(string message)
            : base(message) { }

        public EmptyCategoryException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class EmptyListException : Exception
    {
        public EmptyListException()
            : base("No expenses found!") { }

        public EmptyListException(string message)
            : base(message) { }

        public EmptyListException(string message, Exception inner)
            : base(message, inner) { }
    }
        public class ExpenseNotFoundException : Exception
        {
            public ExpenseNotFoundException()
                : base("Expense not found!") { }

            public ExpenseNotFoundException(string message)
                : base(message) { }

            public ExpenseNotFoundException(string message, Exception inner)
                : base(message, inner) { }
        }
}
