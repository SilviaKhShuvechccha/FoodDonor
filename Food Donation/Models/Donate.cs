using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Donation.Models
{
    public class Donate
    {
        public int DonationId { set; get; }
        public string DonationTitle { set; get; }
        public string FoodDescription { set; get; }
        public double FoodWeight { set; get; }
        public byte[] FoodPhoto { get; set; }
        public string FileUrl { set; get; }
        public string ContactNo { set; get; }
        public string Location { set; get; }
        public int DonatedBy { set; get; }
        public string DonationStatus { set; get; } = "Pending";
        public string DonorName { set; get; }
    }
}
