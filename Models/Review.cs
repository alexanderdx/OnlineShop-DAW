using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Online_Shop___DAW.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public virtual Product Product { get; set; }
    }
}