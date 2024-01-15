using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256978_PRG2Assignment.Classes
{
    internal class Flavour
    {
        // Declaring properties 
        public string Type { get; set; }
        public bool Premium { get; set; }
        public int Quantity { get; set; }

        // Constructors
        public Flavour() { }
        public Flavour(string t, bool p, int q) 
        { 
            Type = t; 
            Premium = p;
            Quantity = q;
        }

        public override string ToString()
        {
            return $"Type: {Type}\tPremium: {Premium}\tQuantity: {Quantity}";
        }
    }
}
