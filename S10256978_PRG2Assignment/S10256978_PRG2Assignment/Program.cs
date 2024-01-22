using S10256978_PRG2Assignment.Classes;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
//Options
//Enjia: 2,5,6, a (advanced)
//Wenya: 1,3,4, b (advanced)
void Menu() //Function to display menu
{
    Console.Write("Ice Cream Shop Management System\n" +
        "----------------------------------\n" +
        "[1] List all customers\n" +
        "[2] List all current orders\n" +
        "[3] Register a new customer\n" +
        "[4] Create a customer's order\n" +
        "[5] Display order details of a customer\n" +
        "[6] Modify order details\n" +
        "[0] Quit\n" +
        "Enter option: ");
}

Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>(); //List to keep keep of all the customer details

void InitCustomers(Dictionary<int, Customer> customerDict)
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string header = sr.ReadLine(); // Read header
        string? s;

        while ((s = sr.ReadLine()) != null)
        {
            string[] line = s.Split(',');
            string Name = line[0];
            int MemberId = Convert.ToInt32(line[1]);
            DateTime Dob = Convert.ToDateTime(line[2]);
            string MemberShipStatus = line[3];
            int MemberShipPoint = Convert.ToInt32(line[4]);
            int PunchCard = Convert.ToInt32(line[5]);

            //Assigning to necessary classes

            Customer customer = new Customer(Name, MemberId, Dob);
            PointCard pointCard = new PointCard(MemberShipPoint,PunchCard);

            pointCard.Tier = MemberShipStatus;
            customer.Rewards = pointCard;
            customerDict[customer.MemberId] = customer;

        }

    }
}

// Option 1 - display all customer
void DisplayAllCustomers(Dictionary<int, Customer> customerDict)
{
    // Display the header
    Console.WriteLine($"{"Name",-15}{"MemberId",-15}{"Date of Birth",-15}{"Membership Status",-22}{"Membership Points",-22}{"PunchCard",-22}");

    foreach (Customer customer in customerDict.Values)
    {
        Console.WriteLine($"{customer.Name, -15}{customer.MemberId, -15}{customer.Dob.ToString("dd/MM/yyyy"), -15}" +
            $"{customer.Rewards.Tier,-22}{customer.Rewards.Points,-22}{customer.Rewards.PunchCard,-22}");
    }
}

//Option 2 - List all current orders
Queue<Order> regularQueue = new Queue<Order>() ; //Queue to store current orders in regular queue
Queue<Order> goldMembersQueue = new Queue<Order>(); //Queue to store current orders in gold members queue
void ListCurrentOrders(Queue<Order> regularQueue, Queue<Order> goldMembersQueue)
{
    Console.WriteLine("Current orders in the regular queue");
    Console.WriteLine("------------------------------------");
    foreach (Order order in regularQueue)
    {
        Console.WriteLine(order);
    }

    Console.WriteLine("\nCurrent orders in the gold members queue");
    Console.WriteLine("------------------------------------------");
    foreach(Order order in goldMembersQueue)
    {
        Console.WriteLine(order);
    }
}

