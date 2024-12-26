
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
            // Create a new DataTable to hold the converted data
            var dataTable = new DataTable();

            // Add columns to the DataTable that correspond to the properties of the model
            dataTable.Columns.Add("Groupname", typeof(string));  // Column for Groupname (string type)
            dataTable.Columns.Add("Membersname", typeof(string));  // Column for Membersname (string type)
            dataTable.Columns.Add("Contributionamaount", typeof(double));  // Column for Contributionamount (double type)

            // Iterate through the collection of entities (ExpenseMemberMasterModel) and add each entity's data as a new row
            foreach (var entity in entities)
            {
                // Add a row with values from the current entity
                dataTable.Rows.Add(entity.Groupname, entity.Membersname, entity.Contributionamaount);
            }

            // Return the populated DataTable
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
