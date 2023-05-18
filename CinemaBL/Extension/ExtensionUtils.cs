using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL.Extension
{
    public static class ExtensionUtils
    {
        public static string ToCheckString(this string? value) 
        {
            if (value is null)
            {
                return string.Empty;
            }
            return value.Trim().ToLowerInvariant(); 
        }

        public static string ToDefault(this string? s)
        { 
            if (s is null)
            {
                return string.Empty;
            }
            return s;
        }

        //public static TEnum ToEnum<TEnum>(this string s)
        //{            
        //    return (TEnum)Enum.Parse(typeof(TEnum), s, true);
        //}

        public static TEnum? ToEnum<TEnum>(this string? s)
        {
            if (s == null)
            {
                return default(TEnum);
            }

            try
            {
                return (TEnum)Enum.Parse(typeof(TEnum), s, true);
            }
            catch
            {
                return default(TEnum);
            }

            //TEnum result;
            //if (Enum.TryParse(typeof(TEnum), s, true, out result))
            //{
            //    return result;
            //}

            //return result;
        }


        public static TEnum? ToEnum<TEnum>(this int? i)
        {
            if (!i.HasValue)
            {
                return default(TEnum);
            }

            return (TEnum)Enum.ToObject(typeof(TEnum), i);
        }

        public static int ToDefault(this int? i)
        { 
            if (i is null)
            {
                return 0;
            }
            return i.Value;
        }


        /*
         Console.WriteLine(DateTime.Now);
        Console.WriteLine(DateTime.Now.Trim(TimeSpan.TicksPerDay));
        Console.WriteLine(DateTime.Now.Trim(TimeSpan.TicksPerHour));
        Console.WriteLine(DateTime.Now.Trim(TimeSpan.TicksPerMillisecond));
        Console.WriteLine(DateTime.Now.Trim(TimeSpan.TicksPerMinute));
        Console.WriteLine(DateTime.Now.Trim(TimeSpan.TicksPerSecond));*/
        public static DateTime Trim(this DateTime date, long roundTicks)
        {
            return new DateTime(date.Ticks - date.Ticks % roundTicks, date.Kind);
        }
    }
}
