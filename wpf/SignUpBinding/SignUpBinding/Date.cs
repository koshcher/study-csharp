using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignUpBinding
{
    class Date
    {
        public int Year { get; set; } = 0;
        public string Month { get; set; } = "";
        public int Day { get; set; } = 0;

        public Date()
        {
            Year = 0;
            Month = "";
            Day = 0;
        }

        public override string ToString()
        {
            return Day + " of " + Month + " " + Year;
        }
    }
}
