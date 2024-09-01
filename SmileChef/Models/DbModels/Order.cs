using ChefApp.Models.DbModels;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmileChef.Models.DbModels
{
    [Table("Orders")]
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        private List<OrderLine> _orderLines = new List<OrderLine>();
        public List<OrderLine> OrderLines => _orderLines;

        [ForeignKey(nameof(Chef))]
        public int ChefId { get; set; }

        public Chef Chef { get; set; }

        public void AddOrUpdateOrderLine(OrderLine orderLine)
        {
            ArgumentNullException.ThrowIfNull(orderLine, nameof(orderLine)); // Throws if orderLine is null
            var existingOrderLine = _orderLines.FirstOrDefault(ol => ol.RecipeId == orderLine.RecipeId);
            if (existingOrderLine == null)
            {
                _orderLines.Add(orderLine);
            }
            else
            {
                existingOrderLine.Quantity += orderLine.Quantity;
                existingOrderLine.Price = orderLine.Price * existingOrderLine.Quantity;
            }
            CalculateTotalPrice();
        }

        public bool RemoveOrderLine(int indexOfOrderLine)
        {
            // Ensure the index is within bounds
            if (indexOfOrderLine < 0 || indexOfOrderLine >= _orderLines.Count)
            {
                return false; // Index out of range
            }

            // Remove the order line at the specified index
            _orderLines.RemoveAt(indexOfOrderLine);
            CalculateTotalPrice(); // Recalculate the total price after removal
            return true;
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = _orderLines.Sum(ol => ol.Price);
        }
    }
}
