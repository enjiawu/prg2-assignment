﻿using S10256978_PRG2Assignment.Classes;
using System.ComponentModel.Design;

Dictionary<int, Customer> customerList = new Dictionary<int, Customer>();

//Option 2 - List all current orders
Dictionary<int, Order> regularQueue = new Dictionary<int, Order>(); //Dictionary to store current orders in regular queue
Dictionary<int, Order> goldMembersQueue = new Dictionary<int, Order>(); //Dictionary to store current orders in gold members queue
void ListCurrentOrders()
{
    Console.WriteLine("Current orders in the regular queue");
    Console.WriteLine("-------------------------------");
    foreach (Order order in regularQueue.Values)
    {
        Console.WriteLine(order.ToString());
    }

    Console.WriteLine("\nCurrent orders in the gold members queue");
    Console.WriteLine("-----------------------------------------");
    foreach(Order order in goldMembersQueue.Values)
    {
        Console.WriteLine(order.ToString());
    }
}

//Option 5 - Display order details of a customers
void InitOrders(Dictionary<int, Customer> customerList)
//Method to initialize orders from orders.csv and add them to respective members
{
    using (StreamReader sr = new StreamReader("orders.csv"))
    //Reading order.csv to get all the current orders
    {
        Dictionary<int, Order> orderDict = new Dictionary<int, Order>(); //Dictionary to keep track of which ice creams are in the same order
        Dictionary<int, List<Order>> memberOrderDict = new Dictionary<int, List<Order>>(); //Dictionary to keep track of which orders are under which member

        string header = sr.ReadLine(); //Reading header 
        string? s;

        while ((s = sr.ReadLine()) != null)
        {
            //Splitting the line and getting all the respective values
            string[] line = s.Split(',');
            int id = Convert.ToInt32(line[0]);
            int memberId = Convert.ToInt32(line[1]);
            DateTime timeReceived = Convert.ToDateTime(line[2]);
            DateTime timeFulfilled = Convert.ToDateTime(line[3]);
            string option = line[4];
            int scoops = Convert.ToInt32(line[5]);

            List<Flavour> flavours = new List<Flavour>(); //List to hold all the flavours in the order
            List<Topping> toppings = new List<Topping>(); //List to hold all the toppings in the flavour

            //Assigning to necessary classes 
            Order order;

            if (orderDict.Keys.Contains(id)) //If order already exists, order will change values
            {
                order = orderDict[id]; 
            }
            else //If order does not exist yet
            {
                order = new Order(id, timeReceived);
                orderDict[id] = order; //Add order into dictionary
            }

            if (!memberOrderDict.Keys.Contains(memberId)) //Check if member does not have any previous orders
            {
                memberOrderDict[memberId] = new List<Order>(); //Add member and order key pair into memberOrderDict
            }

            if (memberOrderDict[memberId].Contains(order)) //Check if memberOrderDict already contains the orderid
            {
                memberOrderDict[memberId][memberOrderDict[memberId].IndexOf(order)] = order; //If it does, replace it with the new one
            }
            else
            {
                memberOrderDict[memberId].Add(order); //If not, add orders to list of orders member has
            }

            order.TimeFulfilled = timeFulfilled;

            Dictionary<string, int> flavourCount = new Dictionary<string, int>(); //Making dictionary to hold the quantity of each flavour

            flavourCount["Vanilla"] = 0;
            flavourCount["Chocolate"] = 0;
            flavourCount["Strawberry"] = 0;
            flavourCount["Durian"] = 0;
            flavourCount["Ube"] = 0;
            flavourCount["Sea Salt"] = 0;

            for (int i = 8; i <= 10; i++) //Check what flavours are included
            {
                if (line[i] == "") //Break the loop if empty because empty means no more flavours
                {
                    break;
                }
                flavourCount[line[i]]++; //Add quantity to flavour
            }

            bool premium = false; //Boolean to see if flavour is premium
            foreach (string flavour in flavourCount.Keys) //Iterating through flavourCount to see which flavours are in the order
            {
                if (flavourCount[flavour] != 0)
                {
                    if (flavour == "Durian" || flavour == "Ube" || flavour == "Sea salt") //Check if the flavour is premium or regular
                    {
                        premium = true; //If premium, change premium value to true
                    }
                    Flavour flav = new Flavour(flavour, premium, flavourCount[flavour]); //Make new flavour 
                    flavours.Add(flav); //Add flavour to list of flavours for current ice cream
                }
            }

            for (int i = 11; i <= 14; i++) //Check what toppings are included
            {
                if (line[i] == "") //Break the loop if empty because empty means no more toppings
                {
                    break;
                }
                else
                {
                    Topping topping = new Topping(line[i]); //Make topping
                    toppings.Add(topping); //Add topping to list of toppings for current ice cream
                }
            }

            if (option == "Cup") //Checking which option the ice cream is
            {
                IceCream icecream = new Cup(option, scoops, flavours, toppings);
                order.AddIceCream(icecream);
            }
            else if (option == "Cone")
            {
                bool dipped = Convert.ToBoolean(line[6]);
                IceCream icecream = new Cone(option, scoops, flavours, toppings, dipped);
                order.AddIceCream(icecream);
            }
            else
            {
                string waffleFlavour = line[7];
                IceCream icecream = new Waffle(option, scoops, flavours, toppings, waffleFlavour);
                order.AddIceCream(icecream);
            }
        }

        foreach (int mId in memberOrderDict.Keys) //Adding all the orders into their respective customer OrderHistory
        {
            foreach(Order order in memberOrderDict[mId])
            {
                customerList[mId].OrderHistory.Add(order);
            }
        }
    }
}

