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
using System.Security.Cryptography;
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
        public void ModifyIceCream(int i)  //function to modify ice cream
        {
            Dictionary<string, double> flavourData = Program.flavourData; //Dictionary to store information on flavour and respective cost
            Dictionary<string, double> toppingData = Program.toppingData; //Dictionary to store information on toppings and respective cost

            IceCream icecream = IceCreamList[i - 1]; //Getting ice cream from ice cream list

            //Prompting the user for information on the modifications they wish to make to the ice cream selected
            bool modificationsMade = false;
            //Prompting user for all required info to create a new ice cream
            string icOption;
            IceCream modifiedIceCream = icecream; //New modified ice cream that will replace the old one if any changes have been made

            Console.Write($"Current option: {icecream.Option}\n" +
                            "----------------\n" +
                            "Available Options\n" +
                            "----------------\n" +
                            "Cone\n" +
                            "Waffle\n" +
                            "Cup\n");

            while (true) //Data validation for ice cream option
            {
                try
                {
                    Console.Write("Enter ice cream option (0 to continue): "); //If option is the same, user can enter 0 to continue to the next modification

                    icOption = Console.ReadLine().ToLower();

                    if (icOption == "0")
                    {
                        break; //breaking the loop
                    }

                    icOption = char.ToUpper(icOption[0]) + icOption.Substring(1).ToLower();
                    string[] options = { "Cone", "Cup", "Waffle" };

                    if (options.Contains(icOption))//Checking if the option exists
                    {
                        modifiedIceCream.Option = icOption; //Changing the option for the modified ice cream
                        break;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(); //throw new error if option entered does not exist
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Incorrect format! Please enter one of the available options!");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Option not found! Please enter one of the available options!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please enter one of the available options!");
                }
            }
                

            Console.Write($"Current number of scoops: {icecream.Scoops}\n" +
                    "----------------\n"); //print out current number of scoops
            int scoops;
            while (true) //Data validation for number of scoops
            {
                try
                {
                    Console.Write("Enter number of scoops (0 to continue): ");
                    scoops = Convert.ToInt32(Console.ReadLine());

                    if (scoops == 0) //check if scoops is 0, if it is continue
                    {
                        scoops = icecream.Scoops;
                        break; //breaking the loop and continuing with existing number of scoops
                    }
                    else if (scoops > 3 || scoops <= 0) //Check if number of scoops is between 1 and 3
                    {
                        throw new ArgumentOutOfRangeException(); //throw new error if scoops is out of range
                    }
                    else
                    {
                        break; //break from loop 
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Format incorrect! Please enter a number from 1-3!");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Option out of range! Please enter an option from 1-3.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please only enter a number between 1-3.");
                }

            }

            List<Flavour> flavours = new List<Flavour>(); //list of hold all the flavours to be added
            List<Topping> toppings = new List<Topping>(); //list of hold all the toppings to be added

            int flavourCount = 0; //Count the quantity of flavours entered
            int scoopCount = scoops; //Count the number of scoops to add  
            bool noFlavourUpdate = false; //Check if flavour still wants to be updated

            for (int y = 0; y < scoops; y++) //getting flavours for new ice cream
            {
                if (noFlavourUpdate) //check if flavour does not want to be updated
                {
                    break; //break from loop if it does not
                }

                //Printing out available flavours
                string currentFlavours = "";
                foreach (Flavour f in icecream.Flavours)
                {
                    currentFlavours += $"{f}\n";
                }

                Console.Write("Current Flavours:\n" + //Print out current flavours
                    "----------------\n" +
                    $"{currentFlavours}\n"); //printing out all current flavours

                Console.Write("--------------------------\n" +
                            "Available Flavours\n" +
                            "----------------\n");
                foreach (string f in flavourData.Keys)
                {
                    Console.WriteLine($"{f}");
                } //printing out all available flavours
                Console.WriteLine("Please enter all selected flavours including modified and existing ones.\n");

                string flavourOption = " ";
                while (true) //Making sure that all scoops have a selected flavour
                {
                    while (true) //Data validation for flavour option
                    {
                        try
                        {
                            Console.Write("Enter flavour to add (0 to not change ANY flavours): "); //If option is the same, user can enter 0 to continue to the next modification

                            flavourOption = Console.ReadLine().ToLower();  

                            if (flavourOption == "0") //Breaking out of the loop if flavourOption == 0
                            {
                                break;
                            }
                            else if (flavourOption.Split(" ").Length > 1) // checking if its Sea Salt
                            {
                                string[] words = flavourOption.Split(' ');
                                for (int x = 0; x < words.Length; x++)
                                {
                                    words[x] = char.ToUpper(words[x][0]) + words[x].Substring(1);
                                }
                                flavourOption = string.Join(" ", words); //Updating the flavour option
                            }
                            else
                            {
                                flavourOption = char.ToUpper(flavourOption[0]) + flavourOption.Substring(1).ToLower(); //Capitalizing user input
                            }

                            if (!flavourData.ContainsKey(flavourOption)) //check if flavourdata contians flavour option
                            {
                                throw new FormatException(); //throw new error if it does not
                            }
                            else
                            {
                                break; //break if it does
                            }
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("No such flavour exists!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please enter an available flavour.");
                        }
                    }

                    if (flavourOption == "0") //Breaking out of the loop if flavourOption == 0
                    {
                        scoops = icecream.Scoops; //Changing the scoops back to the original if no flavours have been changed
                        noFlavourUpdate = true; //break from the main for loop
                        break; //break form the while loop that makes sure all scoops have a flavour
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
                                throw new ArgumentOutOfRangeException(); //throw new error if quantity is not in range
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
                        bool optionExists = false;
                        foreach (Flavour f in flavours)
                        {
                            if (f.Type == flavourOption) 
                            {
                                f.Quantity += quantity; //Adding the quantity and not an entirely new flavour if the flavour already exists
                                optionExists = true; //change option exists to true
                                break;
                            }
                        }

                        if (!optionExists) //if the option doesn't exist yet
                        {
                            if (flavourData[flavourOption] > 0) //Check if ice cream is premium
                            {
                                Flavour newFlavour = new Flavour(flavourOption, true, quantity);
                                flavours.Add(newFlavour); //adding flavour to flavours list
                                Console.WriteLine("Flavour has been added.");
                            }
                            else
                            {
                                Flavour newFlavour = new Flavour(flavourOption, false, quantity);
                                flavours.Add(newFlavour); //adding flavour to flavours list
                                Console.WriteLine("Flavour has been added.");
                            }
                        }

                        flavourCount += quantity; //Add the quantity to the number of flavours that have been added 

                        //Quit for loop if the quantity of flavours have been hit
                        if (flavourCount >= scoops)
                        {
                            Console.WriteLine("Maximum number of flavours have been reached.");
                            break;
                        }
                    }

                }

                int countTopping = 0; //To check if maximum number of toppings has been reached
                string currentToppings = "";
                bool noToppingUpdate = false; //To check if toppings have been updated

                foreach (Topping t in icecream.Toppings)
                {
                    currentToppings += $"{t}";
                }

                Console.Write("Current Toppings:\n" + //Print out current Toppings
                    "----------------\n" +
                    $"{currentToppings}\n");

                while (countTopping < 4) //getting toppings for new ice cream
                {
                    Console.Write("Available Toppings\n" +
                            "----------------\n");
                    foreach (string t in toppingData.Keys)
                    {
                        Console.WriteLine($"{t}");
                    }
                    Console.WriteLine("Enter -1 to remove all toppings\n");

                    string toppingOption = " ";
                    while (true) //Data validation for topping input
                    {
                        try
                        {
                            Console.Write("Enter new topping (0 to NOT make any changes to ALL toppings): ");

                            toppingOption = Console.ReadLine();

                            if (toppingOption == "0")
                            {
                                noToppingUpdate = true; //change no topping update to true
                                break;
                            }
                            else if (toppingOption == "-1") //check if user wants to remove all existing toppings
                            {
                                break;
                            }
                            else if (toppingData.ContainsKey(char.ToUpper(toppingOption[0]) + toppingOption.Substring(1).ToLower()) || toppingOption == "0") //Checking if topping exists
                            {
                                break;
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            Console.WriteLine("There is no such topping!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Invalid input! Please only enter the available options!");
                        }                  
                    }

                    if (toppingOption == "0")
                    {
                        break; //breaking from loop
                    }
                    else if (toppingOption == "-1")
                    {
                        toppings.Clear(); //Clearing all toppings
                        break; //breaking from loop
                    }

                    Topping newTopping = new Topping(char.ToUpper(toppingOption[0]) + toppingOption.Substring(1).ToLower()); //Making new topping
                    toppings.Add(newTopping); //Adding new toppings 
                    countTopping++; //Incrementing countTopping
                    Console.WriteLine("Topping has been added. \n");

                    //Checking if user wants to add more toppings
                    string addTopping = " ";
                    while (true)
                    {
                        try
                        {
                            Console.Write("Do you still want to add add toppings? (Y/N): ");
                            addTopping = Console.ReadLine().ToUpper();

                            if (addTopping != "N" && addTopping != "Y")
                            {
                                throw new FormatException();
                            }
                            else if (addTopping == "N")
                            {
                                countTopping = 1000; //So that it will break from the main while loop
                                break;
                            }
                            else
                            {
                                break; 
                            }
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine("Incorrect format! Please only enter Y or N!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please only enter Y or N");
                        }
                    }
                }

                //Checking if flavours and toppings has been previously cancelled
                if (noFlavourUpdate)
                {
                    flavours = icecream.Flavours; //Making sure the flavours do not change
                }

                if (noToppingUpdate)
                {
                    toppings = icecream.Toppings; //Making sure the toppings do not change
                }

                //Changing all the data for toppings and flavours
                if (icOption == "Cone")
                {
                    if (icecream is not Cone)
                    {
                        modifiedIceCream = new Cone(modifiedIceCream.Option, modifiedIceCream.Scoops, flavours, toppings, false); //Making the modified icecream a cone if it is not but the option is a cone
                    }

                    Cone cone = (Cone)modifiedIceCream; //Need to downcast to access cone properties
                    bool dipped = cone.Dipped; 

                    Console.Write($"\nCurrent Dipping: {cone.Dipped}\n" + //Print out if cone is currently dipped
                        "[1] Chocolate-dipped cone\n" +
                        "[2] Regular cone\n" +
                        "[0] Continue\n");

                    while (true) //Data validation to check if cone option is valid
                    {
                        try
                        {
                            Console.Write("Enter option: ");
                            int coneOption = Convert.ToInt32(Console.ReadLine());
                            if (coneOption == 0) //Break if 0 is chosen
                            {
                                break; 
                            }
                            else if (coneOption == 1) //Check if cone is dipped
                            {
                                dipped = true;
                                break;
                            }
                            else if (coneOption == 2)
                            {
                                dipped = false;
                                break;
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine("Incorrect format! Please enter an option from 1-2.");
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            Console.WriteLine("Option out of range! Enter a number between 1-2!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please only enter a number between 1-2!");
                        }
                    }
                    icecream = new Cone("Cone", scoops, flavours, toppings, dipped); //Updating the ice cream 
                    modificationsMade = true; //Indicate that modifications have been made
                }
                else if (icOption == "Waffle")
                {
                    if (icecream is not Waffle)
                    {
                        modifiedIceCream = new Waffle(modifiedIceCream.Option, modifiedIceCream.Scoops, flavours, toppings, "Original"); //Making the modified icecream a waffle if it is not but the option is a cone
                    }

                    Waffle waffle = (Waffle)modifiedIceCream; //Need to downcast to access waffle properties
                    Console.Write($"\nCurrent Waffle Flavour: {waffle.WaffleFlavour}\n" + //Print out current waffle flavour
                        "[1] Original\n" +
                        "[2] Red Velvet\n" +
                        "[3] Charcoal\n" +
                        "[4] Pandan Waffle\n" +
                        "[0] Continue\n");

                    while (true) //Data validtion to check if waffle option is valid
                    {
                        try
                        {
                            Console.Write("Enter option: ");

                            int waffleOption = Convert.ToInt32(Console.ReadLine());

                            //check what waffle option chosen
                            if (waffleOption == 0) 
                            {
                                break;
                            }
                            else if (waffleOption == 1)
                            {
                                waffle.WaffleFlavour = "Original";
                                break;
                            }
                            else if (waffleOption == 2)
                            {
                                waffle.WaffleFlavour = "Red Velvet";
                                break;
                            }
                            else if (waffleOption == 3)
                            {
                                waffle.WaffleFlavour = "Charcoal";
                                break;
                            }
                            else if (waffleOption == 4)
                            {
                                waffle.WaffleFlavour = "Pandan";
                                break;
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException(); //throw new error if option is out of range
                            }
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine("Incorrect format! Please enter a number between 1-4.");
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            Console.WriteLine("Option out of range! Option must be between the range of 1-4!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please only enter a number between 1-4!");
                        }
                    }
                    icecream = new Waffle("Waffle", scoops, flavours, toppings, waffle.WaffleFlavour);//Updating the ice cream 
                    modificationsMade = true; //Indicate that modifications have been made
                }
                else if (icOption == "Cup")
                {
                    icecream = new Cup("Cup", scoops, flavours, toppings); //Updating the ice cream 
                    modificationsMade = true; //Indicate that modifications have been made
                }
                
                //Checking if any modifications have been made
                if (modificationsMade == true)
                {
                    IceCreamList[IceCreamList.Count - 1] = icecream; //Updating ice cream values
                    Console.WriteLine("Ice Cream Updated!");
                }
                else
                {
                    Console.WriteLine("No changes have been made.");
                }
                break;
            }
        }

        public void AddIceCream(IceCream ic) //Method to add new ice cream
        {
            IceCreamList.Add(ic);
        }
        public void DeleteIceCream(int i) //Method to delete ice cream
        {
            IceCreamList.RemoveAt(i - 1); //Removing at the index entered
        }
        public double CalculateTotal() //Method to calculate order total
        {
            double total = 0;
            foreach (IceCream icecream in IceCreamList)
            {
                total += icecream.CalculatePrice();
            }
            return total;
        }

        public double CalculateWaitingTime(Dictionary<string, int> timeTaken) // Method to calculate waiting time
        {
            double waitingTime = 0;
            //Calculating time taken for each order
            foreach (IceCream ic in IceCreamList)
            {
                if (ic.Option.ToLower() == "waffle") //if option is waffle, plus 3 minutes to waiting time
                {
                    waitingTime += timeTaken["waffle"];
                }
        
                // Add waiting time for scoops and toppings
                waitingTime += ic.Scoops * timeTaken["scoop"]; //add time for each scoop
                waitingTime += ic.Toppings.Count() * timeTaken["topping"]; //add time for each topping
                waitingTime += timeTaken["checkout"]; //adding checkout time per person
            }
            return waitingTime; // Add waiting time for checkout
        }

        public override string ToString()
        {
            string icecreams = "";
            foreach (IceCream ic in IceCreamList)
            {
                icecreams += ic;
            }
            return $"Order ID: {Id}\n" +
                $"Time Received: {TimeReceived}\n" +
                $"Time Fulfilled: {TimeFulfilled}\n" +
                $"Ice Cream Details:{icecreams}\n";
        }
    }
}

