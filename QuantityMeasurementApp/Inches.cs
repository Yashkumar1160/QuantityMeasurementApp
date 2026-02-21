using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public class Inches
    {
        //Encapsulated value field
        private readonly double value;

        //Constructor
        public Inches(double value)
        {
            this.value = value;
        }

        //Override Equals method
        public override bool Equals(object obj)
        {
            //Check same reference
            if (this == obj)
                return true;

            //Check null or different type
            if (obj == null || obj.GetType() != this.GetType())
                return false;

            //Safe Type casting
            Inches inches = (Inches)obj;

            //Compare values
            return value.CompareTo(inches.value) == 0;
        }

        
        //Override GetHashCode method
        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

    }
}