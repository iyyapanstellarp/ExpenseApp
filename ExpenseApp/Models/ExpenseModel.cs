namespace ExpenseApp.Models
{
    public class ExpenseModel
    {

        public ExpenseModel() { }


        private String expenseName;
        private String groupName;
        private double spent;
        private String createdDate;
        private String modifiedDate;
        private String createdBy;
        private long expenseID;
        private String expenseType;

        public string ExpenseName { get => expenseName; set => expenseName = value; }
        public string GroupName { get => groupName; set => groupName = value; }
        public double Spent { get => spent; set => spent = value; }
        public string CreatedDate { get => createdDate; set => createdDate = value; }
        public string ModifiedDate { get => modifiedDate; set => modifiedDate = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public long ExpenseID { get => expenseID; set => expenseID = value; }
        public string ExpenseType { get => expenseType; set => expenseType = value; }
    }
}
