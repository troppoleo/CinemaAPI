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

            return (TEnum)Enum.Parse(typeof(TEnum), s, true);
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


    }
}
