using System;
using System.Linq;
using System.Text;

public class Solution
{
    public string NumberToWords(int num)
    {
        // Prepare the array of numbers
        int[] numbers = num.ToString().Select(number => Int32.Parse(number.ToString()))
                                      .ToArray();
        int count = numbers.Length;

        // ------------------
        // Cases between 0-99
        // ------------------
        if (count < 3)
        {
            return NumbersBuilder.GetNumber(numbers);
        }

        // --------------------
        // Cases larger than 99
        // --------------------
        var resultBuilder = new ResultBuilder();
        int quantityLevel = 0;

        do
        {
            if (numbers.Length >= 3)
            {
                // Get the current last three digits
                int prePreLastNumber = numbers[^3];
                int preLastNumber = numbers[^2];
                int lastNumber = numbers[^1];

                // Skip the already cached last three digits
                numbers = numbers.SkipLast(3).ToArray();

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

                // Update the large quantity word
                quantityLevel += 1;
            }
            else
            {
                // Convert the number to a word
                resultBuilder.AddQuantity(quantityLevel);
                resultBuilder.AddNumber(NumbersBuilder.GetNumber(numbers));

                // Prepare the while loop to break
                numbers = Array.Empty<int>();
            }

        } while (numbers.Length != 0);

        return resultBuilder.Result.ToString().Trim();
    }

    private sealed class ResultBuilder
    {
        public StringBuilder Result { get; } = new StringBuilder();

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
            this.Result.Insert(0, $" {value}");
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

        private static readonly string[] LargeQuantities = new string[] { "Hundred", "Thousand", "Million", "Billion",
                                                                          "Trillion" };

        public static string GetNumber(int[] numbers)
        {
            int length = numbers.Length;
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

        public static bool NonZeroSeries(params int[] numbers)
        {
            int result = 0;

            for (int index = 0; index < numbers.Length; index++)
            {
                int number = numbers[index];
                result += number;
            }

            return result != 0;
        }
    }
}
