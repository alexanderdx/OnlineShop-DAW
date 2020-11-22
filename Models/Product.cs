using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Online_Shop___DAW.Models
{
    public enum ProductStatus
    {
        pending,
        rejected,
        accepted
    }
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }
        public string Image { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public ProductStatus Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}