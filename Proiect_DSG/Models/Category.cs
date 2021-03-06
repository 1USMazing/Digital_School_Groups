using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect_DSG.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Campul de nume este obligatoriu!")]
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}