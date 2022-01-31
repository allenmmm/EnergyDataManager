using System;

namespace SharedKernel
{
    public class Guard
    {
        public static T AgainstNull<T>(T value, string message)  where T : class
        {
            if (value == null)
                throw new ArgumentNullException($"ERROR : {message} (parameter is null)");
            return value;
        }

        public static void AgainstNotEqual<T>(T value1, T value2, string message)
        {
            if (!value1.Equals(value2))
                throw new ArgumentException($"ERROR : {message} (parameter error {value1})");
        }

        public static T AgainstGreaterThan<T>(T upperLimit, T value, string message) where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) > 0)
                throw new ArgumentOutOfRangeException(
                    $"ERROR : {message} (parameter error {value})");
            return value;
        }

        public static T AgainstLessThan<T>(T upperLimit, T value, string message) where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) < 0)
                throw new ArgumentOutOfRangeException(
                    $"ERROR : {message} (parameter error {value})");
            return value;
        }

        public static void AgainstFalse(bool value, string message)
        {
            if (!value)
                throw new ArgumentException(
                      $"ERROR : {message} (parameter error {value})");
        }

        public static DateTime AgainstInvalidDateTime(string value, string message)
        {
            var dateTimeArray = value.Split("  ");
            var dateArray = dateTimeArray[0].Split("/");
            var timeArray = dateTimeArray[1].Split(":");
            try
            {
                return (new DateTime(
                    int.Parse(dateArray[2]),
                    int.Parse(dateArray[1]),
                    int.Parse(dateArray[0]),
                    int.Parse(timeArray[0]),
                    int.Parse(timeArray[1]),
                    int.Parse(timeArray[2])));

            }
            catch(Exception ex)
            {
                throw new FormatException($"ERROR : {message} (parameter error {value})");
            }
        }

    }
}
