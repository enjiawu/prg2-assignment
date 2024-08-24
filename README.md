# I.C.Treats Ice Cream Shop Management System

## Overview

I.C.Treats is an ice cream shop management system developed as part of the Programming 2 module. The system is built using a C# console application and is designed to automate various aspects of managing an ice cream shop. It handles customer registrations, order processing, and rewards management.

## Features

### Customer Management

- **List All Customers**: Display detailed information of all registered customers, including their membership status and reward points.
  
- **Register a New Customer**: Add new customers to the system by entering their name, ID number, and birthdate. Each customer receives a PointCard to track their rewards.

### Order Management

- **List All Current Orders**: Show all active orders, organized into separate queues for gold members and regular customers.
  
- **Create a Customer Order**: Allow users to create new orders for customers, including options for scoop count, flavors, and toppings. Orders are added to the appropriate queue based on the customer's membership status.

- **Display Order Details**: View the history of orders for a specific customer, including order dates and ice cream details.
  
- **Modify Order Details**: Update existing orders by adding, changing, or removing items based on customer preferences.

### Checkout and Rewards

- **Process an Order and Checkout**: Complete orders by dequeuing them, calculating the total bill, applying birthday discounts, membership points, and punch card rewards. Update the customer’s reward points and status accordingly.

- **Apply Discounts and Rewards**: Manage discounts for birthdays and promotions, and apply them during checkout. Adjust reward points based on purchase history.

### Queue Management

- **Priority Order Processing**: Ensure that orders from gold members are processed before those from regular customers to provide premium service for loyal customers.

## Notes

- This application is a console-based system and does not include features such as graphical user interfaces or web-based interactions.
- The system is intended for use as a school assignment and does not include production-level features or optimizations.

