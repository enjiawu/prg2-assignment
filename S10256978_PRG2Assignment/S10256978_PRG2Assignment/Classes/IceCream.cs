//==========================================================
// Student Number : S10262410
// Student Name : Xue Wenya
// Partner Name : Wu Enjia
//==========================================================

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

            Dictionary<string, double> flavourData = Program.flavourData; //Dictionary to store information on flavour and respective cost

            foreach (Flavour flavour in Flavours) //Calculating costs of all flavours
            {
                flavoursPrice += flavourData[flavour.Type] * flavour.Quantity;
            }
            return flavoursPrice; //Return price of flavours
        }

        public double CalculateToppings() //To read data of toppings from toppings.csv and calculate the total costs of toppings
        {
            //Retrieving information of flavours from toppings.csv
            double toppingsPrice = 0.00; //To keep track of the additional price of all toppings

            Dictionary<string, double> toppingData = Program.toppingData; //Dictionary to store information on toppings and respective cost

            foreach (Topping topping in Toppings) //Calculating costs of all toppings
            {
                toppingsPrice += toppingData[topping.Type];
            }
            return toppingsPrice; //Return price of flavours
        }

        public Dictionary<string, List<string>> ReturnOption() //To read data from options.csv and return them compiled in a dictionary 
        {
            Dictionary<string, List<string>> optionData = new Dictionary<string, List<string>> (); //Dictionary to keep track of all the different options for Cups, Cones and Waffles
            List<string> cupOptions = new List<string>(); //List to keep information on all the available options for cups
            List<string> coneOptions = new List<string>(); //List to keep information on all the available options for cones
            List<string> waffleOptions = new List<string>(); //List to keep information on all the available options for waffles

            using (StreamReader sr = new StreamReader("options.csv"))
            {
                string header = sr.ReadLine(); //Reading header
                string? s;

                while((s = sr.ReadLine()) != null) //Reading all the details about each option
                {
                    string[] line = s.Split(','); 
                    string option = line[0]; 
                    int scoops = Convert.ToInt32(line[1]);               
                    double cost = Convert.ToDouble(line[4]);

                    string optionInfo; 

                    if (option == "Cup") //if option is cup, add option to cupOptions List
                    {
                        optionInfo = $"{option},{scoops},{cost}";
                        cupOptions.Add(optionInfo); //Add cup option into cupOptions
                    }
                    else if (option == "Cone") //if option is cone, add option to coneOptions List
                    {
                        optionInfo = $"{option},{scoops},{line[2]},{cost}";
                        coneOptions.Add(optionInfo); //Add cup option into cupOptions
                    }
                    else
                    {
                        optionInfo = $"{option},{scoops},{line[3]},{cost}";
                        waffleOptions.Add(optionInfo); //Add cup option into cupOptions
                    }
                }
            }

            //Adding option lists into the main optionData dictionary
            optionData["Cup"] = cupOptions;
            optionData["Cone"] = coneOptions;
            optionData["Waffle"] = waffleOptions;

            return optionData;
        }

        public override string ToString()
        {
            return $"\n------------------------------------\n" +
                $"Option: {Option}\n" +
                $"Scoops: {Scoops}\n" +
                $"==========\n" +
                $"Flavours:\n" +
                $"{string.Join("", Flavours)}" +
                $"==========\n" +
                $"Toppings:\n" +
                $"==========\n" +
                $"{string.Join("", Toppings)}" +
                $"------------------------------------\n";
        }
    }
}
