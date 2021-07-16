using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_DSG.Models
{
    public class Membership
    {
        [Key]
        public int MembershipId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

        public string Status { get; set; }

        public virtual Group Group { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}