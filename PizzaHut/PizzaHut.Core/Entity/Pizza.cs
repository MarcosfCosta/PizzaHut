using PizzaHut.Core.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaHut.Core.Entity
{
    /// <summary>
    /// Class that represent the object Pizza
    /// </summary>
    public class Pizza
    {
        public PizzaType Type { get; set; }

        public Amount Amount { get; set; }
    }
}
