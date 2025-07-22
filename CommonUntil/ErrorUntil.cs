using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUntil
{
        public class ErrorUntil
        {
            public Error error { get; set; }
        }

        public class Error
        {
            public string code { get; set; }
            public string message { get; set; }
            public string details { get; set; }
            public Data data { get; set; }
            public Validationerror[] validationErrors { get; set; }
        }

        public class Data
        {
            public string additionalProp1 { get; set; }
            public string additionalProp2 { get; set; }
            public string additionalProp3 { get; set; }
        }

        public class Validationerror
        {
            public string message { get; set; }
            public string[] members { get; set; }
        }
}
