using System;
using System.Text;

namespace fge
{
    public static class StringBuilderExtensions
    {
        
        static char[] numberBuffer = new char[11];

        
        public static StringBuilder AppendNumber(this StringBuilder sb, Int32 number)
        {
            bool negative = (number < 0);
            if (negative)
                number = -number;

            int i = numberBuffer.Length;
            do
            {
                numberBuffer[--i] = (char)('0' + (number % 10));
                number /= 10;
            }
            while (number > 0);

            if (negative)
                numberBuffer[--i] = '-';

            sb.Append(numberBuffer, i, numberBuffer.Length - i);

            return sb;
        }
    }
}
