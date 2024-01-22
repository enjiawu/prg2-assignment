//==========================================================
// Student Number : S10256978
// Student Name : Wu Enjia
// Partner Name : Xue Wenya
//==========================================================

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    }
                    else if (icOption == 2)
                    {
                        icecream.Option = "Cone";
                        icecream = new Cone("Waffle", icecream.Scoops, icecream.Flavours, icecream.Toppings, false);
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
                }
                else if (option == 3) //Change flavours option
                {
                    bool breakLoop = true;
                    while (breakLoop) //Iterating through the loop until user does not want to modify flavours anymore
                    {
                        string flavours = "";
                        int count = 1;
                        foreach (Flavour f in icecream.Flavours)
                        {
                            flavours += $"[{count}] {f}\n";
                        }

                        Console.Write("Current Flavours:\n" + //Print out current flavours
                            "----------------\n" +
                            $"{flavours}\n" +
                            $"Enter flavour to modify: ");
                        int modifyOption = Convert.ToInt32(Console.ReadLine());
                        Flavour flavour = icecream.Flavours[modifyOption - 1]; //Get flavour to modify

                        for (int x = 0; x < flavour.Quantity; x++) //Iterating through the quantity of the selected flavour (in case they want to select two different flavours)
                        {
                            Console.Write("Available Flavours\n" +
                            "----------------\n" +
                            "[1] Vanilla\n" +
                            "[2] Chocolate\n" +
                            "[3] Strawberry\n" +
                            "[4] Durian\n" +
                            "[5] Ube\n" +
                            "[6] Sea Salt\n" +
                            "[0] Return" +
                            "Enter new flavour(s): ");

                            int flavourOption = Convert.ToInt32(Console.ReadLine());
                            if (flavourOption == 0)
                            {
                                breakLoop = false; //Breaking out of main while loop
                                continue; 
                            }

                            int quantity;
                            //Check for quantity of flavour
                            while (true)
                            {
                                Console.Write("Enter quantity of flavour: ");
                                quantity = Convert.ToInt32(Console.ReadLine());
                                if (quantity > flavour.Quantity) //Making sure quantity of selected flavour does not exceed the number of scoops in the ice cream
                                {
                                    Console.WriteLine($"Quantity must be lesser than {flavour.Quantity}!");

                                }
                                else
                                {
                                    break;
                                }

                            }

                            //Changing the flavour
                            icecream.Flavours.Remove(flavour); //Removing the flavour that is to be removed from Flavours
                            Flavour newFlavour;
                            if (flavourOption == 1)
                            {
                                newFlavour = new Flavour("Vanilla", false, quantity);
                            }
                            else if (flavourOption == 2)
                            {
                                newFlavour = new Flavour("Chocolate", false, quantity);
                            }
                            else if (flavourOption == 3)
                            {
                                newFlavour = new Flavour("Strawberry", false, quantity);
                            }
                            else if (flavourOption == 4)
                            {
                                newFlavour = new Flavour("Durian", true, quantity);
                            }
                            else if (flavourOption == 5)
                            {
                                newFlavour = new Flavour("Ube", true, quantity);
                            }
                            else
                            {
                                newFlavour = new Flavour("Sea Salt", true, quantity);
                            }
                            icecream.Flavours.Add(newFlavour); //Adding new flavours 

                            //Quittng the for loop if the quantity of flavours have been hit
                            if (quantity == flavour.Quantity)
                            {
                                break;
                            }
                        }
                    }
                }

                else if (option == 4) //Change toppings option
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
                        "[0] Return" +
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
                        else {
                            newTopping = new Topping("Oreos");
                        }
                        icecream.Toppings.Add(newTopping); //Adding new toppings 
                    }
                }
                else if (option == 5) //Change cone dipping option
                {
                    if (icecream is Cone)
                    {
                        Cone cone = (Cone) icecream; //Need to downcast to access cone properties
                        Console.Write($"Current Dipping: {cone.Dipped}\n" + //Print out if cone is currently dipped
                            "[1] Chocolate-dippedcone\n" +
                            "[2] Regular cone\n" +
                            "[0] Return\n" +
                            "Enter option: ");

                        int coneOption = Convert.ToInt32(Console.ReadLine());

                        if (coneOption == 0)
                        {
                            break;
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
                else //Change waffle flavour option
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
                            break;
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
                            waffle.WaffleFlavour = "Pandan Waffle";
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ice cream must be a waffle!");
                    }
                }
            }
        }
        public void AddIceCream(IceCream ic)
        {
            IceCreamList.Add(ic);
        }
        public void DeleteIceCream(int i)
        {
            if (IceCreamList.Count > 1)
            {
                IceCreamList.RemoveAt(i - 1); //Removing at the index entered
            }
            else
            {
                Console.WriteLine("There cannot be zero ice creams in an order.");
            }
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
