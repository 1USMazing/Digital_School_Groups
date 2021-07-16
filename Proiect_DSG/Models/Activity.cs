using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect_DSG.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        public string ActivityName { get; set; }

        public string ActivityDescription { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public int CalendarId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Calendar Calendar { get; set; }
    }
}