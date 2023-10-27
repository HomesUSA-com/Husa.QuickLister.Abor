namespace Husa.Quicklister.Abor.Crosscutting.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public const string CentralTimeZoneId = "Central Standard Time";
        public static DateTime ConvertToCentral(this DateTime date)
        {
            var dateTimeUnspecified = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspecified, TimeZoneInfo.FindSystemTimeZoneById(CentralTimeZoneId));
        }

        public static DateTime ToUtcDateTime(this DateTimeOffset? date)
        {
            if (date == null)
            {
                return DateTime.MinValue;
            }

            return ((DateTimeOffset)date).UtcDateTime;
        }
    }
}
