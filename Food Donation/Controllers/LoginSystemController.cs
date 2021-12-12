using Food_Donation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Donation.Controllers
{
    public class LoginSystemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            string constr = @"Server=LAPTOP-JHUBDUV5; Database=FoodDonation; Integrated Security= True";
            SqlConnection conn = new SqlConnection(constr);
            string query = "SELECT * FROM Users WHERE Email = '" + email + "' AND Password = '" + password + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                User user = new User
                {
                    UserId = int.Parse(reader["UserID"].ToString()),
                    UserName = reader["UserName"].ToString(),
                    Email = reader["Email"].ToString(),
                    RoleId = int.Parse(reader["RoleId"].ToString()),
                };
                conn.Close();
                reader.Close();
                TempData["message"] = "Login Successfull";
                SetSession(user);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["message"] = "Incorrect User or Password";
                return View();
            }
            
            
        }

        private void SetSession(User user)
        {
            HttpContext.Session.SetString("username", user.UserName);
            HttpContext.Session.SetString("userId", user.UserId+"");
            HttpContext.Session.SetString("role", user.RoleId+"");
        }
        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("role");
            return RedirectToAction("Login", "LoginSystem");
        }


    }
}
