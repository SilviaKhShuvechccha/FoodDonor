using Food_Donation.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Donation.Controllers
{
    public class DonationController : Controller
    {
        IWebHostEnvironment Environment;
        public DonationController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
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
        public IActionResult Create(Donate donate, IFormFile Photo)
        {
            donate.FileUrl = SaveFile(Photo);
            string constr = @"Server=LAPTOP-JHUBDUV5; Database=FoodDonation; Integrated Security= True";
            int rowAffacted;
            SqlConnection conn = new SqlConnection(constr);
            string query = "INSERT INTO Donate(DonationTitle, FoodDescription, FoodWeight, FileUrl, Location, ContactNo, DonatedBy, DonationStatus) VALUES('" + donate.DonationTitle + "','" + donate.FoodDescription + "', '" + donate.FoodWeight + "', '" + donate.FileUrl + "', '" + donate.Location + "', '" + donate.ContactNo + "', 1, '" + donate.DonationStatus + "')";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            rowAffacted = cmd.ExecuteNonQuery();
            conn.Close();

            if (rowAffacted > 0)
            {
                TempData["message"] = "Donation Submitted Successfully";
                return RedirectToAction("index", "Home");
            }
            else
            {
                TempData["message"] = "Donation Submission Failed !";
                return View();
            }

        }
        [HttpGet]
        public IActionResult Receive(int id)
        {
            ViewBag.donationId = id;
            return View();
        }
        [HttpPost]
        public IActionResult Receive(Delivery delivery)
        {
            string constr = @"Server=LAPTOP-JHUBDUV5; Database=FoodDonation; Integrated Security= True";
            int rowAffacted;
            SqlConnection conn = new SqlConnection(constr);
            string query = "INSERT INTO Delivery(DeliveryManName, DeliveryManPhone, DonationId) VALUES('" + delivery.DeliveryManName + "','" + delivery.DeliveryManPhone + "', '" + delivery.DonationId + "')";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            rowAffacted = cmd.ExecuteNonQuery();
            conn.Close();

            if (rowAffacted > 0)
            {
                ChangeStatus(delivery.DonationId);
                TempData["message"] = "Donation Accepted Successfully";
                return RedirectToAction("index", "Home");
            }
            else
            {
                TempData["message"] = "Can't Accept this donation";
                return View();
            }
        }
        public IActionResult Delete(int id)
        {
            string constr = @"Server=LAPTOP-JHUBDUV5; Database=FoodDonation; Integrated Security= True";
            int rowAffacted;
            SqlConnection conn = new SqlConnection(constr);
            string query = "DELETE FROM Donate WHERE DonationId = " + id + "";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            rowAffacted = cmd.ExecuteNonQuery();
            conn.Close();

            if (rowAffacted > 0)
            {
                TempData["message"] = "Donation Deleted Successfully";
                return RedirectToAction("index", "Home");
            }
            else
            {
                TempData["message"] = "Can't Delete this donation";
                return View();
            }
        }
        public IActionResult View(int id)
        {
            string constr = @"Server=LAPTOP-JHUBDUV5; Database=FoodDonation; Integrated Security= True";
            SqlConnection conn = new SqlConnection(constr);
            string query = "SELECT * FROM Donate WHERE DonationId = " + id + "";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            Donate donate = new Donate
            {
                DonationId = int.Parse(reader["DonationId"].ToString()),
                DonationTitle = reader["DonationTitle"].ToString(),
                FoodDescription = reader["FoodDescription"].ToString(),
                FoodWeight = double.Parse(reader["FoodWeight"].ToString()),
                FileUrl = reader["FileUrl"].ToString(),
                Location = reader["Location"].ToString(),
                ContactNo = reader["ContactNo"].ToString(),
                DonatedBy = int.Parse(reader["DonatedBy"].ToString()),
                DonationStatus = reader["DonationStatus"].ToString()
            };
            reader.Close();
            conn.Close();
            ViewBag.deliveryMan = GetDeliveryManInformation(id);

            return View(donate);
        }
        private string SaveFile(IFormFile image)
        {
            var uniqueFileName = GetUniqueFileName(image.FileName);
            string fileUrl = "/uploads/" + uniqueFileName;
            var uploads = Path.Combine(Environment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            image.CopyTo(new FileStream(filePath, FileMode.Create));
            return fileUrl;

        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        private bool ChangeStatus(int donationId)
        {
            string constr = @"Server=LAPTOP-JHUBDUV5; Database=FoodDonation; Integrated Security= True";
            int rowAffacted;
            SqlConnection conn = new SqlConnection(constr);
            string query = "UPDATE Donate SET DonationStatus = 'Received' WHERE DonationId = " + donationId + "";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            rowAffacted = cmd.ExecuteNonQuery();
            conn.Close();
            return rowAffacted > 0;

        }
        private Delivery GetDeliveryManInformation(int donationId)
        {
            string constr = @"Server=LAPTOP-JHUBDUV5; Database=FoodDonation; Integrated Security= True";
            SqlConnection conn = new SqlConnection(constr);
            string query = "SELECT * FROM Delivery WHERE DonationId = " + donationId + "";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Delivery delivery = null;
            if(reader.Read())
            {
                delivery = new Delivery
                {
                    DonationId = int.Parse(reader["DonationId"].ToString()),
                    DeliveryManName = reader["DeliveryManName"].ToString(),
                    DeliveryManPhone = reader["DeliveryManPhone"].ToString(),

                };
            }
            reader.Close();
            conn.Close();
            return delivery;
        }
    }
}
