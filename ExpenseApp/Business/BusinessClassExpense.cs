
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ExpenseApp.Context;
using ExpenseApp.Models;
using System.Data;
using System.Data.Common;


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

        // Method to get a list of group IDs for dropdown 
        public List<ExpenseGroupModel> GetGroupid()
        {
            // LINQ query to retrieve group names from the database.
            var groupid = (
                from pr in _billingContext.EXPgroupmaster
                where pr.Active == true
                select new ExpenseGroupModel
                {
                    GroupName = pr.GroupName,
                }).ToList(); // Convert the result to a list.
            return groupid; // Return the list of group names.
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


        public static DataTable DataTable(DbContext context, string sqlQuery, Dictionary<string, object> parameters)
        {
            DataTable dataTable = new DataTable();
            using (var connection = context.Database.GetDbConnection() as SqlConnection)
            {
                if (connection != null)
                {
                    connection.Open();
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        // Add parameters to the command
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dataTable.Load(reader);
                            }
                        }
                    }
                }
            }
            return dataTable;
        }


    }
}
