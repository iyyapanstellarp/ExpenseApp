using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ExpenseApp.Context;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ExpenseApp.Business;
using ExpenseApp.Models;

namespace ExpenseApp.Controllers
{
    public class ExpenseLoginController : Controller
    {
        private ExpenseContext _billingContext;
        private readonly IConfiguration _configuration;

        public ExpenseLoginController(ExpenseContext billingContext, IConfiguration configuration)
        {
            _billingContext = billingContext;
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExpenseLogin()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();

        }

        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            return RedirectToAction("ExpenseLogin", "ExpenseLogin");
        }


        [HttpPost]
        public async Task<IActionResult> ExpenseLogin(UserMaster model)
        {
            var login = await _billingContext.EXPusermaster.FirstOrDefaultAsync(x => x.Username == model.Username);

            if (login != null)
            {
                if (login.Password == model.Password)
                {
                    login.Username = model.Username;

                    login.Password = model.Password;

                    List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, login.Username),
                    new Claim("OtherProperties", "Example Role")
                };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme
                );
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                    TempData["UserName"] = model.Username;

                    return RedirectToAction("Index", "Home");

                }

            }
            ViewBag.Message = "Credentials are Incorrect ";

            return View();


        }

        public async Task<IActionResult> ExpenseRegister(UserMaster model)
        {

            var checkreg = await _billingContext.EXPusermaster.FirstOrDefaultAsync(x=>x.Username == model.Username);
            if(checkreg == null)
            {
   
                ViewBag.Message = "Registered Successfully";
                _billingContext.EXPusermaster.Add(model);

            }
            else
            {
                ViewBag.Message = "Username Already Registered!! Try another username";
            }
          await  _billingContext.SaveChangesAsync();

            return View("ExpenseLogin");

        }
        }
    }
