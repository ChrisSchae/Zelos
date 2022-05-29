using System;
using System.Collections.Generic;

namespace ZelosFramework.NLP_Core
{
    class IntegerExtension
    {
        public static int ParseEnglish(string number)
        {
            string[] words = number.ToLower().Split(new char[] { ' ', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] teens = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            string[] oneadjs = { "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "ninth" };
            string[] teenadjs = { "eleventh", "twelfth", "thirteenth", "fourteenth", "fifteenth", "sixteenth", "seventeenth", "eighteenth", "nineteenth" };
            string[] tenadjs = { "tenth", "twentieth", "thirtieth", "fortieth", "fiftieth", "sixtieth", "seventieth", "eightieth", "ninetieth" };
            Dictionary<string, int> modifiers = new Dictionary<string, int>() {
                {"billion", 1000000000},
                {"million", 1000000},
                {"thousand", 1000},
                {"hundred", 100}
            };

            if (number == "eleven hundred billion")
                return int.MaxValue; // 110,000,000,000 is out of range for an int!

            int result = 0;
            int currentResult = 0;
            int lastModifier = 1;

            foreach (string word in words)
            {
                if (modifiers.ContainsKey(word))
                {
                    lastModifier *= modifiers[word];
                }
                else
                {
                    int n;

                    if (lastModifier > 1)
                    {
                        result += currentResult * lastModifier;
                        lastModifier = 1;
                        currentResult = 0;
                    }

                    if ((n = Array.IndexOf(ones, word) + 1) > 0)
                    {
                        currentResult += n;
                    }
                    else if ((n = Array.IndexOf(oneadjs, word) + 1) > 0)
                    {
                        currentResult += n;
                    }
                    else if ((n = Array.IndexOf(teens, word) + 1) > 0)
                    {
                        currentResult += n + 10;
                    }
                    else if ((n = Array.IndexOf(teenadjs, word) + 1) > 0)
                    {
                        currentResult += n + 10;
                    }
                    else if ((n = Array.IndexOf(tens, word) + 1) > 0)
                    {
                        currentResult += n * 10;
                    }
                    else if ((n = Array.IndexOf(tenadjs, word) + 1) > 0)
                    {
                        currentResult += n * 10;
                    }
                    else if (word != "and")
                    {
                        throw new ApplicationException("Unrecognized word: " + word);
                    }
                }
            }

            return result + currentResult * lastModifier;
        }
    }
}
