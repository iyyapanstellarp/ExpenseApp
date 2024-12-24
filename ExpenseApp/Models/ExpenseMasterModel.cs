namespace ExpenseApp.Models
{
    public class ExpenseMasterModel
    {
        public ExpenseMasterModel() { }

        private long expenseID;
        private String expenseName;
        private String expenseType;

       
        public string ExpenseName { get => expenseName; set => expenseName = value; }
        public string? ExpenseType { get => expenseType; set => expenseType = value; }
        public long ExpenseID { get => expenseID; set => expenseID = value; }
    }
}
