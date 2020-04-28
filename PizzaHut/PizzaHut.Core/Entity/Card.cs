using PizzaHut.Core.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaHut.Core.Entity
{
    public class Card
    {
        public string CardNumber { get; set; }
        public CardType CardType { get; set; }
        public string ExpiryDate { get; set; }
    }
}