// Option 3 - Register new customer
void RegisterNewCustomer(Dictionary<int, Customer> customerDict)
{
    Console.Write("Enter customer name: ");
    string name = Convert.ToString(Console.ReadLine());

    int memberId;
    while (true)
    {
        Console.Write("Enter customer ID number: ");
        if (int.TryParse(Console.ReadLine(), out memberId)) 
        {
            if (customerDict.ContainsKey(memberId))
            {
                Console.WriteLine($"Customer ID {memberId} already exists. Please choose a different ID.");
            }
            else
            {
                break; // Exit the loop if the ID is valid and not already in use
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid integer for the customer ID.");
        }
    }

    DateTime dob;
    bool isValidDate = false;

    do
    {
        Console.Write("Enter customer date of birth (dd/MM/yyyy): ");
        string input = Console.ReadLine();

        isValidDate = DateTime.TryParse(input, out dob);

        if (!isValidDate)
        {
            Console.WriteLine("Invalid date format. Please enter a valid date.");
        }

    } while (!isValidDate);

    // Create a new customer object
    Customer newCustomer = new Customer(name, memberId, dob);

    // Create a new PointCard object
    PointCard newPointCard = new PointCard();
    newCustomer.Rewards = newPointCard;

    // Set the initial Tier to "Ordinary"
    newPointCard.Tier = "Ordinary";

    // Add the new customer to the dictionary
    customerDict.Add(memberId, newCustomer);

    // Append the customer information to the CSV file
    AppendCustomerToCsv(newCustomer);

    Console.WriteLine("Registration successful!\n");
    DisplayAllCustomers(customerDict);
}

void AppendCustomerToCsv(Customer customer)
{
    using (StreamWriter writer = new StreamWriter("customers.csv", true))
    {
        writer.WriteLine($"{customer.Name},{customer.MemberId},{customer.Dob.ToString("dd/MM/yyyy")},{customer.Rewards.Tier},{customer.Rewards.Points},{customer.Rewards.PunchCard}");
    }
}

// Option 4 - Create customer order
void CreateCustomerOrder(Dictionary<int, Customer> customerDict)
{
    // Display available customers
    Console.WriteLine("\nSelect a customer by entering their Member ID:");
    foreach (var kvp in customerDict)
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value.Name}");
    }

    // Get member id
    Console.Write("\nEnter Member ID: ");
    int memberId;

    while (!int.TryParse(Console.ReadLine(), out memberId) || !customerDict.ContainsKey(memberId))
    {
        // Validate the entered ID
        Console.WriteLine("Invalid Member ID. Please enter a valid Member ID.");
        Console.Write("Enter Member ID: ");
    }

    if (customerDict.TryGetValue(memberId, out Customer selectedCustomer))
    {
        // Create a new order for the selected customer
        Order newOrder = new Order();
        selectedCustomer.CurrentOrder = newOrder;

        // Get a order ID for customer
        Console.Write("Enter order ID: ");
        int orderId = Convert.ToInt32(Console.ReadLine());
        newOrder.Id = orderId;

        // Set the current time as the received time for the order
        DateTime timeReceived = DateTime.Now;
        newOrder.TimeReceived = timeReceived;

        // Get ice cream option from the user
        Console.Write("\nEnter ice cream option (Cup/Cone/Waffle): ");
        string option;
        while (true)
        {
            option = Convert.ToString(Console.ReadLine());
            if (option != null && (option.ToLower() == "cup" || option.ToLower() == "cone" || option.ToLower() == "waffle"))
            {
                // Validate the entered ice cream option
                Console.WriteLine($"You selected {option}\n");
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please enter either 'Cup', 'Cone', or 'Waffle'.");
                Console.Write("Enter ice cream option (Cup/Cone/Waffle): ");
            }
        }

        // Get the number of scoops from user
        int scoops;
        while (true)
        {
            Console.Write("Enter number of scoops (1, 2, or 3): ");
            if (int.TryParse(Console.ReadLine(), out scoops) && scoops >= 1 && scoops <= 3)
            {
                Console.WriteLine($"You choose to have {scoops} scoops\n");
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number of scoops (1, 2, or 3).");
            }
        }

        // Check if the option is a cone and prompt user for upgrade to dipped
        bool dipped = false;
        string waffleFlavour = "";
        if (option.ToLower() == "cone")
        {
            while (true)
            {
                Console.Write("Upgrade to chocolate-dipped cone? (Y/N): ");
                string upgradeInput = Console.ReadLine()?.ToUpper();

                if (upgradeInput == "Y")
                {
                    dipped = true;
                    Console.WriteLine("You have upgraded to a chocolate-dipped cone.");
                    break;
                }
                else if (upgradeInput == "N")
                {
                    dipped = false;
                    Console.WriteLine("You have chosen a regular cone.");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'Y' for Yes or 'N' for No.");
                }
            }
        }
        else if (option.ToLower() == "waffle")
        {
            // Get the waffle flavor from user
            Console.Write("Choose waffle flavor (Original/Red Velvet/Charcoal/Pandan): ");
            while (true)
            {
                waffleFlavour = Convert.ToString(Console.ReadLine());
                if (waffleFlavour != null && (waffleFlavour.ToLower() == "original" || waffleFlavour.ToLower() == "red velvet" || waffleFlavour.ToLower() == "charcoal" || waffleFlavour.ToLower() == "pandan"))
                {
                    Console.WriteLine($"You selected {waffleFlavour} as your waffle flavour\n");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid waffle flavor. Please choose from 'Red Velvet', 'Charcoal', or 'Pandan'.");
                    Console.Write("Choose waffle flavor (Original/Red Velvet/Charcoal/Pandan): ");
                }
            }
        }

        // Display the available ice cream flavour
        Console.WriteLine("\nAvailable ice cream flavours:\nVanilla\nChocolate\nStrawberry\nDurian\nUbe\nSea Salt");

        // Get the ice cream flavours from user
        Console.Write("Enter ice cream flavors (comma-separated): ");
        string[] flavors = Console.ReadLine().Split(',');

        List<Flavour> flavourList = new List<Flavour>();

        // Validate and add the entered ice cream flavors to the list
        if (flavors != null)
        {
            foreach (string flavor in flavors)
            {
                string trimmedFlavor = flavor.Trim().ToLower(); 
                // Add validation for available flavors here
                if (trimmedFlavor == "vanilla" || trimmedFlavor == "chocolate" || trimmedFlavor == "strawberry" || trimmedFlavor == "durian" || trimmedFlavor == "ube" || trimmedFlavor == "sea salt")
                {
                    flavourList.Add(new Flavour(trimmedFlavor, false, 1));
                }
                else
                {
                    Console.WriteLine($"Invalid ice cream flavor: {flavor}. Please choose from the available options.");
                }
            }
        }

        // Display available ice cream toppings based on the selected option
        Console.WriteLine("\nAvailable ice cream toppings:\nSprinkles\nMochi\nSago\nOreos");

        // Get the ice cream toppings from the user (comma-separated)
        Console.Write("Enter ice cream toppings (comma-separated): ");
        string[] toppings = Console.ReadLine()?.Split(',');

        List<Topping> toppingList = new List<Topping>();

        // Validate and add the entered ice cream toppings to the list
        if (toppings != null)
        {
            foreach (string topping in toppings)
            {
                string trimmedTopping = topping.Trim().ToLower();
                // Add validation for available toppings here
                if (trimmedTopping == "sprinkles" || trimmedTopping == "mochi" || trimmedTopping == "sago" || trimmedTopping == "oreos")
                {
                    toppingList.Add(new Topping(trimmedTopping));
                }
                else
                {
                    Console.WriteLine($"Invalid ice cream topping: {topping}. Please choose from the available options.");
                }
            }
        }

        // Create the appropriate ice cream object based on the selected option
        IceCream iceCream = null;
        if (option.ToLower() == "cup")
        {
            iceCream = new Cup(option, scoops, flavourList, toppingList);

        }
        else if (option.ToLower() == "cone")
        {
            iceCream = new Cone(option, scoops, flavourList, toppingList, dipped);

        }
        else if (option.ToLower() == "waffle")
        {
            iceCream = new Waffle(option, scoops, flavourList, toppingList, waffleFlavour);
        }
        else
        {
            Console.WriteLine("Invalid option");
        }

        if (iceCream != null)
        {
            newOrder.AddIceCream(iceCream);
        }


        // Link the new order to the customer's current order
        selectedCustomer.CurrentOrder = newOrder;

        // Display the final order
        Console.WriteLine("\nYour order details:");
        Console.WriteLine("--------------------");
        Console.WriteLine($"Customer: {selectedCustomer.Name}");
        Console.WriteLine($"Order ID: {newOrder.Id}");
        Console.WriteLine($"Time Received: {newOrder.TimeReceived}");
        Console.WriteLine($"Ice Cream Option: {option}");
        Console.WriteLine($"Number of Scoops: {scoops}");

        if (option.ToLower() == "cone") //Need to add to queue after everything is done
        {
            Console.WriteLine($"Chocolate-Dipped Cone Upgrade: {dipped}");
        }

        Console.WriteLine($"Waffle Flavor: {waffleFlavour}");
        Console.WriteLine("Ice Cream Flavors: " + string.Join(", ", flavors));
        Console.WriteLine("Ice Cream Toppings: " + string.Join(", ", toppings));

    }
    else
    {
        Console.WriteLine("Customer not found.");
    }
}

