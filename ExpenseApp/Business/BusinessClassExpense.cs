
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ExpenseApp.Context;
using ExpenseApp.Models;
using System.Data;


namespace ExpenseApp.Business
{
    public class BusinessClassExpense
    {
        private readonly ExpenseContext _billingContext;
        private readonly IConfiguration _configuration;

        public BusinessClassExpense(ExpenseContext billingContext, IConfiguration configuration)
        {
            _billingContext = billingContext;
            _configuration = configuration;
        }

        public List<ExpenseGroupModel> GetGroupid()
        {
            var groupid = (
                from pr in _billingContext.EXPgroupmaster
                select new ExpenseGroupModel
                {
                    GroupName = pr.GroupName,
                }).ToList();
            return groupid;
        }

        public static DataTable convertToDataTableMemberMaster(IEnumerable<ExpenseMemberMasterModel> entities)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Groupname", typeof(string));
            dataTable.Columns.Add("Membersname", typeof(string));
            dataTable.Columns.Add("Contributionamaount", typeof(double));

            foreach (var entity in entities)
            {
                dataTable.Rows.Add(entity.Groupname, entity.Membersname,entity.Contributionamaount);
            }

            return dataTable;
        }

    }
}
