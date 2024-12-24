using System.Data.SqlTypes;

namespace ExpenseApp.Models
{
    public class ExpenseGroupModel
    {
        public ExpenseGroupModel() { }

        private int groupID;
        private String groupName;
        private Double budget;
        private String createdDate;
        private String modifiedDate;
        private String createdBy;
        private bool active;


   
       
        public string? CreatedDate { get => createdDate; set => createdDate = value; }
        public string? ModifiedDate { get => modifiedDate; set => modifiedDate = value; }
        public string? CreatedBy { get => createdBy; set => createdBy = value; }
        public string GroupName { get => groupName; set => groupName = value; }
        public double Budget { get => budget; set => budget = value; }
        public bool Active { get => active; set => active = value; }
        public int GroupID { get => groupID; set => groupID = value; }
    }
}
