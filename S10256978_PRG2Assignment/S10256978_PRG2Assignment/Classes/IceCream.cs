using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256978_PRG2Assignment.Classes
{
    abstract class IceCream
    {
        //Declaring properties 
        public string Option { get; set; }
        public int Scoops { get; set; }
        public List<Flavour> Flavours { get; set; } = new List<Flavour>();
        public List<Topping> Toppings { get; set; } = new List<Topping>();


        //Constructors
        public IceCream() {}
        public IceCream(string o, int s, List<Flavour> f, List<Topping> t)
        {
            Option = o;
            Scoops = s;
            Flavours = f;
            Toppings = t;
        }

        // Methods
        public virtual double CalculatePrice()
        {
            double price = 0;
            return price;
        }

        public override string ToString()
        {
            return $"Option: {Option}, Scoops: {Scoops}, Flavours: {string.Join(", ", Flavours)}, Toppings: {string.Join(", ", Toppings)}";
        }
    }
}
