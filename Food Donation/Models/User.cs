using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Donation.Models
{
    public class User
    {
        public int UserId { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public int RoleId { set; get; }


    }
}
