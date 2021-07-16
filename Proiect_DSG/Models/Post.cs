using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_DSG.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        [MinLength(3)][Required(ErrorMessage = "Textbox gol sau contine sub 3 caractere.")]
        public string Text { get; set; }

        public DateTime PostData { get; set; }

        [Required]
        public int GroupId { get; set; }

        public virtual Group Group{ get; set; }
        public virtual ApplicationUser User { get; set; }

        public IEnumerable<SelectListItem> Groups { get; set; }
    }
}