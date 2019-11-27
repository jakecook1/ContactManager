using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactManagerWeb.Helpers
{
    public static class DateTimeExtensions
    {
        private const string DefaultZone = "New Zealand Standard Time";

        public static DateTime UtcToTimeZoneTime(this DateTime time, string timeZoneId = DefaultZone)
        {
            var timezone = GetTimeZoneId(timeZoneId);
            return time.UtcToTimeZoneTime(timezone);
        }

        public static DateTime TimeZoneTimeToUtc(this DateTime time, string timeZoneId = DefaultZone)
        {
            var timezone = GetTimeZoneId(timeZoneId);
            return time.TimeZoneTimeToUtc(timezone);
        }

        public static DateTime UtcToTimeZoneTime(this DateTime time, TimeZoneInfo timezone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(time, timezone);
        }

        public static DateTime TimeZoneTimeToUtc(this DateTime time, TimeZoneInfo timezone)
        {
            var dateTimeUnspec = DateTime.SpecifyKind(time, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, timezone);
        }
    
        public static void UtcToLocalDates<T>(this IEnumerable<T> items, string[] columns, string timeZoneId = DefaultZone)
        {
            // Get timezone before loop starts as this doesn't need to be declare each loop
            var timezone = GetTimeZoneId(timeZoneId);

            // Will need to figure out better way to do this.
            foreach (var item in items)
            {
                ChangeUtcToLocalDates(columns, timezone, item);
            }
        }

        public static void UtcToLocalDates<T>(this T item, string[] columns, string timeZoneId = DefaultZone)
        {
            // Get timezone before loop starts as this doesn't need to be declare each loop
            var timezone = GetTimeZoneId(timeZoneId);
            ChangeUtcToLocalDates(columns, timezone, item);
        }

        private static void ChangeUtcToLocalDates<T>(string[] columns, TimeZoneInfo timezone, T item)
        {
            foreach (var column in columns)
            {
                var propertyInfo = item.GetType().GetProperty(column);
                var value = propertyInfo.GetValue(item, null) as DateTime?;

                if (value.HasValue)
                    propertyInfo.SetValue(item, value.Value.UtcToTimeZoneTime(timezone), null);
            }
        }
    

        public static void LocalToUtcDates<T>(this IEnumerable<T> items, string[] columns, string timeZoneId = DefaultZone)
        {
            // Get timezone before loop starts as this doesn't need to be declare each loop
            var timezone = GetTimeZoneId(timeZoneId);

            // Will need to figure out better way to do this.
            foreach (var item in items)
            {
                ChangeLocalToUtcDates(columns, timezone, item);
            }
        }

        public static void LocalToUtcDates<T>(this T item, string[] columns, string timeZoneId = DefaultZone)
        {
            // Get timezone before loop starts as this doesn't need to be declare each loop
            var timezone = GetTimeZoneId(timeZoneId);
            ChangeLocalToUtcDates(columns, timezone, item);
        }

        private static void ChangeLocalToUtcDates<T>(string[] columns, TimeZoneInfo timezone, T item)
        {
            foreach (var column in columns)
            {
                var propertyInfo = item.GetType().GetProperty(column);
                var value = propertyInfo.GetValue(item, null) as DateTime?;

                if (value.HasValue)
                    propertyInfo.SetValue(item, value.Value.TimeZoneTimeToUtc(timezone), null);
            }
        }

        private static TimeZoneInfo GetTimeZoneId(string timeZoneId)
        {
            var timezones = TimeZoneInfo.GetSystemTimeZones();
            return timezones.FirstOrDefault(x => x.StandardName == timeZoneId);
        }
    }
}