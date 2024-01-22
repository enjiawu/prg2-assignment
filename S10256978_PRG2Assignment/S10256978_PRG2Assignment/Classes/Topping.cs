//==========================================================
// Student Number : S10262410
// Student Name : Xue Wenya
// Partner Name : Wu Enjia
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256978_PRG2Assignment.Classes
{
    internal class Topping
    {
        // Declaring properties 
        public string Type {  get; set; }

        // Constructors
        public Topping() { }
        public Topping(string t) 
        {
            Type = t; 
        }

        public override string ToString()
        {
            return $"Type: {Type}\n";
        }
    }
}
