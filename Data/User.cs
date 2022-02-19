using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPChushka.Data
{
    public enum RoleType { User, Admin }
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public RoleType Role { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
