using ASPChushka.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPChushka.Models
{
    public class ProductsVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public decimal Price { get; set; }

        public string Description { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public TypeFood Type { get; set; }

        public virtual ICollection<SelectListItem> Orders { get; set; }
    }
}
