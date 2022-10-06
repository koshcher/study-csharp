using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    interface Strategy
    {
        double Count(double first, double second);
    }

    class DivisionStrategy : Strategy
    {
        public double Count(double first, double second)
        {
            return first / second;
        }
    }

    class MultiplicationStrategy : Strategy
    {
        public double Count(double first, double second)
        {
            return first * second;
        }
    }

    class DifferenceStrategy : Strategy
    {
        public double Count(double first, double second)
        {
            return first - second;
        }
    }

    class SumStrategy : Strategy
    {
        public double Count(double first, double second)
        {
            return first + second;
        }
    }
}
