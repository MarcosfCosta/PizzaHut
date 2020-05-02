using Newtonsoft.Json;
using PizzaHut.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model
{
    public class Order
    {
        public string BuyerId { get; set; }

        public int SequenceNumber { get; set; }

        [JsonProperty("date")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("status")]
        public OrderStatus OrderStatus { get; set; }

        [JsonProperty("city")]
        public string ShippingCity { get; set; }

        [JsonProperty("street")]
        public string ShippingStreet { get; set; }

        [JsonProperty("card")]
        public Card Card { get; set; }

        [JsonProperty("orderitems")]
        public List<Pizza> OrderItems { get; set; }

        [JsonProperty("total")]
        public Amount Amount { get; set; }

        [JsonProperty("ordernumber")]
        public int OrderNumber { get; set; }
    }
}
