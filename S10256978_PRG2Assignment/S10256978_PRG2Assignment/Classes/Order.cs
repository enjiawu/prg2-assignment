using System;
using System.Collections.Generic;
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
            IceCream icecream = IceCreamList[i - 1];

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
            return $"Order ID: {Id}\tTimeReceived: {TimeReceived}\tTimeFulfilled: {TimeFulfilled}";
        }
    }
}
