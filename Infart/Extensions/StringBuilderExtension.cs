using System;
using System.Text;

namespace Infart.Extensions
{
    public static class StringBuilderExtensions
    {
        private static readonly char[] _numberBuffer = new char[11];

        public static StringBuilder AppendNumber(this StringBuilder sb, Int32 number)
        {
            bool negative = (number < 0);
            if (negative)
                number = -number;

            int i = _numberBuffer.Length;
            do
            {
                _numberBuffer[--i] = (char)('0' + (number % 10));
                number /= 10;
            }
            while (number > 0);

            if (negative)
                _numberBuffer[--i] = '-';

            sb.Append(_numberBuffer, i, _numberBuffer.Length - i);

            return sb;
        }
    }
}