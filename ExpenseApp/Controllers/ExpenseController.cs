using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ExpenseApp.Business;
using ExpenseApp.Context;
using ExpenseApp.Models;
using System.Data;
using System.Globalization;
using System.Web;


namespace ExpenseApp.Controllers
{
    public class ExpenseController : Controller
    {

        // Private field for the database context
        private ExpenseContext _billingContext;

        // Private field for configuration settings
        private readonly IConfiguration _configuration;

        // Constructor to initialize database context and configuration
        public ExpenseController(ExpenseContext billingContext, IConfiguration configuration)
        {
            _billingContext = billingContext;
            _configuration = configuration;
        }

        // Action method to add or update an expense group
        public async Task<IActionResult> Addgroup(ExpenseGroupModel grpmodel)
        {
            string username = null; // Variable to hold the username

            // Retrieve username from TempData if available
            if (TempData["UserName"] != null)
            {
                username = TempData["UserName"].ToString(); // Assign username from TempData
                TempData.Keep("UserName"); // Keep TempData value for further use
            }

            // Check if a group with the same name already exists in the database
            var checkgrp = await _billingContext.EXPgroupmaster
                                .FirstOrDefaultAsync(x => x.GroupName == grpmodel.GroupName);
            if (checkgrp != null) // If group exists
            {
                checkgrp.Active = grpmodel.Active;
                checkgrp.ModifiedDate = DateTime.Now.ToString(); // Set modified date
                checkgrp.CreatedBy = username; // Set created by
                _billingContext.Entry(checkgrp).State = EntityState.Modified; // Mark as modified
            }
            else // If group doesn't exist
            {
                grpmodel.CreatedDate = DateTime.Now.ToString(); // Set created date
                grpmodel.ModifiedDate = DateTime.Now.ToString(); // Set modified date
                grpmodel.CreatedBy = username; // Set created by
                _billingContext.EXPgroupmaster.Add(grpmodel); // Add new group
            }

            ViewBag.Message = "Saved Successfully"; // Message for successful save
            await _billingContext.SaveChangesAsync(); // Save changes to the database

            return View("ExpenseGroupMaster"); // Return to the ExpenseGroupMaster view
        }

        // Action method to add or update an user details
        public async Task<IActionResult> updateuser(UserMaster usermodel)
        {
            var checkuser = await _billingContext.EXPusermaster.FirstOrDefaultAsync(x=>x.Username == usermodel.Username);

            if (checkuser != null)
            {
                checkuser.Password = usermodel.Password;
                checkuser.Mobilenumber = usermodel.Mobilenumber;

                _billingContext.Entry(checkuser).State = EntityState.Modified;
                await _billingContext.SaveChangesAsync();
            }
            else
            {
                ViewBag.Message = "User not found!";
            }

            ViewBag.Message = "Saved Successfully";

            return View("UserMaster");
            
        }


        // Action method to add or update an expense spent
        public async Task<IActionResult> addexpense(ExpenseModel expensemodel,string groupID)
        {
            string username = null;

            if (TempData["UserName"] != null)
            {
                username = TempData["UserName"].ToString();
                TempData.Keep("UserName");
            }

            // Create an instance of the business class to handle business logic.

            BusinessClassExpense business = new BusinessClassExpense(_billingContext, _configuration);

            // Populate ViewData with group IDs to display in the view (e.g., dropdown).
            ViewData["groupid"] = business.GetGroupid();

            var checkexpense = await _billingContext.EXPexpense.FirstOrDefaultAsync(x=>x.ExpenseName == expensemodel.ExpenseName && x.GroupName == groupID);

            // Create an instance of ExpenseFormModel for database interaction
            ExpenseFormModel expenseFormModel = new ExpenseFormModel
            {
                ExpenseName = expensemodel.ExpenseName,
                GroupName = groupID,
                Spent = expensemodel.Spent,
                CreatedBy = username,
                ModifiedDate = DateTime.Now.ToString(),
                CreatedDate = DateTime.Now.ToString()
            };

            if (checkexpense != null)
            {
                checkexpense.Spent = expensemodel.Spent;
                checkexpense.ModifiedDate = DateTime.Now.ToString();
                checkexpense.CreatedBy = username;

                _billingContext.Entry(checkexpense).State = EntityState.Modified;
            }
            else
            {
                _billingContext.EXPexpense.Add(expenseFormModel);

            }

            await _billingContext.SaveChangesAsync();
            ViewBag.Message = "Saved Successfully";

            return View("Expense");

        }


        // Action method to add or update an expense type
        public async Task<IActionResult> AddExpensePop(ExpenseMasterModel model, string expenseName, string type)
        {

            BusinessClassExpense business = new BusinessClassExpense(_billingContext, _configuration);

            ViewData["groupid"] = business.GetGroupid();

            var checkmaster = await _billingContext.EXPexpensemaster.FirstOrDefaultAsync(x=>x.ExpenseName == expenseName);

            if(checkmaster != null)
            {
                checkmaster.ExpenseType = type;
                _billingContext.Entry(checkmaster).State = EntityState.Modified;
            }
            else
            {
                model.ExpenseName = expenseName;
                model.ExpenseType = type;

                _billingContext.EXPexpensemaster.Add(model);
            }

            await _billingContext.SaveChangesAsync();

            return View("Expense");

        }


