//==========================================================
// Student Number : S10256978
// Student Name : Wu Enjia
// Partner Name : Xue Wenya
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256978_PRG2Assignment.Classes
{
    internal class Customer
    {
        //Declaring properties 
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime Dob {  get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; } = new List<Order>();
        public PointCard Rewards { get; set; } 

        //Constructors
        public Customer() { }
        public Customer(string n, int m, DateTime d)
        {
            Name = n;
            MemberId = m;
            Dob = d;
        }

        //Methods
        public Order MakeOrder() 
        {
            CurrentOrder = new Order(MemberId, DateTime.Now); //Make new order
            OrderHistory.Add(CurrentOrder); //Add into order history
            return CurrentOrder;
        }
        public bool IsBirthday()
        {
            if (Dob == DateTime.Now) //Check if today is birthday
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            string history = "";
            foreach (Order order in OrderHistory)
            {
                history += order + "\n";
            }
            return $"Name: {Name}\tMemberID: {MemberId}\tDOB: {Dob.ToString("dd/MM/yyyy")}\tCurrentOrder: {CurrentOrder}\tOrderHistory: {history}\tRewards: {Rewards}";
        }
    }
}
