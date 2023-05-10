using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalmartCalculater.StringHelper
{
    public class StringHelper
    {
        public static string ExtractValue(string inputString)
        {
            // Define the characters to remove
            char[] charsToRemove = {'$', ' ','\r','\n'};

            // Remove the characters using LINQ
            string resultString = new string(inputString.Where(c => !charsToRemove.Contains(c)).ToArray());

            return resultString.Replace("&nbsp;","");
        }

        public static string FormateTitle(string inputString)
        {
            // Define the characters to remove
            char[] charsToRemove = { '\r', '\n' };
            // Remove the characters using LINQ
            string resultString = new string(inputString.Where(c => !charsToRemove.Contains(c)).ToArray());
            // Split the string into words and remove any empty entries
            string[] words = resultString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Join the words back together with a single space between them
            string output = string.Join(" ", words);

            output = output.Replace("&nbsp;", "");

            return output;
        }
    }
}
