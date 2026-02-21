using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementApp
    {
        public static void Main(string[] args)
        {
            //take first value
            Console.WriteLine("Enter First Value (In Feet): ");
            double firstValue=double.Parse(Console.ReadLine());            

            //take first value
            Console.WriteLine("Enter Second Value (In Feet): ");
            double secondValue=double.Parse(Console.ReadLine());

            //Create Instances of Feet class 
            Feet first=new Feet(firstValue);
            Feet second=new Feet(secondValue);

            //Check Both Values
            Console.WriteLine(first.Equals(second)?
            "Both Values Are Equal":"Both Values Are Different");
        }   
    }
}