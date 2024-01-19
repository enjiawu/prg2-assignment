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
            double scoopPrice;

            if (Scoops == 1)
            {
                scoopPrice = 4.00; //it should be $4?
            }
            else if (Scoops == 2) 
            {
                scoopPrice = 5.50;
            }
            else if (Scoops == 3)
            {
                scoopPrice = 6.50;
            }
            else
            {
                scoopPrice = 0.00; 
                Console.WriteLine("Warning: Invalid number of scoops. Please enter a number from 1 to 3.");
            }

            double price = scoopPrice + Flavours.Count * 2.00 + Toppings.Count * 1.00;
            return price;
        }
        public override string ToString()
        {
            return $"{base.ToString()}\tPrice: {CalculatePrice()}";
        }
    }
}