//Option 5 - Display order details of a customers
void InitOrders(Dictionary<int, Customer> customerDict)
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
                customerDict[mId].OrderHistory.Add(order);
            }
        }
    }
}

//Listing Customer Order Details
void ListOrderDetails(Dictionary<int, Customer> customerDict)
{
    //Listing the customers
    DisplayAllCustomers(customerDict);

    try //Data Validation
    {
        //Prompting user to select a customer
        Console.Write("Enter customer ID: ");
        int customerId = Convert.ToInt32(Console.ReadLine());

        //Retrieving the selected customer
        Customer selectedCustomer = customerDict[customerId];

        //Displaying all the details of the order
        Console.WriteLine("\nPast Orders");
        Console.WriteLine("-------------------------------------------------------------");

        int count = 1;
        foreach (Order order in selectedCustomer.OrderHistory)
        {
            Console.WriteLine(order);
            count++;
        }

        Console.WriteLine("\nCurrent Order");
        Console.WriteLine("-------------------------------------------------------------");
        Console.WriteLine(selectedCustomer.CurrentOrder);
    }
    catch (FormatException ex)
    {
        Console.WriteLine($"Customer ID not found! Please try again.");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

//Option 6 - Modifying order details
void ModifyOrder(Dictionary<int, Customer> customerDict)
{
    //Listing all customers
    DisplayAllCustomers(customerDict);

    //Prompting user to select a customer
    Console.Write("Enter customer ID: ");
    int customerId = Convert.ToInt32(Console.ReadLine());

    //Retrieving selected customer's current order
    Customer selectedCustomer = customerDict[customerId];
    Order currentOrder = selectedCustomer.CurrentOrder;

    if (currentOrder != null)
    {
        //Listing all the ice cream objects contained in the order
        int count = 1;
        foreach (IceCream ic in currentOrder.IceCreamList)
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
    else
    {
        Console.WriteLine($"Customer ID [{customerId}] does not have any current orders. Please make an order first.");
    }
}
/*
    catch(Exception ex)
    {
        Console.WriteLine("No customer with that id exists! Please try again.");
    }
}*/

//Advanced feature a - Process an order and checkout
void ProcessCheckoutOrder()
{
    Order order; //Order to be checked out
    //Dequeue the first order in the queue
    if (goldMembersQueue.Count > 0) //Check if gold members queue has any orders
    {
        order = goldMembersQueue.Dequeue(); 
    }
    else
    {
        order = regularQueue.Dequeue();
    }

    //Display all the ice creams in the order
    int count = 1;
    double mostExIceCreamPrice = 0.00;
    foreach (IceCream ic in order.IceCreamList)
    {
        Console.WriteLine($"Ice Cream [{count}]\n" +
            $"------------------------\n" +
            $"{ic}");

        if (ic.CalculatePrice() > mostExIceCreamPrice) //Checking if ice cream is msot expensive in the order
        {
            mostExIceCreamPrice = ic.CalculatePrice(); 
        }
    }

    //Displaying the total bill amount
    double orderTotal = order.CalculateTotal();
    Console.WriteLine($"Total bill amount: ${orderTotal}");

    //Finding customer 
    Customer customer = new Customer();
    foreach(Customer c in customerDict.Values) //Iterating through customerDict
    {
        if (c.CurrentOrder.Id == order.Id) //Checking if customer's current order id is the same as the checkout order id
        {
            customer = c; //If it is, assign customer for the order
            break;
        }
    }

    //Displaying the membership status & points of the customer
    Console.WriteLine($"Membership status: {customer.Rewards.Tier}\n" +
        $"Points: {customer.Rewards.Points}");

    //Checking if its customer's birthday
    if (customer.Dob.ToString("dd/MM") == DateTime.Now.ToString("dd/MM"))
    {
        orderTotal -= mostExIceCreamPrice; //Making the most expensive ice cream in the order cost $0.00
    }

    //Checking if customer has completed their punch card
    IceCream firstIceCream = order.IceCreamList[0]; //Finding the first ice cream in the order
    if (customer.Rewards.PunchCard == 10)
    {
        orderTotal -= firstIceCream.CalculatePrice(); //Making the first ice cream in the order $0.00
        customer.Rewards.PunchCard = 0; //Resetting punch card back to 0
    }

    //Checking point card status to determine if the customer can redeem points
    if ((customer.Rewards.Tier == "Silver" || customer.Rewards.Tier == "Gold") && customer.Rewards.Points != 0)
    {
        //Prompt user asking how many points they want to use
        while (true)
        {
            try
            {
                Console.WriteLine($"Customer [{customer.MemberId}] has {customer.Rewards.Points} to redeem.");
                Console.Write("Number of points to redeem (1 point = $0.02) : ");
                int pointsToRedeem = Convert.ToInt32(Console.ReadLine());

                if (pointsToRedeem <= customer.Rewards.Points) //Check if customer has sufficient points
                {
                    customer.Rewards.RedeemPoints(pointsToRedeem);
                    orderTotal -= customer.Rewards.Points * 0.02; //Redeeming customers points (1 point = $0.02)
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
                break;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Please enter an integer!");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Customer [{customer.MemberId}] does not have sufficient points! Please enter a number between 0 and {customer.Rewards.Points}.");
            }
        }

    }
    else if (customer.Rewards.Points == 0)
    {
        Console.WriteLine($"Customer [{customer.MemberId}] has no points to redeem.");
    }
    else
    {
        Console.WriteLine($"Customer [{customer.MemberId}] is not eligible for points redemption!");
    }

    //Displaying final bill amount
    Console.WriteLine($"Final bill amount: {orderTotal}");

    //Prompting user any key to make payment
    Console.Write("Enter any key to make payment: ");
    
    //Incrementing punch card for every ice cream in the order
    foreach (IceCream ic in order.IceCreamList)
    {
        customer.Rewards.Punch();
    }

    if (customer.Rewards.PunchCard > 10) //Checking if customer punch card has more than 10 punches
    {
        customer.Rewards.PunchCard = 10; //If it does, set it back down to 10
    }

    //Earning points
    int pointsToAdd = Convert.ToInt32(Math.Floor(orderTotal * 0.72));
    customer.Rewards.AddPoints(pointsToAdd);

    //Updating member status 
    if (customer.Rewards.Points >= 100)
    {
        customer.Rewards.Tier = "Gold";
    }
    else if (customer.Rewards.Points >= 50)
    {
        customer.Rewards.Tier = "Silver";
    }

    //Marking the order as fulfilled with the current datetime
    order.TimeFulfilled = DateTime.Now;

    //Adding fulfilled order to customer's order history
    customer.OrderHistory.Add(order);

    //Updating orders.csv
    /*using (StreamWriter writer = new StreamWriter("order.csv", true))
    {
        foreach (IceCream ic in order.IceCreamList)
        {
            writer.WriteLine($"{order.Id},{customer.MemberId},{order.TimeReceived},{order.TimeFulfilled},{ic.Option},{ic.Scoops},{},{},{},{},{},{},{},{},{}");
        }
    }*/
}

//Main Program
InitCustomers(customerDict); //Reading data from customers.csv
InitOrders(customerDict); //Reading data from orders.csv

while (true) //While loop that keeps running until customer quits
{
    try //Data validation
    {
        Menu(); //Printing out the menu
        int option = Convert.ToInt32(Console.ReadLine()); //Read customer's input 

        if (option == 0) //Option 0 - Quit
        {
            Console.WriteLine("Bye!");
            break;
        }
        else if (option == 1) //Option 1 - List all customers
        {
            Console.WriteLine("\nOption 1 - List all customers");
            Console.WriteLine("------------------------------\n");
            DisplayAllCustomers(customerDict);
        }
        else if (option == 2) //Option 2 - List all current orders
        {
            Console.WriteLine("\nOption 2 - List all current orders");
            Console.WriteLine("----------------------------------\n");
            ListCurrentOrders(regularQueue, goldMembersQueue);
        }
        else if (option == 3) //Option 3 - Register a new customer
        {
            Console.WriteLine("\nOption 3 - Register a new customer");
            Console.WriteLine("----------------------------------\n");
            RegisterNewCustomer(customerDict);
        }
        else if (option == 4)
        {
            Console.WriteLine("\nOption 4 - Create a customer's order");
            Console.WriteLine("----------------------------------");
            CreateCustomerOrder(customerDict);
        }
        else if (option == 5)
        {
            Console.WriteLine("\nOption 5 - Display order details of a customer");
            Console.WriteLine("-------------------------------------------------\n");
            ListOrderDetails(customerDict);
        }
        else if (option == 6) 
        {
            Console.WriteLine("\nOption 6 - Modify order details");
            Console.WriteLine("----------------------------------\n");
            ModifyOrder(customerDict);
        }
        else
        {
            throw new ArgumentOutOfRangeException(); //throwing argument out of range exception
        }
        Console.WriteLine();
    }
    catch (FormatException ex)
    {
        Console.WriteLine("\nIncorrect format! Please enter a number from 1-6!\n");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("\nPlease only enter a number between 1-6!\n");
    }
    catch (Exception ex) 
    {
        Console.WriteLine($"\n{ex.Message}\n");
    }
}

