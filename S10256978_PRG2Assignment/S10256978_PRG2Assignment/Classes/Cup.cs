using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256978_PRG2Assignment.Classes
{
    internal class Cup : IceCream
    {
        // Constructors
        public Cup(): base() { }
        public Cup(string o, int s,List<Flavour> f, List<Topping> t): base(o, s, f, t) { }

        // Methods
        public override double CalculatePrice()
        {
            // cup calculation
            return base.CalculatePrice();
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
