using S10256978_PRG2Assignment.Classes;
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
        public abstract double CalculatePrice();

        public double CalculateFlavours() //Method to read flavours.csv and calculate additional price of all flavours
        {
            //Retrieving information of flavours from flavours.csv
            double flavoursPrice = 0.00; //To keep track of the additional price of all flavours
            Dictionary<string, double> flavourData = new Dictionary<string, double>(); //Dictionary to store information on flavour and respective cost

            using (StreamReader sr = new StreamReader("flavours.csv"))
            {
                string header = sr.ReadLine(); //Reading header
                string? s;

                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split(","); //splitting the line into flavour and cost
                    string flavour = line[0];
                    double cost = Convert.ToDouble(line[1]);

                    flavourData[flavour] = cost; //Adding flavour and respective cost into flavourData
                }
            }

            foreach (Flavour flavour in Flavours) //Calculating costs of all flavours
            {
                flavoursPrice += flavourData[flavour.Type] * flavour.Quantity;
            }
            return flavoursPrice; //Return price of flavours
        }

        public double CalculateToppings()
        {
            //Retrieving information of flavours from toppings.csv
            double toppingsPrice = 0.00; //To keep track of the additional price of all toppings
            Dictionary<string, double> toppingData = new Dictionary<string, double>(); //Dictionary to store information on toppings and respective cost

            using (StreamReader sr = new StreamReader("toppings.csv"))
            {
                string header = sr.ReadLine(); //Reading header
                string? s;

                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split(","); //splitting the line into topping and cost
                    string topping = line[0];
                    double cost = Convert.ToDouble(line[1]);

                    toppingData[topping] = cost; //Adding topping and respective cost into toppingsData
                }
            }

            foreach (Topping topping in Toppings) //Calculating costs of all toppings
            {
                toppingsPrice += toppingData[topping.Type];
            }
            return toppingsPrice; //Return price of flavours
        }

        /*public string ReturnOption()
        {
            Dictionary<>
            using (StreamReader sr = new StreamReader("options.csv"))
            {
                string header = sr.ReadLine(); //Reading header
                string? s;

                while((s = sr.ReadLine()) != null) //Reading all the details about each option
                {
                    string[] line = s.Split(','); 
                    string option = line[0]; 
                    int scoops = Convert.ToInt32(line[1]);
                   
                    if (line[2] != "")
                    {
                        bool dipped = Convert.ToBoolean(line[2]);
                    }

                    if (line[3] != "")
                    {
                        string waffleFlavour = line[3]; 
                    }

                    double cost = Convert.ToDouble(line[3]); 
                }


            }
        }*/

        public override string ToString()
        {
            return $"Option: {Option}\tScoops: {Scoops}\tFlavours: {string.Join(", ", Flavours)}\tToppings: {string.Join(", ", Toppings)}\n";
        }
    }
}
