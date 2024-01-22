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
    internal class Cone : IceCream
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
            double optionBasePrice = 0.00;

            List<string> coneOptions = ReturnOption()["Cone"]; //Retrieving cone options available from options.csv
            foreach (string coneOption in coneOptions)
            {
                string[] optionInfo = coneOption.Split(','); //splitting option info into option, scoops, waffle flavour and cost
                if (Scoops == Convert.ToInt32(optionInfo[1]) && Dipped == Convert.ToBoolean(optionInfo[2]))
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
                $"Price: ${CalculatePrice():f2}";
        }
    }
}
