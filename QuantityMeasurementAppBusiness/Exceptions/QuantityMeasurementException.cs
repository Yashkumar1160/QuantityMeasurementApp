    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace QuantityMeasurementAppBusiness.Exceptions
    {
        public class QuantityMeasurementException : Exception
        {
            public QuantityMeasurementException(string message) : base(message)
            {
            }

            public QuantityMeasurementException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }
    }