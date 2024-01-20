using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256978_PRG2Assignment.Classes
{
    internal class Waffle : IceCream
    {
        // Properties
        public string WaffleFlavour { get; set; }

        // Constructors
        public Waffle() : base() { }
        public Waffle(string o, int s, List<Flavour> f, List<Topping> t, string wf) : base(o, s, f, t)
        {
            WaffleFlavour = wf;
        }

        // Method
        public override double CalculatePrice()
        {
            // Waffle calculation
            double scoopPrice;
            double premiumPrice = 0.00;

            if (Scoops == 1)
            {
                scoopPrice = 7.00;
            }
            else if (Scoops == 2)
            {
                scoopPrice = 8.50;
            }
            else if (Scoops == 3)
            {
                scoopPrice = 9.50;
            }
            else
            {
                scoopPrice = 0.00;
                Console.WriteLine("Warning: Invalid number of scoops. Please enter a number from  1 to 3.");
            }

            double price = scoopPrice + CalculateFlavours() + CalculateToppings();

            if (WaffleFlavour != "Original")
             {
                 price += 3.00;
             }

            return price;
        }

        public override string ToString()
        {
            return $"{base.ToString()}\tWaffle Flavour: {WaffleFlavour}\tPrice: {CalculatePrice()}";
        }
    }
}
