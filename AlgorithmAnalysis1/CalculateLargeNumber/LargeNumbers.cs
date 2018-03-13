using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CalculateLargeNumber
{
    public enum NumSign { Positive, Negative}
    public class LargeNumbers
    {
        public NumSign Sign { get; private set; }
        public int DecimalPlaces { get; private set; }
        public ReadOnlyCollection<int> Digits { get; private set; }
        public LargeNumbers()
        {

        }
       public LargeNumbers(string a)
        {
            int position = 0;
            List<int> digits = new List<int>();
            if (a[0] == '-')
            {
                Sign = NumSign.Negative;
                position++;
            }
            else { Sign = NumSign.Positive; }
            DecimalPlaces = 0;
            for (; position < a.Length; position++)
            {
                if (a[position] >= '0' && a[position] <= '9')
                {
                    digits.Add(a[position] - '0');
                }

                else if (a[position] == '.')
                {
                    DecimalPlaces = a.Length - position - 1;

                }
                Digits = new ReadOnlyCollection<int>(digits);
            }

        }
         LargeNumbers(NumSign sign, IEnumerable<int> digits, int decimalPlaces)
        {
            Sign = sign;
            Digits = new ReadOnlyCollection<int>( digits.ToList());
            DecimalPlaces = decimalPlaces;

        }
        public static LargeNumbers Add(LargeNumbers first, LargeNumbers second)
        {
            if (first.Sign == NumSign.Negative && second.Sign == NumSign.Negative)
            {
                LargeNumbers positiveFirst = new LargeNumbers(NumSign.Positive, first.Digits, first.DecimalPlaces);
                LargeNumbers positiveSecond = new LargeNumbers(NumSign.Positive, second.Digits, second.DecimalPlaces);
                LargeNumbers sum = Add(positiveFirst, positiveSecond);
                return new LargeNumbers(NumSign.Negative, sum.Digits, sum.DecimalPlaces);

            }
            else if (first.Sign == NumSign.Negative && second.Sign == NumSign.Positive)
            {
                LargeNumbers positiveFirst = new LargeNumbers(NumSign.Positive, first.Digits, first.DecimalPlaces);
                
                return Sub(second ,positiveFirst );
            }
            else if (first.Sign == NumSign.Positive && second.Sign == NumSign.Negative)
            {
                LargeNumbers positiveSecond = new LargeNumbers(NumSign.Positive, second.Digits, second.DecimalPlaces);

                return Sub(first, positiveSecond);
            }
            else
            {
                List<int> digits1 = first.Digits.ToList();
                List<int> digits2 = second.Digits.ToList();
                int maxDigitsBeforeDecimalPoint = Math.Max(
                    (digits1.Count - first.DecimalPlaces),
                    (digits2.Count - second.DecimalPlaces));
                digits1.InsertRange(0, Enumerable.Repeat(0, maxDigitsBeforeDecimalPoint - (digits1.Count - first.DecimalPlaces)));
                digits2.InsertRange(0, Enumerable.Repeat(0, maxDigitsBeforeDecimalPoint - (digits2.Count - second.DecimalPlaces)));
                int maxDigitsAfterDecimalPoint = Math.Max(
                    (first.DecimalPlaces),
                    (second.DecimalPlaces)
                    );
                digits1.AddRange( Enumerable.Repeat(0, maxDigitsAfterDecimalPoint - first.DecimalPlaces));
                digits2.AddRange( Enumerable.Repeat(0, maxDigitsAfterDecimalPoint - second.DecimalPlaces));
                List<int> resultDigits = new List<int>();
                int carry = 0;               
                for (int i = digits1.Count - 1; i >= 0; i--)
                {
                    int digitsSum = digits1[i] + digits2[i] + carry;
                    carry = digitsSum / 10;
                    int digit = digitsSum % 10;
                    resultDigits.Insert(0, digit);
                }

                if (carry != 0)
                {
                    resultDigits.Insert(0, carry);
                }
                return new LargeNumbers(NumSign.Positive, resultDigits, maxDigitsAfterDecimalPoint);
            }


        }

        public static LargeNumbers Sub(LargeNumbers first, LargeNumbers second)
        {
            if (first.Sign == NumSign.Negative && second.Sign == NumSign.Positive)
            {
                LargeNumbers positiveFirst = new LargeNumbers(NumSign.Positive, first.Digits, first.DecimalPlaces);
                LargeNumbers sum = Add(positiveFirst, second);
                return new LargeNumbers(NumSign.Negative, sum.Digits, sum.DecimalPlaces);
            }
            else if (first.Sign == NumSign.Positive && second.Sign == NumSign.Negative)
            {
                LargeNumbers positiveSecond = new LargeNumbers(NumSign.Positive, second.Digits, second.DecimalPlaces);

                return Add(first, positiveSecond);
            }
            else
            {
                List<int> digits1 = first.Digits.ToList();
                List<int> digits2 = second.Digits.ToList();
                List<int> resultDigits = new List<int>();
                NumSign resultSign = new NumSign();
                if ((digits1.Count - first.DecimalPlaces) < (digits2.Count - second.DecimalPlaces))
                {
                    if (first.Sign == NumSign.Positive && second.Sign == NumSign.Positive)
                    {
                        LargeNumbers sum = Sub(second, first);
                        return new LargeNumbers(NumSign.Negative, sum.Digits, sum.DecimalPlaces);
                    }
                    else if (first.Sign == NumSign.Negative && second.Sign == NumSign.Negative)
                    {
                        LargeNumbers sum = Sub(second, first);
                        return new LargeNumbers(NumSign.Positive, sum.Digits, sum.DecimalPlaces);
                    }
                }

                int maxDigitsBeforeDecimalPoint = Math.Max(
                    (digits1.Count-first.DecimalPlaces),
                    (digits2.Count-second.DecimalPlaces));

                digits1.InsertRange(0, Enumerable.Repeat(0, maxDigitsBeforeDecimalPoint - (digits1.Count - first.DecimalPlaces)));
                digits2.InsertRange(0, Enumerable.Repeat(0, maxDigitsBeforeDecimalPoint - (digits2.Count - second.DecimalPlaces)));

                int maxDigitsAfterDecimalPoint = Math.Max(
                    first.DecimalPlaces,
                    second.DecimalPlaces
                    );

                digits1.AddRange(Enumerable.Repeat(0, maxDigitsAfterDecimalPoint- first.DecimalPlaces));
                digits2.AddRange(Enumerable.Repeat(0, maxDigitsAfterDecimalPoint - second.DecimalPlaces));
                int carry = 0;
                if (digits1[0] < digits2[0])
                {
                    if (first.Sign == NumSign.Positive && second.Sign == NumSign.Positive)
                    {
                        LargeNumbers sum = Sub(second, first);
                        return new LargeNumbers(NumSign.Negative, sum.Digits, sum.DecimalPlaces);
                    }
                    else if (first.Sign == NumSign.Negative && second.Sign == NumSign.Negative)
                    {
                        LargeNumbers sum = Sub(second, first);
                        return new LargeNumbers(NumSign.Positive, sum.Digits, sum.DecimalPlaces);
                    }
                }
                else if (digits1[0] == digits2[0])
                {
                    int i = -1;
                    do
                    {
                        ++ i;
                        if (i == digits1.Count - 1 && i == digits2.Count - 1 && digits1[i] == digits2[i])
                        {
                            break;
                        }
                        else if (digits1[i] < digits2[i])
                        {
                            if (first.Sign == NumSign.Positive && second.Sign == NumSign.Positive)
                            {
                                LargeNumbers sum = Sub(second, first);
                                return new LargeNumbers(NumSign.Negative, sum.Digits, sum.DecimalPlaces);
                            }
                            else if (first.Sign == NumSign.Negative && second.Sign == NumSign.Negative)
                            {
                                LargeNumbers sum = Sub(second, first);
                                return new LargeNumbers(NumSign.Positive, sum.Digits, sum.DecimalPlaces);
                            }

                        }   
                    } while (digits1[i] == digits2[i] && i != digits2.Count);
                }
              
                
                for (int i = digits1.Count - 1; i >= 0; i--)
                {
                    if (digits1[i] - digits2[i]- carry < 0)
                    {
                        
                            resultDigits.Insert(0, (10 - digits2[i] + digits1[i] - carry));
                            carry = 1;
                      
                        
                    }
                    else 
                    {
                        resultDigits.Insert(0,((digits1[i] - carry ) - digits2[i]));
                        carry = 0;
                    }
                }
                if (carry == 0)
                {
                    resultSign = first.Sign;
                }
                if (carry ==1)
                {
                    resultSign = second.Sign;
                }
                    return new LargeNumbers(resultSign, resultDigits, maxDigitsAfterDecimalPoint);
            }

            
        }

        public static LargeNumbers Multiply(LargeNumbers first, LargeNumbers second)
        {
            /* 1 2 3 
             * 1 1 
             * 
             * 0 1 2 3
             * 1 2 3 0
             * 
             * 1 3 5 3
             * 
             */
            List<int> digits1 = first.Digits.ToList();
            List<int> digits2 = second.Digits.ToList();
            string stringDigits1 = string.Join("", digits1);
            string stringDigits2 = string.Join("", digits2);


            LargeNumbers l1 = second;
            BigInteger remainder = 0;
 

            BigInteger d1 = BigInteger.Parse(stringDigits1);
            BigInteger d2 = BigInteger.Parse(stringDigits2);

            List<int> sumdigits = new List<int>();
                BigInteger sum = d1 * d2;
            l1 = new LargeNumbers(sum.ToString());
            if ((first.Sign == NumSign.Negative && second.Sign == NumSign.Negative) ||
     (first.Sign == NumSign.Positive && second.Sign == NumSign.Positive))
            {
                l1.Sign = NumSign.Positive;
            }
            else if ((first.Sign == NumSign.Negative && second.Sign == NumSign.Positive)
                || (second.Sign == NumSign.Negative && first.Sign == NumSign.Positive))
            {
                l1.Sign = NumSign.Negative;

            }

            else l1.Sign = NumSign.Positive;
            l1.DecimalPlaces = first.DecimalPlaces + second.DecimalPlaces;
            sumdigits = l1.Digits.ToList();
            if (sumdigits.Count < l1.DecimalPlaces)
            {
               
                sumdigits.InsertRange(0,Enumerable.Repeat(0, l1.DecimalPlaces - sumdigits.Count));
                
            }

            return new LargeNumbers( l1.Sign, sumdigits, l1.DecimalPlaces);
 
        }

        

        public static LargeNumbers Divide(LargeNumbers first, LargeNumbers second)
        {

            string stringDigits1 = string.Join("", first.Digits.ToList());
            string stringDigits2 = string.Join("", second.Digits.ToList());
            LargeNumbers resultDigits ;
            string sumdigits;

            BigInteger dividend = BigInteger.Parse(stringDigits1);
            BigInteger divisor  = BigInteger.Parse(stringDigits2);
            BigInteger quotient , remainQuotient;
            BigInteger remainder = 0;
            if (divisor == 0)
            {
                Console.WriteLine("you cant divide by zero");
                return new LargeNumbers(NumSign.Positive, second.Digits, 0);
               
            }
            quotient = BigInteger.DivRem(dividend, divisor, out remainder);
            sumdigits = quotient.ToString();
            sumdigits += ".";
            for (int i = 0; i < 50; i++)
            {
                if (remainder != 0)
                {
                    dividend = remainder * 10;
                    remainQuotient = BigInteger.DivRem(dividend, divisor, out remainder);
                    sumdigits += remainQuotient.ToString();


                }
                
            }

            resultDigits = new LargeNumbers(sumdigits);
            List<int> d = resultDigits.Digits.ToList();


            if (first.DecimalPlaces > second.DecimalPlaces|| first.DecimalPlaces == second.DecimalPlaces)
                resultDigits.DecimalPlaces = resultDigits.DecimalPlaces + first.DecimalPlaces - second.DecimalPlaces;
            else {

                d.AddRange(Enumerable.Repeat(0, Math.Abs (second.DecimalPlaces - first.DecimalPlaces)));

            }



            if (d.Count < resultDigits.DecimalPlaces)
            {

                d.InsertRange(0, Enumerable.Repeat(0, resultDigits.DecimalPlaces - d.Count));

            }
           // else if (d.Count == resultDigits.DecimalPlaces  && first.DecimalPlaces< second.DecimalPlaces)

            if ((first.Sign == NumSign.Negative && second.Sign == NumSign.Negative) ||
                (first.Sign == NumSign.Positive && second.Sign == NumSign.Positive))
            {
                resultDigits.Sign = NumSign.Positive;
            }
            else if ((first.Sign == NumSign.Negative && second.Sign == NumSign.Positive)
                || (second.Sign == NumSign.Negative && first.Sign == NumSign.Positive))
            {
                resultDigits.Sign = NumSign.Negative;

            }

            else resultDigits.Sign = NumSign.Positive;
            

            return new LargeNumbers(resultDigits.Sign,d,resultDigits.DecimalPlaces );
        }
        public override string ToString()
        {
           
            string a ="";
            for (int i = 0 ; i< Digits.Count ; i++ )
            {
                if (Digits.Count - DecimalPlaces == 0  && i == 0)
                {
                    a += "0";
                }
                if (i == Digits.Count- DecimalPlaces )
                {
                    
                    a += ".";
                }
                a += Digits[i].ToString();



            }

            if (Sign == NumSign.Negative)
            {
                a = "-" + a;
            }
            else
            {
                a = "+" + a;
            }
            return a;

        }
        public static LargeNumbers operator -(LargeNumbers l1, LargeNumbers l2)
        {
            return Sub(l1, l2);
        }
        public static LargeNumbers operator *(LargeNumbers l1, LargeNumbers l2)
        {
            return Multiply(l1, l2);
        }
        public static LargeNumbers operator +(LargeNumbers l1, LargeNumbers l2)
        {
            return Add(l1, l2);
        }
        public static LargeNumbers operator /(LargeNumbers l1, LargeNumbers l2)
        {
            return Divide(l1, l2);
        }
    }
}
