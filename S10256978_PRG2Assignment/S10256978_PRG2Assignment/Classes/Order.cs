//==========================================================
// Student Number : S10256978
// Student Name : Wu Enjia
// Partner Name : Xue Wenya
//==========================================================

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace S10256978_PRG2Assignment.Classes
{
    internal class Order
    {
        //Declaring properties 
        public int Id { get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFulfilled { get; set; } 
        public List<IceCream> IceCreamList { get; set; } = new List<IceCream>();

        //Constructors
        public Order() { }
        public Order(int i, DateTime tr)
        {
            Id = i;
            TimeReceived = tr;
        }

        //Methods
        public void ModifyIceCream(int i) 
        {
            Dictionary<string, double> flavourData = Program.flavourData; //Dictionary to store information on flavour and respective cost
            Dictionary<string, double> toppingData = Program.toppingData; //Dictionary to store information on toppings and respective cost

            void ChangeFlavours(IceCream ic) //Function to add flavours
            {
                IceCream icecream = ic;
                while (true) //Iterating through the loop until user does not want to modify flavours anymore
                {
                    string currentFlavours = "";
                    int count = 1;
                    foreach (Flavour f in icecream.Flavours)
                    {
                        currentFlavours += $"[{count}] {f}\n";
                    }

                    Console.Write("Current Flavours:\n" + //Print out current flavours
                        "----------------\n" +
                        $"{currentFlavours}\n" +
                        $"Enter flavour to modify: ");
                    int modifyOption = Convert.ToInt32(Console.ReadLine());
                    Flavour flavour = icecream.Flavours[modifyOption - 1]; //Get flavour to modify

                    for (int x = 0; x < flavour.Quantity; x++) //Iterating through the quantity of the selected flavour (in case they want to select two different flavours)
                    {
                        int flavourCount = 0; //Count the quantity of flavours entered
                        int scoopCount = flavour.Quantity; //Count the number of flavours to add  
                        for (int i = 0; i < icecream.Scoops; i++) //getting flavours for new ice cream
                        {
                            //Printing out available flavours
                            Console.Write("\nAvailable Flavours\n" +
                                        "----------------\n");
                            foreach (string f in flavourData.Keys)
                            {
                                Console.WriteLine($"{f}");
                            }
                            Console.WriteLine();

                            string flavourOption;
                            while (true) //Making sure that all scoops have a selected flavour
                            {
                                while (true) //Data validation for flavour option
                                {
                                    try
                                    {
                                        Console.Write("Enter new flavour: ");

                                        flavourOption = Console.ReadLine().ToLower(); //checking if its Sea Salt 
                                        if (flavourOption.Split(" ").Length > 1)
                                        {
                                            string[] words = flavourOption.Split(' ');
                                            for (int y = 0; y < words.Length; y++)
                                            {
                                                words[y] = char.ToUpper(words[y][0]) + words[y].Substring(1);
                                            }
                                            flavourOption = string.Join(" ", words); //Updating the flavour option
                                        }
                                        else
                                        {
                                            flavourOption = char.ToUpper(flavourOption[0]) + flavourOption.Substring(1).ToLower(); //Capitalizing user input
                                        }

                                        if (!flavourData.ContainsKey(flavourOption))
                                        {
                                            Console.WriteLine("No such flavour exists!");
                                            throw new FormatException();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Please enter an available flavour.");
                                    }
                                }

                                //Check for quantity of flavour
                                int quantity;
                                while (true) //For data validation
                                {

                                    try
                                    {
                                        Console.Write("Enter quantity of flavour (0 to choose another flavour) : ");
                                        quantity = Convert.ToInt32(Console.ReadLine());
                                        if (quantity > scoopCount || quantity < 0) //Making sure quantity of selected flavour does not exceed the number of scoops in the ice cream or is negative
                                        {
                                            throw new ArgumentOutOfRangeException();
                                        }
                                        {
                                            scoopCount -= quantity; //Deducting the number of scoops remaining 
                                            break;
                                        }
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.WriteLine("Incorrect format! Please enter a number!");
                                    }
                                    catch (ArgumentOutOfRangeException ex)
                                    {
                                        Console.WriteLine($"Option out of range! Quantity must be lesser or equals to {scoopCount}!");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Quantity must be lesser or equals to {scoopCount}!");
                                    }

                                }

                                if (quantity > 0) //Making sure user entered a quantity
                                {
                                    if (flavourData[flavourOption] > 0) //Check if ice cream is premium
                                    {
                                        if (!icecream.Flavours.Contains(flavour)) //Check if flavour is already in ice cream
                                        {
                                            Flavour newFlavour = new Flavour(flavourOption, true, quantity);
                                            icecream.Flavours.Add(newFlavour); //adding flavour to flavours list
                                        }
                                        else
                                        {
                                            icecream.Flavours[count].Quantity++; //Add 1 to quantity
                                        }
                                    }
                                    else
                                    {
                                        if (!icecream.Flavours.Contains(flavour)) //Check if flavour is already in ice cream
                                        {
                                            Flavour newFlavour = new Flavour(flavourOption, false, quantity);
                                            icecream.Flavours.Add(newFlavour); //adding flavour to flavours list
                                        }
                                        else
                                        {
                                            icecream.Flavours[count].Quantity++; //Add 1 to quantity
                                        }
                                    }
                                    Console.WriteLine("Flavour has been added.");
                                    flavourCount += quantity; //Add the quantity to the number of flavours that have been added 

                                    //Quit for loop if the quantity of flavours have been hit
                                    if (flavourCount >= icecream.Scoops)
                                    {
                                        Console.WriteLine("Maximum number of flavours have been reached.");
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
            }

            void ChangeToppings(IceCream icecream) //Function for changing of toppings
            {
                while (true) //Iterating through the loop until user does not want to modify toppings anymore
                {
                    string toppings = "";
                    int count = 1;
                    foreach (Topping t in icecream.Toppings)
                    {
                        toppings += $"[{count}] {t}\n";
                    }

                    Console.Write("Current Toppings:\n" + //Print out current Toppings
                        "----------------\n" +
                        $"{toppings}\n" +
                        $"Enter toppings to modify: ");
                    int modifyOption = Convert.ToInt32(Console.ReadLine());
                    Topping topping = icecream.Toppings[modifyOption - 1]; //Get topping to modify

                    Console.Write("Available Toppings\n" +
                    "----------------\n" +
                    "[1] Sprinkles\n" +
                    "[2] Mochi\n" +
                    "[3] Sago\n" +
                    "[4] Oreos\n" +
                    "[0] Return\n" +
                    "Enter new topping(s): ");

                    int toppingOption = Convert.ToInt32(Console.ReadLine());
                    if (toppingOption == 0)
                    {
                        break; //break from while loop
                    }

                    //Changing the topping
                    icecream.Toppings.Remove(topping); //Removing the topping that is to be removed from Toppings
                    Topping newTopping;
                    if (toppingOption == 1)
                    {
                        newTopping = new Topping("Sprinkles");
                    }
                    else if (toppingOption == 2)
                    {
                        newTopping = new Topping("Mochi");
                    }
                    else if (toppingOption == 3)
                    {
                        newTopping = new Topping("Sago");
                    }
                    else
                    {
                        newTopping = new Topping("Oreos");
                    }
                    icecream.Toppings.Add(newTopping); //Adding new toppings 
                }
            }

            void ChangeDipping(IceCream icecream) //Function to change dipping
            {
                if (icecream is Cone)
                {
                    Cone cone = (Cone)icecream; //Need to downcast to access cone properties
                    Console.Write($"Current Dipping: {cone.Dipped}\n" + //Print out if cone is currently dipped
                        "[1] Chocolate-dipped cone\n" +
                        "[2] Regular cone\n" +
                        "[0] Return\n" +
                        "Enter option: ");

                    int coneOption = Convert.ToInt32(Console.ReadLine());

                    if (coneOption == 0)
                    {
                        return;
                    }
                    else if (coneOption == 1)
                    {
                        cone.Dipped = true;
                    }
                    else
                    {
                        cone.Dipped = false;
                    }
                }
                else
                {
                    Console.WriteLine("Ice cream must be a cone!");
                }
            }

            void ChangeWaffleFlavour(IceCream icecream) //Function to change waffle flavour
            {
                if (icecream is Waffle)
                {
                    Waffle waffle = (Waffle)icecream; //Need to downcast to access waffle properties
                    Console.Write($"Current Waffle Flavour: {waffle.WaffleFlavour}\n" + //Print out current waffle flavour
                        "[1] Original\n" +
                        "[2] Red Velvet\n" +
                        "[3] Charcoal\n" +
                        "[4] Pandan Waffle\n" +
                        "[0] Return" +
                        "Enter option: ");

                    int waffleOption = Convert.ToInt32(Console.ReadLine());

                    if (waffleOption == 0)
                    {
                        return;
                    }
                    else if (waffleOption == 1)
                    {
                        waffle.WaffleFlavour = "Original";
                    }
                    else if (waffleOption == 2)
                    {
                        waffle.WaffleFlavour = "Red Velvet";
                    }
                    else if (waffleOption == 3)
                    {
                        waffle.WaffleFlavour = "Charcoal";
                    }
                    else
                    {
                        waffle.WaffleFlavour = "Pandan";
                    }
                }
                else
                {
                    Console.WriteLine("Ice cream must be a waffle!");
                }
            }

            IceCream icecream = IceCreamList[i - 1]; //Getting ice cream from ice cream list

            //Prompting the user for information on the modifications they wish to make to the ice cream selected
            bool modificationsMade = false;
            while (true)
            {
                Console.Write("\nSelect which item you want to modify:\n" +
                            "[1] Option\n" +
                            "[2] Scoop\n" +
                            "[3] Flavours\n" +
                            "[4] Toppings\n" +
                            "[5] Dipped Cone (if applicable)\n" +
                            "[6] Waffle flavour (if applicable)\n" +
                            "[0] Done\n" +
                            "Enter your option: ");
                int option = Convert.ToInt32(Console.ReadLine());

                if (option == 0) //Quit option to return to main program
                {
                    if (modificationsMade) //Check if modifications have been made
                    {
                        Console.WriteLine("Ice Cream has been modified.");
                        break;
                    }
                    Console.WriteLine("No changes have been made.");
                    break;
                }
                else if (option == 1) //Change ice cream option
                {
                    Console.Write($"Current option: {icecream.Option}\n" +
                        "----------------\n" +
                        "[1] Waffle\n" +
                        "[2] Cone\n" +
                        "[3] Cup\n" +
                        "[0] Return\n" +
                        "Enter option: ");
                    int icOption = Convert.ToInt32(Console.ReadLine());

                    if (icOption == 0)
                    {
                        continue;
                    }
                    else if (icOption == 1)
                    {
                        icecream.Option = "Waffle";
                        icecream = new Waffle("Waffle", icecream.Scoops, icecream.Flavours, icecream.Toppings, "Original");

                        ChangeWaffleFlavour(icecream); //Calling change waffle flavour function
                    }
                    else if (icOption == 2)
                    {
                        icecream.Option = "Cone";
                        icecream = new Cone("Waffle", icecream.Scoops, icecream.Flavours, icecream.Toppings, false);
                        ChangeDipping(icecream); //Calling change dipping function
                    }
                    else
                    {
                        icecream.Option = "Cup";
                        icecream = new Cup("Waffle", icecream.Scoops, icecream.Flavours, icecream.Toppings);
                    }
                }
                else if (option == 2) //Change scoop option
                {
                    Console.Write($"Current number of scoops: {icecream.Scoops}\n" +
                        "----------------\n" +
                        "Enter number of scoops between 1-3: ");
                    int scoops = Convert.ToInt32(Console.ReadLine());
                    if (scoops == 1)
                    {
                        icecream.Scoops = 1;
                    }
                    else if (scoops == 2)
                    {
                        icecream.Scoops = 2;
                    }
                    else if (scoops == 3)
                    {
                        icecream.Scoops = 3;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number from 1 to 3!");
                    }

                    int flavoursCount = 0; //To keep track of how many flavours there currently are
                    Dictionary<string, Flavour> icDict = new Dictionary<string, Flavour>(); //To keep track of ice cream flavour and corresponding ice cream
                    foreach (Flavour f in icecream.Flavours)
                    {
                        flavoursCount += f.Quantity;
                        icDict[f.Type] = f;
                    }

                    //To check if ice cream flavours match the new number of scoops
                    if (icecream.Scoops > flavoursCount) //If number of ice cream scoops is more than flavours
                    {
                        int flavoursToRemove = icecream.Scoops - flavoursCount; //Number of flavours to be removed
                        while (flavoursToRemove != 0)
                        {
                            Console.Write($"You currently have too many flavours for {icecream.Scoops} scoops.\n" +
                            $"{icecream.Flavours}\n");
                            while (true)
                            {
                                Console.Write($"Choose ONE ice cream flavour to remove: \n");
                                string icRemove = Console.ReadLine().ToLower(); //IceCream flavour to remove
                                icRemove = char.ToUpper(icRemove[0]) + icRemove.Substring(1).ToLower(); //Capitalizing first letter of input)
                                if ( icDict.ContainsKey(icRemove))
                                {
                                    if (icDict[icRemove].Quantity == 1)
                                    {
                                        icecream.Flavours.Remove(icDict[icRemove]); //Removing from ice cream
                                        icDict.Remove(icRemove); //Removing icecream from icDict
                                    }
                                    else
                                    {
                                        icDict[icRemove].Quantity --; //Deducting quantity by 1
                                    }
                                    flavoursToRemove--; //Deducting from the number of flavours to remove
                                    break; //Breaking out of while loop
                                }
                                else
                                {
                                    Console.WriteLine("There is no flavour in that ice cream!");
                                }
                            }                            
                        }

                    }
                    else if (icecream.Scoops < flavoursCount)
                    {
                        int flavoursToAdd = icecream.Scoops - flavoursCount; //Number of flavours to be added
                        while (flavoursToAdd != 0)
                        {
                            Console.Write($"You currently have insufficient flavours for {icecream.Scoops} scoops.");
                            while (true)
                            {
                                Console.Write("\nAvailable Flavours\n" +
                                "----------------\n" +
                                "Vanilla\n" +
                                "Chocolate\n" +
                                "Strawberry\n" +
                                "Durian\n" +
                                "Ube\n" +
                                "Sea Salt\n" +
                                "Return\n" +
                                "Enter new flavour(s): ");
                                string icAdd = Console.ReadLine(); //IceCream flavour to Add
                                icAdd = char.ToUpper(icAdd[0]) + icAdd.Substring(1).ToLower(); //Capitalizing first letter of input)
                                string[] flavours = { "Vanilla", "Chocolate", "Strawberry", "Durian", "Ube", "Sea salt" };

                                /*
                                if (flavours.Contains(icAdd))
                                {
                                    if (icDict.ContainsKey(icAdd))
                                    {
                                        icDict[icAdd].Quantity++; //Deducting quantity by 1
                                    }
                                    else
                                    {
                                        icDict[icAdd] = 1; //Add new flavour
                                    }
                                    flavoursToAdd++; //Deducting from the number of flavours to remove
                                    break; //Breaking out of while loop
                                }
                                else
                                {
                                    Console.WriteLine("There is no flavour in that ice cream!");
                                }*/
                            }
                        }
                    }
                    
                }
                else if (option == 3) //Change flavours option
                {
                    ChangeFlavours(icecream); //Calling change flavours function
                }
                else if (option == 4) //Change toppings option
                {
                    ChangeToppings(icecream); //Calling change toppings function
                }
                else if (option == 5) //Change cone dipping option
                {
                    ChangeDipping(icecream); //Calling change dipping function
                }
                else //Change waffle flavour option
                {
                    ChangeWaffleFlavour(icecream); //Calling change waffle flavour
                }
            }
        }
        public void AddIceCream(IceCream ic)
        {
            IceCreamList.Add(ic);
        }
        public void DeleteIceCream(int i)
        {
            IceCreamList.RemoveAt(i - 1); //Removing at the index entered
        }
        public double CalculateTotal()
        {
            double total = 0;
            foreach (IceCream icecream in IceCreamList)
            {
                total += icecream.CalculatePrice();
            }
            return total;
        }

        public override string ToString()
        {
            string icecreams = "";
            foreach (IceCream ic in IceCreamList)
            {
                icecreams += ic + "\n";
            }
            return $"Order ID: {Id}\n" +
                $"Time Received: {TimeReceived}\n" +
                $"Time Fulfilled: {TimeFulfilled}\n" +
                $"Ice Cream Details:\n{icecreams}";
        }
    }
}
