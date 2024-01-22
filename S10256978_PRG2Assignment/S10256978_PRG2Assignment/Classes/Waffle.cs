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
            // Waffle price calculation
            double optionBasePrice = 0.00;

            List<string> waffleOptions = ReturnOption()["Waffle"]; //Retrieving waffle options available from options.csv
            foreach (string waffleOption in waffleOptions)
            {
                string[] optionInfo = waffleOption.Split(','); //splitting option info into option, scoops, waffle flavour and cost
                if (Scoops == Convert.ToInt32(optionInfo[1]) && WaffleFlavour == optionInfo[2])
                {
                    optionBasePrice = Convert.ToDouble(optionInfo[3]);
                    break;
                }
            }

            double price = optionBasePrice + CalculateFlavours() + CalculateToppings();
            return price;
        }

        public override string ToString()
        {
            return $"{base.ToString()}" +
                $"Waffle Flavour: {WaffleFlavour}\n" +
                $"==========\n" +
                $"Price: ${CalculatePrice():f2}";
        }
    }
}
