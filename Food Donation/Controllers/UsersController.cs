using Food_Donation.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Donation.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(User user)
        {
            string constr = @"Server=LAPTOP-JHUBDUV5; Database=FoodDonation; Integrated Security= True";
            int rowAffacted;
            SqlConnection conn = new SqlConnection(constr);
            string query = "INSERT INTO Users(UserName, Email, Password, RoleId) VALUES('" + user.UserName + "','" + user.Email + "', '"+user.Password+"', 1)";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            rowAffacted = cmd.ExecuteNonQuery();
            conn.Close();

            if (rowAffacted > 0)
            {
                TempData["message"] = "Account Created Successfully";
                return View();
            }
            else
            {
                TempData["message"] = "Acount Creation Failed !";
                return View();
            }
        }

    }
    
}
