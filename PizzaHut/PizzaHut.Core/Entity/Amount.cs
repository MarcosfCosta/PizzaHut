using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PizzaHut.Core.Entity
{
    /// <summary>
    /// Class that represent the object Amount
    /// </summary>
    public class Amount
    {

        public double AmountTrx { get; set; }

        [StringLength(3, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Curreny { get; set; }


    }
}
