namespace Husa.Quicklister.Abor.Crosscutting.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public const string CentralTimeZoneId = "Central Standard Time";
        public static DateTime ConvertToCentral(this DateTime date)
        {
            var dateTimeUnspec = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, TimeZoneInfo.FindSystemTimeZoneById(CentralTimeZoneId));
        }
    }
}
