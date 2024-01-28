//Options
//==============================================================
//Wu Enjia: 2,5,6,a(7)
//Xue Wenya: 1,3,4,b(8)
//==============================================================

using Microsoft.VisualBasic.FileIO;
using S10256978_PRG2Assignment.Classes;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
internal class Program
{
    //Defining methods to be used for different classes and the main program
    public static Dictionary<string, double> flavourData = new Dictionary<string, double>(); //Dictionary to store information on flavour and respective cost
    public static Dictionary<string, double> toppingData = new Dictionary<string, double>(); //Dictionary to store information on toppings and respective cost
    private static void Main(string[] args)
    {
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
                "[7] Process an order and checkout\n" +
                "[8] Display Monthly charged amounts breakdown & total charged amount for the year\n" +
                "[0] Quit\n" +
                "Enter option: ");
        }

        void InitFlavours(Dictionary<string, double> flavourData)
        {
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
        }
        void InitToppings(Dictionary<string, double> toppingData)
        {
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
        }


        Dictionary<int, Customer> customerDict = new Dictionary<int, Customer>(); //List to keep of all the customer details
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
                    DateTime Dob = DateTime.ParseExact(line[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //DateTime Dob = Convert.ToDateTime(line[2]);
                    string MemberShipStatus = line[3];
                    int MemberShipPoint = Convert.ToInt32(line[4]);
                    int PunchCard = Convert.ToInt32(line[5]);

                    //Assigning to necessary classes

                    Customer customer = new Customer(Name, MemberId, Dob);
                    PointCard pointCard = new PointCard(MemberShipPoint, PunchCard);

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
                Console.WriteLine($"{customer.Name,-15}{customer.MemberId,-15}{customer.Dob.ToString("dd/MM/yyyy"),-15}" +
                    $"{customer.Rewards.Tier,-22}{customer.Rewards.Points,-22}{customer.Rewards.PunchCard,-22}");
            }
        }

        //Option 2 - List all current orders
        Queue<Order> regularQueue = new Queue<Order>(); //Queue to store current orders in regular queue
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
            foreach (Order order in goldMembersQueue)
            {
                Console.WriteLine(order);
            }
        }

        // Option 3 - Register new customer
        void RegisterNewCustomer(Dictionary<int, Customer> customerDict)
        {
            try
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
            catch (FormatException ex)
            {
                Console.WriteLine("Invalid input format. Please ensure you enter data in the correct format.");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred:");
                Console.WriteLine(ex.Message);
            }

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
            try
            {
                // Display available customers
                Console.WriteLine("\nSelect a customer by entering their Member ID:");
                foreach (var kvp in customerDict)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value.Name}");
                }

                // Create a new order for the selected customer
                Order newOrder = new Order();

                // Get member id
                Console.Write("\nEnter Member ID: ");
                int memberId;
                while (!int.TryParse(Console.ReadLine(), out memberId) || !customerDict.ContainsKey(memberId))
                {
                    // Validate the entered ID
                    Console.WriteLine("Invalid Member ID. Please enter a valid Member ID.");
                    Console.Write("Enter Member ID: ");
                }


                // Get an order ID for customer
                Console.Write("\nEnter order ID: ");
                int orderId = Convert.ToInt32(Console.ReadLine());
                newOrder.Id = orderId;

                // Set the current time as the received time for the order
                DateTime timeReceived = DateTime.Now;
                newOrder.TimeReceived = timeReceived;


                while (true)
                {
                    try
                    {
                        AddNewIceCream(newOrder);
                    }
                    catch (Exception ex)
                    {
                        // Handle the error appropriately, e.g., prompt for retry or exit
                        Console.WriteLine("Error adding ice cream:");
                        Console.WriteLine(ex.Message);
                    }

                    // prompt user if they want to add another icecream
                    string addAnotherIcecream;
                    while (true)
                    {
                        Console.Write("Would you like to add another ice cream to the order? (Y/N): ");
                        addAnotherIcecream = Convert.ToString(Console.ReadLine().ToUpper());
                        if (addAnotherIcecream != "Y" && addAnotherIcecream != "N")
                        {
                            Console.WriteLine("Invalid input. Please enter 'Y' to add another ice cream or 'N' to finish.");
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (addAnotherIcecream == "Y")
                    {
                        Console.WriteLine("You choose to add another ice cream.\n");
                        // Continue the loop to add another ice cream
                    }
                    else if (addAnotherIcecream == "N")
                    {
                        // Break out of the loop
                        break;
                    }


                }



                // Link the new order to the customer's current order
                if (customerDict.TryGetValue(memberId, out var selectedCustomer))
                {
                    selectedCustomer.CurrentOrder = newOrder;
                }


                // Determine the Pointcard tier and append the order to the appropriate queue
                if (selectedCustomer.Rewards != null && selectedCustomer.Rewards.Tier == "Gold")
                {
                    goldMembersQueue.Enqueue(newOrder);
                    Console.WriteLine("Order added to Gold Member Queue.");
                }
                else
                {
                    regularQueue.Enqueue(newOrder);
                    Console.WriteLine("Order added to Regular Queue.");
                }

                // Display order details directly in CreateCustomerOrder
                Console.WriteLine("Order Details:");
                Console.WriteLine($"Order ID: {newOrder.Id}");
                Console.WriteLine($"Time Received: {newOrder.TimeReceived}");

                // Print ice cream details
                foreach (var iceCream in newOrder.IceCreamList)
                {
                    Console.WriteLine($"Ice Cream: {iceCream}");
                }

                Console.WriteLine("Order made successfully!\n");

                Console.WriteLine(newOrder.ToString());
                foreach (Order order in regularQueue)
                {
                    Console.WriteLine($"{order}");
                }
                foreach (Order order in goldMembersQueue)
                {
                    Console.WriteLine($"{order}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging or troubleshooting
                Console.WriteLine("An unexpected error occurred:");
                Console.WriteLine(ex.Message);
            }
        }


        //Option 5 - Display order details of a customers
        Dictionary<int, Order> orderDict = new Dictionary<int, Order>(); //Dictionary to keep track of which ice creams are in the same order
        Dictionary<int, List<Order>> memberOrderDict = new Dictionary<int, List<Order>>(); //Dictionary to keep track of which orders are under which member

        void InitOrders(Dictionary<int, Customer> customerDict)
        //Method to initialize orders from orders.csv and add them to respective members
        {
            using (StreamReader sr = new StreamReader("orders.csv"))
            //Reading order.csv to get all the current orders
            {

                string header = sr.ReadLine(); //Reading header 
                string? s;

                while ((s = sr.ReadLine()) != null)
                {
                    //Splitting the line and getting all the respective values
                    string[] line = s.Split(',');
                    int id = Convert.ToInt32(line[0]);
                    int memberId = Convert.ToInt32(line[1]);
                    DateTime timeReceived = DateTime.ParseExact(line[2], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    DateTime timeFulfilled = DateTime.ParseExact(line[3], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //DateTime timeReceived = Convert.ToDateTime(line[2]);
                    //DateTime timeFulfilled = Convert.ToDateTime(line[3]);
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
                    foreach (Order order in memberOrderDict[mId])
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
        void ModifyIceCream(Order currentOrder) //Method to mofidy ice cream
        {
            Console.WriteLine("\nChoose an existing ice cream to modify");
            Console.WriteLine("---------------------------------------");
            while (true)
            {
                try //Data validation
                {
                    //Prompting user to select an ice cream
                    Console.Write("Enter a number to select which ice cream to modify (0 to cancel): ");
                    int icNo = Convert.ToInt32(Console.ReadLine());

                    if (icNo == 0)
                    {
                        break; //quit loop
                    }
                    else if (icNo < currentOrder.IceCreamList.Count() || icNo > currentOrder.IceCreamList.Count()) //Making sure the ice cream number is in range
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    //Calling method to modify ice cream
                    currentOrder.ModifyIceCream(icNo);
                    break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Incorrect format! Please enter a number.");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Option is out of range. Please enter a valid option!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please only enter the number of the ice cream in the order!");
                }
            }

        }

        void AddNewIceCream(Order currentOrder) //Method to add new ice cream
        {
            Console.WriteLine("Add an entirely new ice cream object to the order");
            Console.WriteLine("----------------------------------------------");

            //Prompting user for all required info to create a new ice cream
            string icOption;
            Console.Write("Available Options\n" +
                            "----------------\n" +
                            "Cone\n" +
                            "Waffle\n" +
                            "Cup\n");

            while (true) //Data validation for ice cream option
            {
                try
                {
                    Console.Write("Enter ice cream option: ");
                    icOption = Console.ReadLine().ToLower();
                    icOption = char.ToUpper(icOption[0]) + icOption.Substring(1).ToLower();
                    string[] options = { "Cone", "Cup", "Waffle" };

                    if (options.Contains(icOption))//Checking if the option exists
                    {
                        break;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Please enter one of the available options!");
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

            int scoops;
            while (true) //Data validation for number of scoops
            {
                try
                {
                    Console.Write("Enter number of scoops: ");
                    scoops = Convert.ToInt32(Console.ReadLine());

                    if (scoops > 3 || scoops <= 0) //Check if number of scoops is between 1 and 3
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    else
                    {
                        break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Format incorrect! Please enter a number from 1-3!");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Please enter an option from 1-3.");
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
            for (int i = 0; i < scoops; i++) //getting flavours for new ice cream
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
                            Console.Write("Enter flavour to add: ");

                            flavourOption = Console.ReadLine().ToLower(); //checking if its Sea Salt 
                            if (flavourOption.Split(" ").Length > 1)
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
                while (countTopping < 4) //getting toppings for new ice cream
                {
                    Console.Write("\nAvailable Toppings\n" +
                            "----------------\n");
                    foreach (string t in toppingData.Keys)
                    {
                        Console.WriteLine($"{t}");
                    }

                    string toppingOption;
                    while (true) //Data validation for topping input
                    {
                        try
                        {
                            Console.Write("Enter new topping (0 to continue): ");

                            toppingOption = Console.ReadLine();

                            if (toppingData.ContainsKey(char.ToUpper(toppingOption[0]) + toppingOption.Substring(1).ToLower()) || toppingOption == "0") //Checking if topping exists
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
                        break; //break from for loop
                    }

                    Topping newTopping = new Topping(char.ToUpper(toppingOption[0]) + toppingOption.Substring(1).ToLower()); //Making new topping
                    toppings.Add(newTopping); //Adding new toppings 
                    countTopping++; //Incrementing countTopping
                    Console.WriteLine("Topping has been added. \n");
                }

                IceCream iceCream;
                int coneOption;
                if (icOption == "Cone")
                {
                    bool dipped;
                    Console.WriteLine("\nCone dipping available\n" +
                        "--------------------------\n" +
                        "[1] Chocolate-dipped cone\n" +
                        "[2] Regular cone");

                    while (true) //Data validation to check if cone option is valid
                    {
                        try
                        {
                            Console.Write("Enter option: ");
                            coneOption = Convert.ToInt32(Console.ReadLine());
                            if (coneOption == 1) //Check if cone is dipped
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
                            Console.WriteLine("Enter a number between 1-2!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please only enter a number between 1-2!");
                        }
                    }
                    iceCream = new Cone("Cone", scoops, flavours, toppings, dipped);
                }
                else if (icOption == "Waffle")
                {
                    string waffleFlavour = "";
                    Console.WriteLine("\nWaffle flavours available\n" +
                        "--------------------------\n" +
                        "[1] Original\n" +
                        "[2] Red Velvet\n" +
                        "[3] Charcoal\n" +
                        "[4] Pandan\n");

                    while (true) //Data validtion to check if waffle option is valid
                    {
                        try
                        {
                            Console.Write("Enter option: ");
                            int waffleOption = Convert.ToInt32(Console.ReadLine());

                            if (waffleOption == 1)
                            {
                                waffleFlavour = "Original";
                                break;
                            }
                            else if (waffleOption == 2)
                            {
                                waffleFlavour = "Red Velvet";
                                break;
                            }
                            else if (waffleOption == 3)
                            {
                                waffleFlavour = "Charcoal";
                                break;
                            }
                            else if (waffleOption == 4)
                            {
                                waffleFlavour = "Pandan";
                                break;
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine("Incorrect format! Please enter a number between 1-4.");
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            Console.WriteLine("Option must be between the range of 1-4!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please only enter a number between 1-4!");
                        }
                    }
                    iceCream = new Waffle("Waffle", scoops, flavours, toppings, waffleFlavour);
                }
                else
                {
                    iceCream = new Cup("Cup", scoops, flavours, toppings);
                }
                currentOrder.AddIceCream(iceCream); //Adding the new ice cream to the current order
                Console.WriteLine("Ice Cream has been added.");
                Console.WriteLine("\n" + iceCream); //Printing out ice cream
                break; //Quitting the for loop
            }
        }

        void DeleteIceCream(Order currentOrder) //Function to delete ice cream
        {
            while (true) //Data validation
            {
                try
                {
                    Console.Write("Select which ice cream to delete (0 to cancel): ");
                    int delOption = Convert.ToInt32(Console.ReadLine());

                    if (delOption == 0) //Check if user wants to cancel
                    {
                        break;
                    }
                    else if (delOption > currentOrder.IceCreamList.Count() || delOption < 0) //Check if delOption is one of the ice cream options and if it is not a negative number
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    else if (currentOrder.IceCreamList.Count > 1) //Check if ice cream list has more than one ice cream
                    {
                        currentOrder.DeleteIceCream(delOption);
                        Console.WriteLine("Ice Cream has been deleted.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error! There cannot be zero ice creams in an order!");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Please enter the number of the ice cream to be deleted!");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("No such ice cream exists! Please enter one of the options.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error! Ice cream not found!");
                }
            }

        }

        void ModifyOrder(Dictionary<int, Customer> customerDict)
        {
            //Listing all customers
            DisplayAllCustomers(customerDict);
            while (true)
            {
                try
                {
                    //Prompting user to select a customer
                    Console.Write("Enter customer ID (0 to cancel): ");
                    int customerId = Convert.ToInt32(Console.ReadLine());

                    if (customerId == 0)
                    {
                        break;
                    }

                    //Retrieving selected customer's current order
                    Customer selectedCustomer = customerDict[customerId];
                    Order currentOrder = selectedCustomer.CurrentOrder;

                    if (currentOrder != null) //Check if there is any current order
                    {
                        while (true) //Data validation for choosing main options
                        {
                            try
                            {
                                //Listing all the ice cream objects contained in the order
                                int count = 1;
                                foreach (IceCream ic in currentOrder.IceCreamList)
                                {
                                    Console.WriteLine($"\n[Ice Cream {count}]{ic}");
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
                                    ModifyIceCream(currentOrder); //Call Modify Ice Cream function

                                }
                                else if (option == 2) //Option 2 - Add entirely new ice cream
                                {
                                    AddNewIceCream(currentOrder); //Call Add new ice cream function
                                }
                                else if (option == 3)
                                {
                                    DeleteIceCream(currentOrder); //Call delete ice cream function
                                }
                                else
                                {
                                    throw new ArgumentOutOfRangeException();
                                }

                                Console.WriteLine("\nUpdated Order\n" +
                                    "-------------------");
                                Console.WriteLine(currentOrder);
                                break;
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine("Incorrect format! Please enter a number between 1-3!");
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                Console.WriteLine("Error! Option must be between 1-3!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Please enter an option between 1-3!");
                            }
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Customer ID [{customerId}] does not have any current orders. Please make an order first.");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Incorrect format! Please a valid MemberID!");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Please a valid MemberID!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please a valid MemberID");
                }
            }

        }


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
                Console.WriteLine($"Ice Cream [{count}]" +
                    $"{ic}\n");

                if (ic.CalculatePrice() > mostExIceCreamPrice) //Checking if ice cream is most expensive in the order
                {
                    mostExIceCreamPrice = ic.CalculatePrice();
                }
                count++;
            }

            //Displaying the total bill amount
            double orderTotal = order.CalculateTotal();
            Console.WriteLine($"\nTotal bill amount: ${orderTotal:f2}");

            //Finding customer 
            Customer customer = new Customer();
            foreach (Customer c in customerDict.Values) //Iterating through customerDict
            {
                if (c.CurrentOrder.Id == order.Id) //Checking if customer's current order id is the same as the checkout order id
                {
                    customer = c; //If it is, assign customer for the order
                    break;
                }
            }
            Console.WriteLine(customer.Rewards);
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
                        Console.WriteLine($"\nCustomer [{customer.MemberId}] has {customer.Rewards.Points} points to redeem.");
                        Console.Write("Number of points to redeem (1 point = $0.02) : ");
                        int pointsToRedeem = Convert.ToInt32(Console.ReadLine());

                        if (pointsToRedeem <= customer.Rewards.Points) //Check if customer has sufficient points
                        {
                            if ((pointsToRedeem * 0.02) > orderTotal) //Check if the pointsToRedeem price exceeds the order total
                            {
                                int newPointsToRedeem = Convert.ToInt32(Math.Ceiling(orderTotal / 0.02)); //Points to redeem after offsetting the leftover points
                                int leftoverPoints = pointsToRedeem - newPointsToRedeem;
                                orderTotal -= newPointsToRedeem * 0.02; //Redeeming customers points (1 point = $0.02)
                                customer.Rewards.RedeemPoints(newPointsToRedeem);
                                Console.WriteLine($"{newPointsToRedeem} points used. Customer [{customer.MemberId}] has {leftoverPoints} left.");
                            }
                            else
                            {
                                customer.Rewards.RedeemPoints(pointsToRedeem);
                                orderTotal -= pointsToRedeem * 0.02; //Redeeming customers points (1 point = $0.02)
                                Console.WriteLine($"{pointsToRedeem} points [${pointsToRedeem * 0.02:f2}] have been redeemed. Customer [{customer.MemberId}] has {customer.Rewards.Points} left.");
                            }
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
            Console.WriteLine($"Final bill amount: ${orderTotal:f2}");

            //Prompting user any key to make payment
            Console.Write("Enter any key to make payment: ");
            Console.ReadLine();

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
            using (StreamWriter writer = new StreamWriter("orders.csv", true))
            {
                foreach (IceCream ic in order.IceCreamList)
                {
                    //Checking if ice cream is a cone
                    string dipped = "";
                    if (ic is Cone)
                    {
                        Cone cone = (Cone)ic; //Need to downcast to access cone properties
                        dipped = cone.Dipped.ToString().ToUpper(); //Assigning dipped boolean as a upper case string 
                    }

                    //Checking if ice cream is a waffle
                    string waffleFlavour = "";
                    if (ic is Waffle)
                    {
                        Waffle waffle = (Waffle)ic; //Need to downcast to access waffle properties
                        waffleFlavour = waffle.WaffleFlavour; //Assigning waffle flavour to waffleFlavour
                    }

                    //Formatting all the flavours neatly
                    string flavours = "";
                    int flavourCount = 0; //keep track of how many flavours have been added
                    foreach (Flavour f in ic.Flavours)
                    {
                        flavours += $"{f.Type},"; //stringing it together
                        flavourCount++; //icnrement flavour count by 1
                    }
                    if (flavourCount < 3)
                    {
                        for (int i = 0; i < (3 - flavourCount); i++)
                        {
                            flavours += ",";
                        }
                    }

                    //Formatting all the toppings neatly
                    string toppings = "";
                    int toppingCount = 0; //keep track of how many toppings have been added
                    foreach (Topping t in ic.Toppings)
                    {
                        toppings += $"{t.Type},"; //stringing it together
                        toppingCount++; //increment flavour count by 1
                    }
                    if (toppingCount < 3)
                    {
                        for (int i = 0; i < (3 - toppingCount); i++)
                        {
                            toppings += ",";
                        }
                    }

                    writer.WriteLine($"{order.Id},{customer.MemberId},{order.TimeReceived},{order.TimeFulfilled},{ic.Option},{ic.Scoops},{dipped},{waffleFlavour},{flavours}{toppings}");
                }
            }
            Console.WriteLine(customer.Rewards);
        }

        // Advanced feature b - Display monthly harged amounts breakdown & total charges amounts for the year
        void displayChargedAmount(Dictionary<int, Customer> customerDict)
        {
            try
            {
                while (true)
                {
                    // prompt user for year
                    Console.Write("Enter the year: ");

                    if (int.TryParse(Console.ReadLine(), out int inputYear))
                    {
                        decimal[] monthlyAmounts = new decimal[12]; // Array to hold monthly amounts
                        decimal totalAmount = 0;

                        foreach (var customer in customerDict.Values)
                        {
                            foreach (var order in customer.OrderHistory)
                            {
                                if (order.TimeFulfilled.HasValue && order.TimeFulfilled.Value.Year == inputYear)
                                {
                                    int month = order.TimeFulfilled.Value.Month - 1; // Month index in array (0-based)

                                    decimal orderAmount = Convert.ToDecimal(order.CalculateTotal()); // Ensure orderAmount is decimal

                                    monthlyAmounts[month] += orderAmount;
                                    totalAmount += orderAmount;
                                }
                            }
                        }

                        Console.WriteLine();

                        // Display monthly amounts
                        for (int i = 0; i < 12; i++)
                        {
                            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i + 1); // Get 3-letter month name
                            Console.WriteLine($"{monthName} {inputYear}: {monthlyAmounts[i]:C}");
                        }

                        Console.WriteLine($"\nTotal {inputYear}: {totalAmount:C}");
                        break;
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid year.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Invalid input. The entered year is too large.");
            }

        }

        //Main Program
        InitFlavours(flavourData); //Reading data from flavours.csv
        InitToppings(toppingData); //Reading data from toppings.csv
        InitCustomers(customerDict); //Reading data from customers.csv
        InitOrders(customerDict); //Reading data from orders.csv

        Order order1 = new Order(12, DateTime.ParseExact("27/10/2023 13:28", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
        //Order order1 = new Order(12, Convert.ToDateTime("27 / 10 / 2023 13:28"));
        List<Flavour> flavours = new List<Flavour>();
        List<Topping> toppings = new List<Topping>();
        flavours.Add(new Flavour("Chocolate", false, 3));
        toppings.Add(new Topping("Sprinkles"));
        IceCream ic = new Cone("Cone", 3, flavours, toppings, false);
        order1.AddIceCream(ic);
        regularQueue.Enqueue(order1);
        customerDict[685582].CurrentOrder = order1;

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
                else if (option == 4) //Option 4 - Create a customer's order
                {
                    Console.WriteLine("\nOption 4 - Create a customer's order");
                    Console.WriteLine("----------------------------------");
                    CreateCustomerOrder(customerDict);
                }
                else if (option == 5) //Option 5 - Display the details of a customer
                {
                    Console.WriteLine("\nOption 5 - Display order details of a customer");
                    Console.WriteLine("-------------------------------------------------\n");
                    ListOrderDetails(customerDict);
                }
                else if (option == 6) //Option 6 - Modify order details
                {
                    Console.WriteLine("\nOption 6 - Modify order details");
                    Console.WriteLine("----------------------------------");
                    ModifyOrder(customerDict);
                }
                else if (option == 7) //Option 7 - Process an order and checkout (Advanced feature - a)
                {
                    Console.WriteLine("\nOption 7 - Process an order and checkout");
                    Console.WriteLine("--------------------------------------------");
                    ProcessCheckoutOrder();
                }
                else if (option == 8)
                {
                    Console.WriteLine("\nOption 8 - Display monthly charged amounts breakdown & total charged amounts for the year");
                    Console.WriteLine("--------------------------------------------------------------------------------------------");
                    displayChargedAmount(customerDict);
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
        Console.WriteLine();
    }
}
