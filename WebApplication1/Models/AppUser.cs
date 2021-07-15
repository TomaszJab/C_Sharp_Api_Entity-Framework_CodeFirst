using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AppUser
    {
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string RefreshTocken { get; set; }
        public DateTime? RefreshTokenExp { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
