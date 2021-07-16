using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_DSG.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required (ErrorMessage = "Campul de nume este obligatoriu!")][MaxLength(40, ErrorMessage = "Numele Grupului nu poate avea mai mult de 40 de caractere!")]
        public string GroupName { get; set; }

        public string GroupCreatorId { get; set; }

        //[Required(ErrorMessage = "Campul de nume este obligatoriu!")]
        public string GroupCreatorName { get; set; }

        public string GroupDescription { get; set; }
        
        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual Calendar Calendar { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Membership> Memberships { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }
}