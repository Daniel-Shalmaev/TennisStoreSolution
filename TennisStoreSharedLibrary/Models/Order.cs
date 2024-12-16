using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisStoreSharedLibrary.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; } // Relationship: Many-to-One with Customer

        public List<OrderItem>? Items { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required, Range(1, 99999)]
        public int Quantity { get; set; }

        [Required, Range(0.1, 99999.99)]
        public double Price { get; set; }
    }
}
