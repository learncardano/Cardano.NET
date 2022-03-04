using System;
using System.Globalization;

namespace Cardano.NET
{
    public partial class WalletAPI
    {
        public partial class Tools
        {
            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Converts a whole number in Lovelace into a decimal value in ADA (1 Lovelace = 0.000001 ADA).
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Decimal ToADA(Int32 amount)
            {
                // 1000000 Lovelace = 1 ADA, 1 Lovelace = 0.000001 ADA
                return (Decimal)amount / 1000000.00m;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Converts a string to integer.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Int32 ToInteger(String value)
            {
                try
                { return Convert.ToInt32(value); }
                catch
                { return 0; }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Converts a decimal ADA value into a whole number in Lovelace (1 ADA = 1000000 Lovelace).
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Int32 ToLovelace(Decimal amount)
            { 
                // 1000000 Lovelace = 1 ADA, 1 Lovelace = 0.000001 ADA
                String value = amount.ToString("N6");   // Make sure there are 6 decimal places
                value = value.Replace(".", "");         // Remove dot -> 1.123456 ADA = 1123456 Lovelace

                return ToInteger(value);                // Convert to integer
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Formats the date into ISO 8601 date-and-time format (yyyy-MM-ddThh:mm:ssZ).
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public String ToDateISO8601(DateTime date)
            {
                return date.ToString("yyyy-MM-dd") + "T" + date.ToString("hh:mm:ss") + "Z";
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Counts the number of words in a given sentence.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Int32 WordCount(String sentence)
            {
                Int32 count = 0;
                try
                {
                    if (sentence.Equals(""))
                        return count;
                    else
                        return sentence.Split(' ').Length;
                }
                catch
                { return count; }
            }
        }
    }
}