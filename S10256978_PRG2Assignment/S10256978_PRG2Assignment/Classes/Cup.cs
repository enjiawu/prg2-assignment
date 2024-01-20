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
            // Cup price calculation
            double optionBasePrice = 0.00;

            List<string> cupOptions = ReturnOption()["Cup"]; //Retrieving cup options available from options.csv
            foreach (string cupOption in cupOptions)
            {
                string[] optionInfo = cupOption.Split(','); //splitting option info into option, scoops and cost
                if (Scoops == Convert.ToInt32(optionInfo[1]))
                {
                    optionBasePrice = Convert.ToDouble(optionInfo[2]);
                    break;
                }
            }

            double price = optionBasePrice + CalculateFlavours() + CalculateToppings();
            return price;
        }
        public override string ToString()
        {
            return $"{base.ToString()}\tPrice: {CalculatePrice()}";
        }
    }
}
