using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Donation.Models
{
    public class Delivery
    {
        public int DeliveryId { set; get; }
        public string DeliveryManName { set; get; }
        public string DeliveryManPhone { set; get; }
        public int DonationId { set; get; }

    }
}
