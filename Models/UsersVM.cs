using ASPChushka.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASPChushka.Models
{
   
    public class UsersVM
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [EnumDataType(typeof(Roles))]
        public Roles Role { get; set; }

        [NotMapped]
        public List<SelectListItem> Orders { get; set; }

    }
}
