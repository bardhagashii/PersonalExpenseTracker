namespace PersonalExpenseTracker.Exceptions
{
    public class InvalidAmountException : Exception
    {
        public InvalidAmountException()
            : base("Amount cannot be 0 or negative!") { }
    }

    public class EmptyDescriptionException : Exception
    {
        public EmptyDescriptionException()
            : base("Description cannot be empty!") { }
    }

    public class EmptyCategoryException : Exception
    {
        public EmptyCategoryException()
            : base("Category cannot be empty!") { }
    }

    public class EmptyListException : Exception
    {
        public EmptyListException()
            : base("No expenses found!") { }
    }

}