//Listing Customer Order Details
void ListOrderDetails(Dictionary<int, Customer> customerList)
{
    //Listing the customers
    ListAllCustomers(customerList);

    try //Data Validation
    {
        //Prompting user to select a customer
        Console.Write("Enter customer ID: ");
        int customerId = Convert.ToInt32(Console.ReadLine());

        //Retrieving the selected customer
        Customer selectedCustomer = customerList[customerId];

        //Retrieving all the order objects of the customer, past and current
        InitOrders(customerList);

        //Displaying all the details of the order
        int count = 1;
        Console.WriteLine("Past Orders");
        Console.WriteLine("---------------------------------------");

        foreach (Order order in selectedCustomer.OrderHistory)
        {
            Console.WriteLine(order);
        }

        Console.WriteLine("\nCurrent Order");
        Console.WriteLine("---------------------------------------");
        Console.WriteLine(selectedCustomer.CurrentOrder);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Customer ID not found! Please try again.");
    }
}

//Option 6 - Modifying order details
void ModifyOrder(Dictionary<int, Customer> customerList)
{
    //Listing all customers
    ListAllCustomers(customerList);

    //Prompting user to select a customer
    Console.Write("Enter customer ID: ");
    int customerId = Convert.ToInt32(Console.ReadLine());

    //Retrieving selected customer's current order
    Customer selectedCustomer = customerList[customerId];
    Order currentOrder = selectedCustomer.CurrentOrder;

    //Listing all the ice cream objects contained in the order
    int count = 1;
    foreach(IceCream ic in currentOrder.IceCreamList)
    {
        Console.WriteLine($"[Ice Cream {count}]\n{ic}");
        count++;
    }

    //Prompting user to select 1-3
    Console.Write("\n[1] Choose an existing ice cream to modify\n" +
        "[2] Add an entirely new ice cream object to the order\n" +
        "[3] Choose an existing ice cream object to delete from the order\n" +
        "Enter your option: "); //Printing out options
    int option = Convert.ToInt32(Console.ReadLine()); //Reading user input

    if (option == 1) //Option 1 - Modifying an existing ice cream
    {
        Console.WriteLine("Choose an existing ice cream to modify");
        Console.WriteLine("---------------------------------------");

        //Prompting user to select an ice cream
        Console.Write("Enter a number to select which ice cream to modify: ");
        int icNo = Convert.ToInt32(Console.ReadLine());

        //Calling method to modify ice cream
        currentOrder.ModifyIceCream(icNo);
    }
    else if (option == 2)
    {
        Console.Write("Add an entirely new ice cream object to the order");
        Console.WriteLine("----------------------------------------------");

        //Prompting user for all required info to create a new ice cream
        while (true)
        {
            Console.Write("Enter ice cream option (Cone, Waffle or Cup): ");
            string icOption = Console.ReadLine();
            Console.Write("Enter number of scoops: ");
            int scoops = Convert.ToInt32(Console.ReadLine());

            List<Flavour> flavours = new List<Flavour>(); //list of hold all the flavours to be added
            List<Topping> toppings = new List<Topping>(); //list of hold all the toppings to be added
            for (int i = 0; i < scoops; i++) //getting flavours for new ice cream
            {
                Console.Write("\nAvailable Flavours\n" +
                            "----------------\n" +
                            "[1] Vanilla\n" +
                            "[2] Chocolate\n" +
                            "[3] Strawberry\n" +
                            "[4] Durian\n" +
                            "[5] Ube\n" +
                            "[6] Sea Salt\n" +
                            "Enter flavour to add: ");
                int flavourOption = Convert.ToInt32(Console.ReadLine());

                int quantity;
                //Check for quantity of flavour
                while (true)
                {
                    Console.Write("Enter quantity of flavour");
                    quantity = Convert.ToInt32(Console.ReadLine());
                    if (quantity > scoops) //Making sure quantity of selected flavour does not exceed the number of scoops in the ice cream
                    {
                        Console.WriteLine($"Quantity must be lesser than {scoops}!");

                    }
                    else
                    {
                        break;
                    }
                }

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
                flavours.Add(newFlavour); //adding flavour to flavours list

                //Quittng the for loop if the quantity of flavours have been hit
                if (quantity == scoops)
                {
                    break;
                }
            }

            for (int i = 0; i <= 4; i++) //getting toppings for new ice cream
            {
                Console.Write("Available Toppings\n" +
                       "----------------\n" +
                       "[1] Sprinkles\n" +
                       "[2] Mochi\n" +
                       "[3] Sago\n" +
                       "[4] Oreos\n" +
                       "[0] Done" +
                       "Enter new topping(s): ");

                int toppingOption = Convert.ToInt32(Console.ReadLine());
                if (toppingOption == 0)
                {
                    break; //break from while loop
                }

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
                toppings.Add(newTopping); //Adding new toppings 
            }

            IceCream iceCream;
            if (icOption == "Cone")
            {
                bool dipped;
                Console.WriteLine("Cone dippings available\n" +
                    "-------------------" +
                    "[1] Chocolate-dippedcone\n" +
                    "[2] Regular cone\n" +
                    "Enter option: ");
                int coneOption = Convert.ToInt32(Console.ReadLine());

                if (coneOption == 1) //Check if cone is dipped
                {
                    dipped = true;
                }
                else
                {
                    dipped = false;
                }

                iceCream = new Cone("Cone", scoops, flavours, toppings, dipped);
            }
            else if (icOption == "Waffle")
            {
                string waffleFlavour = "";
                Console.WriteLine("Waffle flavours available\n" +
                    "-------------------" +
                    "[1] Original\n" +
                    "[2] Red Velvet\n" +
                    "[3] Charcoal\n" +
                    "[4] Pandan Waffle\n" +
                    "Enter option: ");
                int waffleOption = Convert.ToInt32(Console.ReadLine());

                if (waffleOption == 1)
                {
                    waffleFlavour = "Original";
                }
                else if (waffleOption == 2)
                {
                    waffleFlavour = "Red Velvet";
                }
                else if (waffleOption == 3)
                {
                    waffleFlavour = "Charcoal";
                }
                else
                {
                    waffleFlavour = "Pandan Waffle";
                }
                iceCream = new Waffle("Waffle", scoops, flavours, toppings, waffleFlavour);
            }
            else
            {
                iceCream = new Cup("Cup", scoops, flavours, toppings);
            }
            currentOrder.AddIceCream(iceCream); //Adding the new ice cream to the current order
            Console.WriteLine("Ice Cream has been added.");
        }
    }
    else
    {
        Console.Write("Select which ice cream to delete: ");
        int delOption = Convert.ToInt32(Console.ReadLine());   

        if (currentOrder.IceCreamList.Count > 0)
        {
            currentOrder.DeleteIceCream(delOption);
            Console.WriteLine("Ice Cream has been deleted.");
        }
        else
        {
            Console.WriteLine("Error! There cannot be zero ice creams in an order!");
        }

    }

    Console.WriteLine("Updated Order\n" +
        "-------------------");
    Console.WriteLine(currentOrder);
}

ModifyOrder(customerList);
