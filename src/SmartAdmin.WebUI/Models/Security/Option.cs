using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SideXC.WebUI.Models.Security
{
    public class Option
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="The Description field is required.")]
        [StringLength(50,ErrorMessage = "The Max legnth is 50 characters for this field.")]
        public int Description { get; set; }
        [StringLength(50, ErrorMessage = "The Max legnth is 50 characters for this field.")]
        public string FaIcon { get; set; }
        [StringLength(500, ErrorMessage = "The Max legnth is 500 characters for this field.")]
        public string Url { get; set; }
        [StringLength(100, ErrorMessage = "The Max legnth is 100 characters for this field.")]
        public string Title { get; set; }
        public virtual Profile Profile { get; set; }
        public bool Active { get; set; }
    }
}
