using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantComputerControl {
    class WordsToNumbers {
        public int EnglishGet(string words) {
            //Thanks to https://www.programmingalgorithms.com/algorithm/words-to-numbers/

            if (string.IsNullOrEmpty(words)) return -1;

            words = words.Trim();
            words += ' ';

            int number = -1;
            string[] singles = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] teens = new string[] { "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = new string[] { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninty" };
            string[] powers = new string[] { "", "thousand", "million", "billion", "trillion", "quadrillion", "quintillion" };

            for (int i = powers.Length - 1; i >= 0; i--) {
                if (!string.IsNullOrEmpty(powers[i])) {
                    int index = words.IndexOf(powers[i]);

                    if (index >= 0 && words[index + powers[i].Length] == ' ') {
                        int count = EnglishGet(words.Substring(0, index));
                        number += count * (int)Math.Pow(1000, i);
                        words = words.Remove(0, index);
                    }
                }
            }

            {
                int index = words.IndexOf("hundred");

                if (index >= 0 && words[index + "hundred".Length] == ' ') {
                    int count = EnglishGet(words.Substring(0, index));
                    number += count * 100;
                    words = words.Remove(0, index);
                }
            }

            for (int i = tens.Length - 1; i >= 0; i--) {
                if (!string.IsNullOrEmpty(tens[i])) {
                    int index = words.IndexOf(tens[i]);

                    if (index >= 0 && words[index + tens[i].Length] == ' ') {
                        number += (int)(i * 10);
                        words = words.Remove(0, index);
                    }
                }
            }

            for (int i = teens.Length - 1; i >= 0; i--) {
                if (!string.IsNullOrEmpty(teens[i])) {
                    int index = words.IndexOf(teens[i]);

                    if (index >= 0 && words[index + teens[i].Length] == ' ') {
                        number += (int)(i + 10);
                        words = words.Remove(0, index);
                    }
                }
            }

            for (int i = singles.Length - 1; i >= 0; i--) {
                if (!string.IsNullOrEmpty(singles[i])) {
                    int index = words.IndexOf(singles[i] + ' ');

                    if (index >= 0 && words[index + singles[i].Length] == ' ') {
                        number += (int)(i);
                        words = words.Remove(0, index);
                    }
                }
            }

            return number;
        }

    }
}
