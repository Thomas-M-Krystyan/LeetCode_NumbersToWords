using NUnit.Framework;

namespace NumbersToDigits.Tests
{
    [TestFixture]
    public class SolutionTests
    {
        [TestCase(0, "Zero")]
        [TestCase(6, "Six")]
        [TestCase(10, "Ten")]
        [TestCase(11, "Eleven")]
        [TestCase(15, "Fifteen")]
        [TestCase(20, "Twenty")]
        [TestCase(37, "Thirty Seven")]
        [TestCase(100, "One Hundred")]
        [TestCase(101, "One Hundred One")]
        [TestCase(123, "One Hundred Twenty Three")]
        [TestCase(1000, "One Thousand")]
        [TestCase(1001, "One Thousand One")]
        [TestCase(10000, "Ten Thousand")]
        [TestCase(12345, "Twelve Thousand Three Hundred Forty Five")]
        [TestCase(100000, "One Hundred Thousand")]
        [TestCase(100001, "One Hundred Thousand One")]
        [TestCase(123123, "One Hundred Twenty Three Thousand One Hundred Twenty Three")]
        [TestCase(1000000, "One Million")]
        [TestCase(1000001, "One Million One")]
        [TestCase(1000500, "One Million Five Hundred")]
        [TestCase(1234567, "One Million Two Hundred Thirty Four Thousand Five Hundred Sixty Seven")]
        [TestCase(12345678, "Twelve Million Three Hundred Forty Five Thousand Six Hundred Seventy Eight")]
        [TestCase(1000000001, "One Billion One")]
        [TestCase(1234567891, "One Billion Two Hundred Thirty Four Million Five Hundred Sixty Seven Thousand Eight Hundred Ninety One")]
        public void CheckIf_123_ReturnsProperString(int number, string stringResult)
        {
            // Arrange
            var solution = new Solution();

            // Act
            var result = solution.NumberToWords(number);

            // Assert
            Assert.That(result, Is.EqualTo(stringResult));
        }
    }
}
