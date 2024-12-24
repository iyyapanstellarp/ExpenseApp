namespace ExpenseApp.Models
{
    public class ExpenseMemberMasterModel
    {
        public ExpenseMemberMasterModel() { }

        private String groupname;
        private String membersname;
        private double contributionamaount;
        private String createdDate;
        private String modifiedDate;
        private String createdBy;
        private bool isDelete;

        public string Groupname { get => groupname; set => groupname = value; }
        public string Membersname { get => membersname; set => membersname = value; }
        public double Contributionamaount { get => contributionamaount; set => contributionamaount = value; }
        public string? CreatedDate { get => createdDate; set => createdDate = value; }
        public string? ModifiedDate { get => modifiedDate; set => modifiedDate = value; }
        public string? CreatedBy { get => createdBy; set => createdBy = value; }
        public bool IsDelete { get => isDelete; set => isDelete = value; }
    }
}
