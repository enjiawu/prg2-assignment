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
            Tier = "Ordinary";
        }
        
        //Methods
        public void AddPoints(int p)
        {
            Points += p;
            if (Points >= 50)
            {
                Tier = "Silver";
            }
            else if (Points >= 100)
            {
                Tier = "Gold";
            }
        }
        public void RedeemPoints(int p)
        {
            Points -= p;
        }
        public void Punch()
        {
            PunchCard = 0;
        }

        public override string ToString()
        {
            return $"Points: {Points}\tPunchCard: {PunchCard}\tTier: {Tier}";
        }
    }
}
