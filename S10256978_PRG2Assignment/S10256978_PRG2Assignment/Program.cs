//Options
//==============================================================
//Wu Enjia: 2,5,6,a(7)
//Xue Wenya: 1,3,4,b(8)
//==============================================================

using Microsoft.VisualBasic.FileIO;
using S10256978_PRG2Assignment.Classes;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

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
                "[9] Order tracking and queue visualization\n" +
                "[0] Quit\n" +
                "Enter option: ");
        }

        //Initializing necessary files
        void InitFlavours(Dictionary<string, double> flavourData) //Method to initialize flavours.csv
        {
            using (StreamReader sr = new StreamReader("flavours.csv")) 
            {
                string header = sr.ReadLine(); //Reading header
                string? s;

                while ((s = sr.ReadLine()) != null)
                {
                    string[] line = s.Split(","); //Splitting the line into flavour and cost
                    string flavour = line[0];
                    double cost = Convert.ToDouble(line[1]);

                    flavourData[flavour] = cost; //Adding flavour and respective cost into flavourData
                }
            }
        }
        void InitToppings(Dictionary<string, double> toppingData) //Method to initialize toppings.csv
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
        void InitCustomers(Dictionary<int, Customer> customerDict) //Method to initialize customers.csv
        {
            using (StreamReader sr = new StreamReader("customers.csv"))
            {
                string header = sr.ReadLine(); // Read header
                string? s;

                while ((s = sr.ReadLine()) != null)
                {
                    //Splitting the line and getting all the respective values
                    string[] line = s.Split(','); 
                    string Name = line[0];
                    int MemberId = Convert.ToInt32(line[1]);
                    DateTime Dob = DateTime.ParseExact(line[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //DateTime Dob = Convert.ToDateTime(line[2]);
                    string MemberShipStatus = line[3];
                    int MemberShipPoint = Convert.ToInt32(line[4]);
                    int PunchCard = Convert.ToInt32(line[5]);

                    //Assigning to necessary classes
                    Customer customer = new Customer(Name, MemberId, Dob); //Assigning information to new customer
                    PointCard pointCard = new PointCard(MemberShipPoint, PunchCard); //Assigning information to new point card

                    pointCard.Tier = MemberShipStatus; //Changing membership status for point card
                    customer.Rewards = pointCard; //Changing point card for customer.Rewards
                    customerDict[customer.MemberId] = customer; //Adding customer into customerDict
                }

            }
        }

        // Option 1 - display all customer
        void DisplayAllCustomers(Dictionary<int, Customer> customerDict)
        {
            // Display the header
            Console.WriteLine($"{"Name",-15}{"MemberId",-15}{"Date of Birth",-15}{"Membership Status",-22}{"Membership Points",-22}{"PunchCard",-22}");

            // Iterate through each customer in the dictionary and display their information
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
            Console.WriteLine("Current orders in the regular queue"); //Printing out regular queue header
            Console.WriteLine("------------------------------------");
            foreach (Order order in regularQueue) //Iterating through every current order in the regular queue
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("\nCurrent orders in the gold members queue"); //Printing out gold queue header
            Console.WriteLine("------------------------------------------"); 
            foreach (Order order in goldMembersQueue) //Iterating through every current order in the gold queue
            {
                Console.WriteLine(order);
            }

        }

        // Option 3 - Register new customer
        void RegisterNewCustomer(Dictionary<int, Customer> customerDict)
        {

            Console.Write("Enter customer name (0 to cancel): "); // Prompt user for customer information - customer name
            string name = Convert.ToString(Console.ReadLine());

            // Check if the user wants to exit
            if (name == "0")
            {
                Console.WriteLine("Exiting customer registration.");
                return;
            }

            int memberId;
            while (true)
            {
                try
                {
                    // Prompt user to give a customer ID number
                    Console.Write("Enter customer ID number: ");
                    // Try to parse the user input as an integer and assign it to the memberId variable.
                    if (int.TryParse(Console.ReadLine(), out memberId))
                    {
                        if (customerDict.ContainsKey(memberId)) //Check the customer ID is it exists
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
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Please enter a ");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer for the customer ID.");
                }
            }

            DateTime dob;
            bool isValidDate = false;

            do
            {
                // prompt user to enter DOB for customer
                Console.Write("Enter customer date of birth (dd/MM/yyyy): ");
                string input = Console.ReadLine();

                // Try to parse the user input as a DateTime, and store the result in the 'dob' variable.
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
        void CreateCustomerOrder(Dictionary<int, Customer> customerDict, Dictionary<int, Order> orderDict)
        {
            while (true)
            {
                try //Data validation
                {
                    // Display available customers
                    Console.WriteLine("\nSelect a customer by entering their Member ID:");
                    foreach (var kvp in customerDict)
                    {
                        Console.WriteLine($"{kvp.Key}: {kvp.Value.Name}");
                    }


                    // Get member id
                    Console.Write("\nEnter Member ID (0 to exit): ");
                    int memberId;

                    while (!int.TryParse(Console.ReadLine(), out memberId) || !customerDict.ContainsKey(memberId) || memberId == 0 )
                    {
                        if (memberId == 0)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid Member ID [{memberId}]. Please enter a valid Member ID.");
                            Console.Write("Enter Member ID (0 to exit): ");
                        }
                    }

                    if (memberId == 0)
                    {
                        Console.WriteLine("Exiting Create Customer Order option.");
                        break;
                    }

                    var selectedCustomer = customerDict[memberId];
                    if (selectedCustomer.CurrentOrder != null)
                    {
                        Console.WriteLine("Customer already has an existing order in progress. Please choose a different customer ID.");
                        continue; // Skip to the next iteration of the loop
                    }


                    int orderIdCounter = 1; // Initialize a counter for order IDs
                    int orderId;

                    do
                    {
                        orderId = orderIdCounter++;
                    } while (orderDict.Keys.Contains(orderId));



                    // Create a new order for the selected customer using the MakeOrder method
                    Order newOrder = customerDict[memberId].MakeOrder();
                    newOrder.Id = orderId;
                    newOrder.TimeReceived = DateTime.Now;


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
                            Console.Write("\nWould you like to add another ice cream to the order? (Y/N): ");
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

                    // Link the new order to the customer's current order using Item property
                    Customer customer;
                    if (customerDict.TryGetValue(memberId, out customer))
                    {
                        customer.CurrentOrder = newOrder;
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
                    Console.WriteLine("\nOrder Details:");
                    Console.WriteLine($"Order ID: {newOrder.Id}");
                    Console.WriteLine($"Time Received: {newOrder.TimeReceived}");

                    // Print ice cream details
                    foreach (var iceCream in newOrder.IceCreamList)
                    {
                        Console.WriteLine($"Ice Cream: {iceCream}");
                    }

                    // Add the new order to the orderDict dictionary
                    orderDict.Add(orderId, newOrder);

                    Console.WriteLine("Order made successfully!\n");
                    break;
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging or troubleshooting
                    Console.WriteLine("An unexpected error occurred:");
                    Console.WriteLine(ex.Message);
                }
            }
        }



        //Option 5 - Display order details of a customers
        Dictionary<int, Order> orderDict = new Dictionary<int, Order>(); //Dictionary to keep track of which ice creams are in the same order and for keeping track of orders and their order id
        Dictionary<int, List<Order>> memberOrderDict = new Dictionary<int, List<Order>>(); //Dictionary to keep track of which orders are under which member

        void InitOrders(Dictionary<int, Customer> customerDict, Dictionary<int, Order> orderDict, Dictionary<int, List<Order>> memberOrderDict)
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

                    if (orderDict.Keys.Contains(id)) //If order already exists, order will get the existing order values
                    {
                        order = orderDict[id];
                    }
                    else //If order does not exist yet
                    {
                        order = new Order(id, timeReceived); //Making new order
                        orderDict[id] = order; //Add order into dictionary
                    }

                    if (!memberOrderDict.Keys.Contains(memberId)) //Check if member does not have any previous orders
                    {
                        memberOrderDict[memberId] = new List<Order>(); //Add member and order key pair into memberOrderDict
                    }

                    if (memberOrderDict[memberId].Contains(order)) //Check if memberOrderDict already contains the orderId
                    {
                        memberOrderDict[memberId][memberOrderDict[memberId].IndexOf(order)] = order; //If it does, replace it with the new one
                    }
                    else
                    {
                        memberOrderDict[memberId].Add(order); //If not, add orders to list of orders member has
                    }

                    order.TimeFulfilled = timeFulfilled; //Updating the time fulfilled

                    Dictionary<string, int> flavourCount = new Dictionary<string, int>(); //Making dictionary to hold the quantity of each flavour

                    foreach(string flavour in flavourData.Keys) //iterating through every flavour available and assigning them to the dictionary flavour count to keep track of how many instances of each flavour
                    {
                        flavourCount[flavour] = 0;
                    }

                    for (int i = 8; i <= 10; i++) //Check what flavours are included
                    {
                        if (line[i] == "") //Break the loop if empty because empty means no more flavours
                        {
                            break;
                        }
                        flavourCount[line[i]]++; //Add quantity to flavour
                    }

                    foreach (string flavour in flavourCount.Keys) //Iterating through flavourCount to see which flavours are in the order
                    {

                        if (flavourCount[flavour] != 0)
                        {
                            if (flavourData[flavour] > 0) //Check if ice cream is premium
                            {
                                Flavour newFlavour = new Flavour(flavour, true, flavourCount[flavour]); //making a new flavour
                                flavours.Add(newFlavour); //Add flavour to list of flavours for current ice cream
                            }
                            else
                            {
                                Flavour newFlavour = new Flavour(flavour, false, flavourCount[flavour]); //making a new flavour
                                flavours.Add(newFlavour); //Add flavour to list of flavours for current ice cream
                            } 
                        }
                    }

                    for (int i = 11; i <= 14; i++) //Check what toppings are included
                    {
                        if (line[i] == "") //Break the loop if empty because empty means no more toppings
                        {
                            break; //Break from for loop
                        }
                        else
                        {
                            Topping topping = new Topping(line[i]); //Make topping
                            toppings.Add(topping); //Add topping to list of toppings for current ice cream
                        }
                    }

                    if (option == "Cup") //Checking which option the ice cream is
                    {
                        IceCream icecream = new Cup(option, scoops, flavours, toppings); //Assigning values to new icecream
                        order.AddIceCream(icecream); //Adding icecream to order
                    }
                    else if (option == "Cone")
                    {
                        bool dipped = Convert.ToBoolean(line[6]);
                        IceCream icecream = new Cone(option, scoops, flavours, toppings, dipped); //Assigning values to new icecream
                        order.AddIceCream(icecream); //Adding icecream to order
                    }
                    else
                    {
                        string waffleFlavour = line[7];
                        IceCream icecream = new Waffle(option, scoops, flavours, toppings, waffleFlavour); //Assigning values to new icecream
                        order.AddIceCream(icecream); //Adding icecream to order
                    }
                }

                foreach (int mId in memberOrderDict.Keys) //Adding all the orders into their respective customer OrderHistory
                {
                    foreach (Order order in memberOrderDict[mId]) //Iterating through every order in the memberOrderDict
                    {
                        customerDict[mId].OrderHistory.Add(order); //Adding order into the customer's orderdict
                    }
                }
            }
        }

        //Listing Customer Order Details
        void ListOrderDetails(Dictionary<int, Customer> customerDict)
        {
            //Listing the customers
            DisplayAllCustomers(customerDict);

            while (true)
            {
                try //Data Validation
                {
                    //Prompting user to select a customer
                    Console.Write("\nEnter customer ID (0 to return): ");
                    int customerId = Convert.ToInt32(Console.ReadLine());

                    if (customerId == 0) //Returning to the main menu
                    {
                        break; //breaking from the loop
                    }

                    //Retrieving the selected customer
                    Customer selectedCustomer = customerDict[customerId];

                    //Displaying all the details of the order
                    Console.WriteLine("\nPast Orders");
                    Console.WriteLine("-------------------------------------------------------------");

                    int count = 1;
                    foreach (Order order in selectedCustomer.OrderHistory) //Iterating through every order in the selected customer's order history
                    {
                        Console.WriteLine($"[{count}] {order}\n"); //Printing out the order
                        count++;
                    }

                    Console.WriteLine("\nCurrent Order");
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine(selectedCustomer.CurrentOrder); //Printing out selected customers current order
                    break;
                }
                catch (FormatException ex) 
                {
                    Console.WriteLine($"Incorrect format! CustomerID not found. Please try again.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Customer ID not found! Please try again.");
                }
            }
           
        }

        //Option 6 - Modifying order details
        void ModifyIceCream(Order currentOrder) //Method to modify ice cream
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
                    else if (icNo < 1 || icNo > currentOrder.IceCreamList.Count()) //Making sure the ice cream number is in range
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
                        break; //breaking out of loop 
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(); //throwing new error if option does not exist
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

            int scoops; //To keep track of the number of scops user entered
            while (true) //Data validation for number of scoops
            {
                try
                {
                    Console.Write("Enter number of scoops: ");
                    scoops = Convert.ToInt32(Console.ReadLine());

                    if (scoops > 3 || scoops <= 0) //Check if number of scoops is between 1 and 3
                    {
                        throw new ArgumentOutOfRangeException(); //throwing new error if number of scoops is not between 1-3
                    }
                    else
                    {
                        break; //if scoops is between 1-3, break from while loop
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
                foreach (string f in flavourData.Keys) //iterating through all the available flavours in flavourData.Keys
                {
                    Console.WriteLine($"{f}");
                }
                Console.WriteLine();

                string flavourOption; //to hold the flavour option input by user
                while (true) //Making sure that all scoops have a selected flavour
                {
                    while (true) //Data validation for flavour option
                    {
                        try
                        {
                            Console.Write("Enter flavour to add: ");

                            flavourOption = Console.ReadLine().ToLower(); //checking if its Sea Salt 
                            if (flavourOption.Split(" ").Length > 1) //formatting the option to make sea salt capitalized
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

                            if (!flavourData.ContainsKey(flavourOption)) //Check if flavourData contains flavour option
                            {
                                throw new FormatException(); //if it does not, throw new error
                            }
                            else
                            {
                                break; //if not, break from while loop
                            }
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine("No such flavour exists! Please enter an available flavour.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Please enter an available flavour.");
                        }
                    }

                    //Check for quantity of flavour
                    int quantity; //to keep track of quantity user entered
                    while (true) //For data validation
                    {
                        try
                        {
                            Console.Write("Enter quantity of flavour (0 to choose another flavour) : ");
                            quantity = Convert.ToInt32(Console.ReadLine());
                            if (quantity > scoopCount || quantity < 0) //Making sure quantity of selected flavour does not exceed the number of scoops remaining or is negative
                            {
                                throw new ArgumentOutOfRangeException(); //throw new error if quantity exceeds number of scoops remaining or is negative
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
                        bool optionExists = false; //boolean to check if the option already exists in flavours
                        foreach(Flavour f in flavours) //iterating through all the flavours in flavours
                        {
                            if (f.Type == flavourOption) //checking if the flavour already exists
                            {
                                f.Quantity += quantity; //Adding the quantity and not an entirely new flavour if the flavour already exists
                                optionExists = true; //change option exists to true
                                break; //breaking from for loop
                            }
                        }

                        if (!optionExists) //if the flavour doesn't exist yet, add a new flavour to flavours
                        {
                            if (flavourData[flavourOption] > 0) //Check if ice cream is premium
                            {
                                Flavour newFlavour = new Flavour(flavourOption, true, quantity); //making a new flavour
                                flavours.Add(newFlavour); //adding flavour to flavours list
                            }
                            else
                            {
                                Flavour newFlavour = new Flavour(flavourOption, false, quantity); //making a new flavour
                                flavours.Add(newFlavour); //adding flavour to flavours list
                            }
                            Console.WriteLine("Flavour has been added."); //printing out message
                        }


                        flavourCount += quantity; //Add the quantity to the number of flavours that have been added 

                        //Quit for loop if the quantity of flavours have been hit
                        if (flavourCount >= scoops)
                        {
                            Console.WriteLine("Maximum number of flavours have been reached.");
                            break; //break from loop
                        }
                    }

                }

                int countTopping = 0; //To check if maximum number of toppings has been reached
                while (countTopping < 4) //getting toppings for new ice cream
                {
                    Console.Write("\nAvailable Toppings\n" + 
                            "----------------\n");
                    foreach (string t in toppingData.Keys) //printing out available toppings
                    {
                        Console.WriteLine($"{t}");
                    }

                    string toppingOption; //to hold the toppingOption entered by user
                    while (true) //Data validation for topping input
                    {
                        try
                        {
                            Console.Write("Enter new topping (0 to continue): ");

                            toppingOption = Console.ReadLine();

                            if (toppingData.ContainsKey(char.ToUpper(toppingOption[0]) + toppingOption.Substring(1).ToLower()) || toppingOption == "0") //Checking if topping exists
                            {
                                break; //breaking the loop
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException(); //throw new error if topping does not exist in toppingData
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

                    if (toppingOption == "0") //checking if user wants to continue with the other options
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
                if (icOption == "Cone") //checking if icOption is a cone
                {
                    bool dipped; //to keep track if ice cream is dipped or not
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
                                dipped = true; //changing dipping if option is 1
                                break;
                            }
                            else if (coneOption == 2)
                            {
                                dipped = false; //changing dipping if option is 2
                                break;
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException(); //if option entered is not 1 or 2, throw new error
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
                    iceCream = new Cone("Cone", scoops, flavours, toppings, dipped); //making ice cream a cone with all the necessary properties
                }
                else if (icOption == "Waffle") //if icOption is waffle
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
                            int waffleOption = Convert.ToInt32(Console.ReadLine()); //reading user input

                            if (waffleOption == 1) //checking what option user input and assigning waffle flavour based on that
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
                                throw new ArgumentOutOfRangeException(); //throw new error if flavour is not in the available options
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
                    iceCream = new Waffle("Waffle", scoops, flavours, toppings, waffleFlavour); //making ice cream a waffle with all the necessary properties
                }
                else 
                {
                    iceCream = new Cup("Cup", scoops, flavours, toppings); //making ice cream a cup with all the necessary properties
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
                        throw new ArgumentOutOfRangeException(); //throw new error if delOption is not valid
                    }
                    else if (currentOrder.IceCreamList.Count > 1) //Check if ice cream list has more than one ice cream
                    {
                        currentOrder.DeleteIceCream(delOption); //Calling DeleteIceCream function from order.cs
                        Console.WriteLine("Ice Cream has been deleted."); //Printing out message to let user know
                        break; //breaking from while loop
                    }
                    else //if there is only one icre cream in the order
                    {
                        Console.WriteLine("Error! There cannot be zero ice creams in an order!"); //printing out error message
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

        void ModifyOrder(Dictionary<int, Customer> customerDict) //Function to be called in the main program from option 6
        {
            //Listing all customers
            DisplayAllCustomers(customerDict);
            while (true) //Data validation
            {
                try
                {
                    //Prompting user to select a customer
                    Console.Write("Enter customer ID (0 to cancel): ");
                    int customerId = Convert.ToInt32(Console.ReadLine()); //Reading user input

                    if (customerId == 0) //Checking if user input is 0
                    {
                        break; //if so break from loop
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
                                foreach (IceCream ic in currentOrder.IceCreamList) //Iterating through all the ice creams in the customer's current order
                                {
                                    Console.WriteLine($"\n[Ice Cream {count}]{ic}");
                                    count++;
                                }

                                //Prompting user to select 1-3
                                Console.Write("\n[1] Choose an existing ice cream to modify\n" +
                                    "[2] Add an entirely new ice cream object to the order\n" +
                                    "[3] Choose an existing ice cream object to delete from the order\n" +
                                    "[0] Return\n" +
                                    "Enter your option: "); //Printing out options
                                int option = Convert.ToInt32(Console.ReadLine()); //Reading user input

                                if (option == 0) //Checking if user wants to return to home page
                                {
                                    break; //if so, break from loop
                                }
                                else if (option == 1) //Option 1 - Modifying an existing ice cream
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
                                    throw new ArgumentOutOfRangeException(); //throw new error if none of the available options are entered
                                }

                                Console.WriteLine("\nUpdated Order\n" + //printing out the updated order
                                    "-------------------");
                                Console.WriteLine(currentOrder);
                                break; //break from loop
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
                        break; //breaking out form main while loop
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
        void ProcessCheckoutOrder(Dictionary<int, Order> orderDict)
        {
            try
            {
                Order order; //Order to be checked out

                //Dequeue the first order in the queue
                if (goldMembersQueue.Count == 0 && regularQueue.Count == 0) //Checking if both queues are empty
                {
                    throw new ArgumentException(); //throwing error if they are
                }
                else if (goldMembersQueue.Count > 0) //Check if gold members queue has any orders
                {
                    order = goldMembersQueue.Dequeue(); //dequeuing the first gold member queue order
                }
                else
                {
                    order = regularQueue.Dequeue(); //dequeuing the first regular member queue order
                }

                //Display all the ice creams in the order
                int count = 1;
                double mostExIceCreamPrice = 0.00; //to keep trac of the most expensive ice cream price and deduct from total cost later
                foreach (IceCream ic in order.IceCreamList) //Iterating through all the ice creams in the order
                {
                    Console.WriteLine($"Ice Cream [{count}]" +
                        $"{ic}\n");

                    if (ic.CalculatePrice() > mostExIceCreamPrice) //Checking if ice cream is most expensive in the order
                    {
                        mostExIceCreamPrice = ic.CalculatePrice(); //Updating the ice cream price
                    }
                    count++;
                }

                //Displaying the total bill amount
                double orderTotal = order.CalculateTotal(); //Getting the total for the order
                Console.WriteLine($"\nTotal bill amount: ${orderTotal:f2}"); //Printing out the total bill amount

                //Finding customer 
                Customer customer = new Customer();

                foreach (Customer c in customerDict.Values) //Iterating through customerDict
                {
                    if (c.CurrentOrder == null) //Check if customer has a current order
                    {
                        continue; //if customer does not, continue with the next customer
                    }
                    else if (c.CurrentOrder.Id == order.Id) //Checking if customer's current order id is the same as the checkout order id
                    {
                        customer = c; //If it is, assign customer for the order
                        break;
                    }
                }
                //Displaying the membership status & points of the customer
                Console.WriteLine($"Membership status: {customer.Rewards.Tier}\n" +
                    $"Points: {customer.Rewards.Points}");

                //Checking if its customer's birthday
                if (customer.IsBirthday())
                {
                    orderTotal -= mostExIceCreamPrice; //Making the most expensive ice cream in the order cost $0.00
                    Console.WriteLine($"It's customers [{customer.MemberId}]'s Birthday! The most expensive ice cream (${mostExIceCreamPrice}) is free!\n" +
                        $"New Order Total: {orderTotal}");
                }

                //Checking if customer has completed their punch card
                IceCream firstIceCream = order.IceCreamList[0]; //Finding the first ice cream in the order
                if (customer.Rewards.PunchCard == 10)
                {
                    orderTotal -= firstIceCream.CalculatePrice(); //Making the first ice cream in the order $0.00
                    customer.Rewards.PunchCard = 0; //Resetting punch card back to 0
                    Console.WriteLine($"Customer [{customer.MemberId}] has obtained 10 punches in his punch card. The first ice cream is free!\n" +
                        $"New Order Total: ${orderTotal}");
                }

                //Checking point card status and number of points to determine if the customer can redeem points and checking if they're order is not free
                if ((customer.Rewards.Tier == "Silver" || customer.Rewards.Tier == "Gold") && customer.Rewards.Points != 0 && orderTotal != 0)
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
                                if (pointsToRedeem == 0) //Check if customer does not want to redeem any points
                                {
                                    Console.WriteLine("No points have been redeemed.");
                                }
                                else if (pointsToRedeem < 0) //throw new error if points lesser than 0
                                {
                                    throw new ArgumentOutOfRangeException();
                                }
                                else if ((pointsToRedeem * 0.02) > orderTotal) //Check if the pointsToRedeem price exceeds the order total
                                {
                                    int newPointsToRedeem = Convert.ToInt32(Math.Ceiling(orderTotal / 0.02)); //Points to redeem after offsetting the leftover points
                                    int leftoverPoints = pointsToRedeem - newPointsToRedeem;
                                    orderTotal -= newPointsToRedeem * 0.02; //Redeeming customers points (1 point = $0.02)
                                    customer.Rewards.RedeemPoints(newPointsToRedeem);
                                    Console.WriteLine($"{newPointsToRedeem} points used. Customer [{customer.MemberId}] has {leftoverPoints} left.");
                                }
                                else
                                {
                                    customer.Rewards.RedeemPoints(pointsToRedeem); //Redeem points
                                    orderTotal -= pointsToRedeem * 0.02; //Redeeming customers points (1 point = $0.02)
                                    Console.WriteLine($"{pointsToRedeem} points [${pointsToRedeem * 0.02:f2}] have been redeemed. Customer [{customer.MemberId}] has {customer.Rewards.Points} left.");
                                }
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException(); //throw new error if points to redeem is not in range
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
                else if (orderTotal == 0)
                {
                    //If order is free, just skip to paying the final bill amount
                }
                else if (customer.Rewards.Points == 0) //Check if customer has no points
                {
                    Console.WriteLine($"Customer [{customer.MemberId}] has no points to redeem.");
                }
                else //Check if customer is eligible for point redeemption
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
                    customer.Rewards.Punch(); //Punch customer punch card
                }

                if (customer.Rewards.PunchCard > 10) //Checking if customer punch card has more than 10 punches
                {
                    customer.Rewards.PunchCard = 10; //If it does, set it back down to 10
                }

                //Earning points
                int pointsToAdd = Convert.ToInt32(Math.Floor(orderTotal * 0.72));
                customer.Rewards.AddPoints(pointsToAdd);

                //Updating member status 
                if (customer.Rewards.Points >= 100) //Update to gold if reward points greater or equals to 100
                {
                    customer.Rewards.Tier = "Gold";
                }
                else if (customer.Rewards.Points >= 50 && customer.Rewards.Tier != "Gold") //Upgrade to silver is reward points greater or equals to 50 and the customer tier is not gold
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
                            for (int i = 0; f.Quantity > i; i++)
                            {
                                flavours += $"{f.Type},"; //stringing it together
                                flavourCount++; //increment flavour count by 1
                            }
                            
                        }
                        if (flavourCount < 3) //Check if flavour count is lesser than 3, if it is then just append ','
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
                        if (toppingCount < 3) //Check if toppings is lesser than 3 , if it is then just append ',' (leave out the last comma)
                        {
                            for (int i = 0; i < (3 - toppingCount); i++)
                            {
                                toppings += ",";
                            }
                        }

                        writer.WriteLine($"{order.Id},{customer.MemberId},{order.TimeReceived},{order.TimeFulfilled},{ic.Option},{ic.Scoops},{dipped},{waffleFlavour},{flavours}{toppings}"); //Formatting the order to be added into the orders.csv
                    }
                }
                Console.WriteLine(customer.Rewards); //Printing out the new customer rewards
                customer.CurrentOrder = null; //Resetting the current order back to nothing
                orderDict[order.Id] = order; //Updating order details
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("There are no current orders in the queue now. Try again after an order has been added.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Advanced feature b - Display monthly charged amounts breakdown & total charges amounts for the year
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

        //Advanced C - Order tracking: Real-time queue visualization for customers to see their order position and estimated wait time
        void OrderTracking(Queue<Order> regularQueue, Queue<Order> goldMembersQueue)
        {
            /* 
            Assuming that time taken to make ice cream is:
            Waffle - 3 minutes
            Cone/Cup - 0 minutes (Pre-prepared)
            Scoop - 10 seconds * Number of scoops
            Toppings - 5 seconds * Number of toppings
            Wait time person at checkout - 15 second * Number of ice creams
            */
            Dictionary<string, int> timeTaken = new Dictionary<string, int>(); //Dictionary to hold data of time taken for each ice cream
            timeTaken["waffle"] = 3 * 60;
            timeTaken["scoop"] = 10;
            timeTaken["topping"] = 5;
            timeTaken["checkout"] = 15;

            if (regularQueue.Count() == 0 && goldMembersQueue.Count() == 0) //Check if there are any others in the queue now
            {
                Console.WriteLine("There are currently no orders in the queue!");
                return;
            }

            ListCurrentOrders(regularQueue, goldMembersQueue); //Printing out all the current orders

            while (true)
            {
                try
                {
                    Console.Write("Enter order number (0 to cancel): ");
                    int orderId = Convert.ToInt32(Console.ReadLine());
                    Order order;

                    if (orderId == 0) //break from while loop and return to main program
                    {
                        break;
                    }

                    bool orderUpdated = false; //Boolean to check if order has been updated
                    int goldPosition = 0; //To keep track of number of orders in gold members queue before the order entered
                    bool orderGoldQueue = false; //Boolean to keep track if order is in the gold queue 
                    double waitingTime = 0; //To keep track of waiting time

                    foreach(Order o in goldMembersQueue) //iterate through every order in the gold queue to find a matching order there
                    {
                        if (o.Id == orderId)
                        {
                            order = o; //update order details
                            orderGoldQueue = true; //change to true
                            orderUpdated = true; //change to true a
                            break; //break from for loop
                        }
                        else
                        {
                            goldPosition++; 
                        }

                        //Calculating time taken for each order
                        foreach (IceCream ic in o.IceCreamList)
                        {
                            if (ic.Option.ToLower() == "waffle") //if option is waffle, plus 3 minutes to waiting time
                            {
                                waitingTime += timeTaken["waffle"];
                            }

                            waitingTime += ic.Scoops * timeTaken["scoop"]; //add time for each scoop
                            waitingTime += ic.Toppings.Count() * timeTaken["topping"]; //add time for each topping
                            waitingTime += timeTaken["checkout"]; //adding checkout time per person
                        }

                    }

                    int regularPosition = 0; //To keep track of number of orders in regular queue before the order entered
                    if (orderGoldQueue != true)
                    {
                        foreach (Order o in regularQueue)
                        {
                            if (o.Id == orderId)
                            {
                                order = o; //update order details
                                orderUpdated = true; //change to true
                                break; //break from for loop
                            }
                            else
                            {
                                regularPosition++;
                            }

                            //Calculating time taken for each order
                            foreach (IceCream ic in o.IceCreamList)
                            {
                                if (ic.Option.ToLower() == "waffle") //if option is waffle, plus 3 minutes to waiting time
                                {
                                    waitingTime += timeTaken["waffle"];
                                }

                                waitingTime += ic.Scoops * timeTaken["scoop"]; //add time for each scoop
                                waitingTime += ic.Toppings.Count() * timeTaken["topping"]; //add time for each topping
                                waitingTime += timeTaken["checkout"]; //adding checkout time per person
                            }
                        }

                    }

                    if (!orderUpdated)
                    {
                        throw new ArgumentOutOfRangeException(); //throw error that order could not be found
                    }

                    Console.WriteLine($"Order [{orderId}]\n" +
                        $"There are currently [{regularPosition + goldPosition}] orders in front of order [{orderId}].\n" +
                        $"The waiting time until order [{orderId}] is approximately {waitingTime / 60.0:f2} minutes.\n");
                }
                catch(FormatException ex)
                {
                    Console.WriteLine("Incorrect format! Please only enter a number!");
                }
                catch(ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("No such order exists in the current queues! Please only enter an order that exists!");
                }
            }

        }

        //Main Program
        InitFlavours(flavourData); //Reading data from flavours.csv
        InitToppings(toppingData); //Reading data from toppings.csv
        InitCustomers(customerDict); //Reading data from customers.csv
        InitOrders(customerDict, orderDict, memberOrderDict); //Reading data from orders.csv

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
                    Console.WriteLine("------------------------------");
                    DisplayAllCustomers(customerDict);
                }
                else if (option == 2) //Option 2 - List all current orders
                {
                    Console.WriteLine("\nOption 2 - List all current orders");
                    Console.WriteLine("----------------------------------");
                    ListCurrentOrders(regularQueue, goldMembersQueue);
                }
                else if (option == 3) //Option 3 - Register a new customer
                {
                    Console.WriteLine("\nOption 3 - Register a new customer");
                    Console.WriteLine("----------------------------------");
                    RegisterNewCustomer(customerDict);
                }
                else if (option == 4) //Option 4 - Create a customer's order
                {
                    Console.WriteLine("\nOption 4 - Create a customer's order");
                    Console.WriteLine("----------------------------------");
                    CreateCustomerOrder(customerDict, orderDict);
                }
                else if (option == 5) //Option 5 - Display the details of a customer
                {
                    Console.WriteLine("\nOption 5 - Display order details of a customer");
                    Console.WriteLine("-------------------------------------------------");
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
                    ProcessCheckoutOrder(orderDict);
                }
                else if (option == 8)
                {
                    Console.WriteLine("\nOption 8 - Display monthly charged amounts breakdown & total charged amounts for the year");
                    Console.WriteLine("--------------------------------------------------------------------------------------------");
                    displayChargedAmount(customerDict);
                }
                else if (option == 9)
                {
                    Console.WriteLine("\nOption 9 - Order tracking and queue visualization");
                    Console.WriteLine("-----------------------------------------------------");
                    OrderTracking(regularQueue, goldMembersQueue); 
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
