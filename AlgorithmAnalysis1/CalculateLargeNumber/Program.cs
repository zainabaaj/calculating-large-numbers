using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalculateLargeNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            
            while ( true)
            {
                Console.Write("Enter the first number:");
                string a = Console.ReadLine();

                Console.Write("Enter an operator (+ - *):");

                char op = Convert.ToChar(Console.ReadLine());

                Console.Write("Enter the second number:");
                string b = Console.ReadLine();

                string pattern1 = @"^-?\d*(\.\d+)?$";
                string pattern2 = @"^(\+|-|\*|/)$";

                string[] inputs = { a, b };
                foreach (var input in inputs)
                {
                    Match m1 = Regex.Match(input, pattern1);
                    if (!m1.Success)
                    {
                        Console.WriteLine("the input has be to a number. terminating");
                        Console.ReadLine();
                        return;
                    }
                }
                Match m2 = Regex.Match(op.ToString(), pattern2);

                if (!m2.Success)
                {
                    Console.WriteLine("only operators are accepted");
                    Console.ReadLine();
                    return;
                }


                LargeNumbers l1 = new LargeNumbers(a);
                LargeNumbers l2 = new LargeNumbers(b);

                if (op == '+') Console.WriteLine(l1 + l2);
                else if (op == '-') Console.WriteLine(l1 - l2);
                else if (op == '*') Console.WriteLine(l1 * l2);
                else if (op == '/') Console.WriteLine(l1 / l2);
                
            }
            
        }
    }
}
