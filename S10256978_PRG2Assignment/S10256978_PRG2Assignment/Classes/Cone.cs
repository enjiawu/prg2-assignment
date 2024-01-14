using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256978_PRG2Assignment.Classes
{
    internal class Cone
    {
        // Properties
        public bool Dipped { get; set; }

        // Constructors
        public Cone() : base() { }
        public Cone(string o, int s, List<Flavour> f, List<Topping> t, bool d) :base(o, s, f, t) 
        {
            Dipped = d;
        }

        // Methods
        public override double CalculatePrice()
        {
            // Cone Calculation
            return base.CalculatePrice();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
