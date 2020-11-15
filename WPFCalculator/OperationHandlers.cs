using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCalculator
{
    class OperationHandlers
    {
        public static double Add(double x, double y) => x + y;
        public static double Subtract(double x, double y) => x - y;
        public static double Multiply(double x, double y) => x * y;
        public static double Divide(double x, double y)
        {
            if(y == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero");
            }
            return x / y; 
        }
    }
}
