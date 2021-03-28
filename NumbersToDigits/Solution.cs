using System;
using System.Collections.Generic;
using System.Linq;

public class Solution
{
    private const int Three = 3;  // NOTE: "The Rule of Three"

    public string NumberToWords(int num)
    {
        // Prepare the array of numbers
        int[] numbers = num.ToString().Select(number => Int32.Parse(number.ToString()))
                                      .ToArray();
        int length = numbers.Length;

        // ------------------
        // Cases between 0-99
        // ------------------
        if (length < 3)
        {
            return NumbersBuilder.GetNumber(numbers, length);
        }

        // --------------------
        // Cases larger than 99
        // --------------------
        var resultBuilder = new ResultBuilder();
        int quantityLevel = 0;
        int tripletsCounter = 0;

        do
        {
            if (length >= Three)  // NOTE: First iteration always true
            {
                // Get the current last three digits
                int prePreLastNumber = numbers[^(3 + tripletsCounter)];
                int preLastNumber = numbers[^(2 + tripletsCounter)];
                int lastNumber = numbers[^(1 + tripletsCounter)];

                // Update do-while loop condition
                length -= Three;

                // Convert the number to a word
                if (quantityLevel > 0 && NumbersBuilder.NonZeroSeries(prePreLastNumber, preLastNumber, lastNumber))
                {
                    resultBuilder.AddQuantity(quantityLevel);
                }
                resultBuilder.AddNumber(NumbersBuilder.GetTwoDigitNumber(preLastNumber, lastNumber));

                if (prePreLastNumber != 0)
                {
                    resultBuilder.AddQuantity(quantityLevel: 0);  // NOTE: Always add "Hundred"
                    resultBuilder.AddNumber(NumbersBuilder.GetSingleDigitNumber(prePreLastNumber));
                }

                // Update the number of parsed triplets
                tripletsCounter += Three;

                // Update the large quantity word
                quantityLevel += 1;
            }
            else
            {
                // Convert the number to a word
                resultBuilder.AddQuantity(quantityLevel);
                resultBuilder.AddNumber(NumbersBuilder.GetNumber(numbers, length));

                // Prepare the loop to break
                length = 0;
            }

        } while (length > 0);

        return resultBuilder.FinalResult;
    }

    private sealed class ResultBuilder
    {
        private readonly LinkedList<string> _words = new LinkedList<string>();

        private string _tempResult;
        public string FinalResult
        {
            get
            {
                foreach (string word in _words)
                {
                    _tempResult += word;
                }

                return _tempResult.Trim();
            }
        }

        public void AddQuantity(int quantityLevel)
        {
            string quantity = NumbersBuilder.GetQuantity(quantityLevel);

            Prepend(quantity);
        }

        public void AddNumber(string number)
        {
            if (!String.IsNullOrEmpty(number))
            {
                Prepend(number);
            }
        }

        private void Prepend(object value)
        {
            this._words.AddFirst(" ");
            this._words.AddFirst(value.ToString());
        }
    }

    private static class NumbersBuilder
    {
        private const string Unknown = "???";
        private const string Empty = "";

        private static readonly string[] SingleNumbers = new string[10] { "Zero", "One", "Two", "Three", "Four",
                                                                          "Five", "Six", "Seven", "Eight", "Nine" };

        private static readonly string[] TeenNumbers = new string[10] { "Ten", "Eleven", "Twelve", "Thirteen",
                                                                        "Fourteen", "Fifteen", "Sixteen",
                                                                        "Seventeen", "Eighteen", "Nineteen" };

        private static readonly string[] DozenNumbers = new string[8] { "Twenty", "Thirty", "Forty", "Fifty",
                                                                        "Sixty", "Seventy", "Eighty", "Ninety" };  // [index - 2]

        private static readonly string[] LargeQuantities = new string[] { "Hundred", "Thousand", "Million", "Billion" };

        public static string GetNumber(int[] numbers, int length)
        {
            string number = Unknown;

            // -----------------
            // Cases between 0-9
            // -----------------
            if (length == 1)
            {
                number = GetSingleDigitNumber(numbers[0]);
            }

            // -------------------
            // Cases between 10-99
            // -------------------
            if (length == 2)
            {
                int firstNumber = numbers[length - 2];
                int secondNumber = numbers[length - 1];

                number = GetTwoDigitNumber(firstNumber, secondNumber);
            }

            return number;
        }

        public static string GetSingleDigitNumber(int number)
        {
            return SingleNumbers[number];
        }

        public static string GetTwoDigitNumber(int firstNumber, int secondNumber)
        {
            if (firstNumber == 0)
            {
                return secondNumber == 0
                    ? Empty
                    : SingleNumbers[secondNumber];
            }
            else if (IsPlural(firstNumber))
            {
                string dozen = DozenNumbers[firstNumber - 2];
                string single = secondNumber == 0  // Ignore "0"
                    ? Empty
                    : $" {SingleNumbers[secondNumber]}";

                return dozen + single;
            }
            else
            {
                return TeenNumbers[secondNumber];
            }
        }

        private static bool IsPlural(int number)
        {
            return number > 1;
        }

        public static string GetQuantity(int quantityLevel)
        {
            return quantityLevel < LargeQuantities.Length ? LargeQuantities[quantityLevel]
                                                          : Unknown;
        }

        public static bool NonZeroSeries(int number1, int number2, int number3)
        {
            return number1 + number2 + number3 != 0;
        }
    }
}
