using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public class Feet
    {
        //Encapsulated value field 
        private readonly double value;

        //Constructor
        public Feet(double value)
        {
            this.value = value;
        }


        //Override Equals Method
        public override bool Equals(object obj)
        {
            //Check same reference
            if (this == obj)
            {
                return true;
            }

            //Check for different type and null
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            //Safe Type Casting
            Feet feet = (Feet)obj;

            //Compare Values 
            return value.CompareTo(feet.value) == 0;
        }
        
        //Override GetHashCode method
        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
    }
}