using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaHut.Core.Entity
{
    /// <summary>
    /// Class that represent the object User
    /// authenticated in the system
    /// </summary>
    public class Client
    {
        public string Mail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