        public IActionResult ExpenseMemberMaster()
        {
            BusinessClassExpense business = new BusinessClassExpense(_billingContext, _configuration);

            ViewData["groupid"] = business.GetGroupid();

            ExpenseMemberMasterModel model = new ExpenseMemberMasterModel();

           
            string groupName = model.Groupname;
            ViewData["Memberdata"] = AdditionalMemberMasterFun(groupName);

            return View();
        }

        public async Task<DataTable> AdditionalMemberMasterFun(string groupName)
        {
           
            // Step 1: Perform the query
            var entities = _billingContext.EXPmembermaster
                                  .Where(e => e.IsDelete == false && e.Groupname == groupName)
                                  .ToList();

            // Step 2: Convert to DataTable
            return BusinessClassExpense.convertToDataTableMemberMaster(entities);


        }


        public async Task<IActionResult> Addmember(ExpenseMemberMasterModel membermodel,string buttonType)
        {
            string username = null;

            if (TempData["UserName"] != null)
            {
                username = TempData["UserName"].ToString();
                TempData.Keep("UserName");
            }

            BusinessClassExpense business = new BusinessClassExpense(_billingContext, _configuration);

            ViewData["groupid"] = business.GetGroupid();

            if (buttonType == "Save")
            {

                var checkmember = await _billingContext.EXPmembermaster.FirstOrDefaultAsync(x => x.Groupname == membermodel.Groupname && x.Membersname == membermodel.Membersname);

                if (checkmember != null)
                {
                    checkmember.Contributionamaount = membermodel.Contributionamaount;
                    checkmember.ModifiedDate = DateTime.Now.ToString();
                    checkmember.CreatedBy = username;

                    _billingContext.Entry(checkmember).State = EntityState.Modified;

                }
                else
                {

                    membermodel.CreatedDate = DateTime.Now.ToString();
                    membermodel.ModifiedDate = DateTime.Now.ToString();
                    membermodel.CreatedBy = username;
                    _billingContext.EXPmembermaster.Add(membermodel);
                }
                ViewBag.Message = "Saved Successfully";
                await _billingContext.SaveChangesAsync();

                var dataTable = await AdditionalMemberMasterFun(membermodel.Groupname);

                // Store the DataTable in ViewData for access in the view
                ViewData["Memberdata"] = dataTable;
            }

            if(buttonType == "Delete")
            {
                var membertodelete = await _billingContext.EXPmembermaster.FirstOrDefaultAsync(x => x.Groupname == membermodel.Groupname && x.Membersname == membermodel.Membersname && x.IsDelete == false);

                if (membertodelete != null)
                {
                    membertodelete.IsDelete = true;
                    _billingContext.Entry(membertodelete).State = EntityState.Modified;
                    await _billingContext.SaveChangesAsync();

                    ViewBag.Message = "Member deleted successfully";

                    var dataTable1 = await AdditionalMemberMasterFun(membermodel.Groupname);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Memberdata"] = dataTable1;

                }
                else
                {
                    ViewBag.Message = "Member Not Found";

                    var dataTable1 = await AdditionalMemberMasterFun(membermodel.Groupname);

                    // Store the DataTable in ViewData for access in the view
                    ViewData["Memberdata"] = dataTable1;
                }
            }

            return View("ExpenseMemberMaster");
        }


            public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserMaster()
        {
            string username = null;

            if (TempData["UserName"] != null)
            {
                username = TempData["UserName"].ToString();
                TempData.Keep("UserName");
            }
            var usermodel = new UserMaster
            {
                Username = username
            };

            return View(usermodel);
        }

        public IActionResult ExpenseGroupMaster()
        {
            return View();
        }

        public IActionResult Expense()
        {
            BusinessClassExpense business = new BusinessClassExpense(_billingContext, _configuration);

            ViewData["groupid"] = business.GetGroupid();


            return View();
        }

        public IActionResult ExpenseBalance()
        {
            BusinessClassExpense business = new BusinessClassExpense(_billingContext, _configuration);

            ViewData["groupid"] = business.GetGroupid();


            return View();
        }

        public IActionResult GetReports(ExpenseGroupModel model, string GroupBy)
        {
            BusinessClassExpense business = new BusinessClassExpense(_billingContext, _configuration);

            ViewData["groupid"] = business.GetGroupid();

            // Define the SQL query with a parameter placeholder
            var reportQuery = "select Membersname, Contributionamaount, '' as percentage, '' as balance " +
                              "from EXPmembermaster where Groupname = @GroupName";

            if (string.IsNullOrEmpty(model.GroupName))
            {
                ViewBag.Message = "Group Name is required.";
                return View("ExpenseBalance");
            }

            // Use parameterized query to prevent SQL injection
            var query = BusinessClassExpense.DataTable(_billingContext, reportQuery, new Dictionary<string, object>
    {
        { "@GroupName", model.GroupName }
    });

            if (query.Rows.Count == 0)
            {
                ViewBag.Message = "No records found for the specified Group Name.";
                return View("Reports");
            }

            ViewData["balancetable"] = query;

            return View("ExpenseBalance");
        }



    }
}
