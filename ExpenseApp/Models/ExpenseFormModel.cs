using System.Data.SqlTypes;

namespace ExpenseApp.Models
{
    public class ExpenseFormModel
    {
        public ExpenseFormModel() { }

        private String expenseName;
        private String groupName;
        private double spent;
        private String createdDate;
        private String modifiedDate;
        private String createdBy;
   
        public string? CreatedDate { get => createdDate; set => createdDate = value; }
        public string? ModifiedDate { get => modifiedDate; set => modifiedDate = value; }
        public string? CreatedBy { get => createdBy; set => createdBy = value; }
       
       
        public double Spent { get => spent; set => spent = value; }
        public string ExpenseName { get => expenseName; set => expenseName = value; }
        public string GroupName { get => groupName; set => groupName = value; }
    }
}
