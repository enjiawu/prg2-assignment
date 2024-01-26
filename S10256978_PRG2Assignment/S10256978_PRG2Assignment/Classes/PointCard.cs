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
    internal class PointCard
    {
        //Declaring properties 
        public int Points { get; set; }
        public int PunchCard { get; set; }
        public string Tier { get; set; }

        //Constructors
        public PointCard() { }
        public PointCard(int p, int pc)
        {
            Points = p;
            PunchCard = pc;
        }
        
        //Methods
        public void AddPoints(int p)
        {
            Points += p;
        }
        public void RedeemPoints(int p)
        {
            Points -= p;
        }
        public void Punch() // To keep track of number of punches in point card
        {
            PunchCard ++; 
        }

        public override string ToString()
        {
            return $"Points: {Points}\tPunchCard: {PunchCard}\tTier: {Tier}";
        }
    }
}
