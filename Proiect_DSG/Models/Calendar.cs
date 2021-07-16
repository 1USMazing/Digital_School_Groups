using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Proiect_DSG.Models
{
    public class Calendar
    {
        [ForeignKey("Group")]
        public int CalendarId { get; set; }

        public virtual ICollection<Activity> Activity { get; set; }

        public virtual Group Group { get; set; }
    }
}