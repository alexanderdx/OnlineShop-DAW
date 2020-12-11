using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShopDAW.Models
{
    public enum OrderStatus
    {
        not_accepted,
        accepted,
        en_route,
        delivered
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}