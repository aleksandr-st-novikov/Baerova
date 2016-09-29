using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebUI.Helpers
{
    public class Texts
    {
        public static string Translit(string str)
        {
            string[] lat_up = { "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh", "Ts", "Ch", "Sh", "Shch", "", "Y", "", "E", "Yu", "Ya" };
            string[] lat_low = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "", "y", "", "e", "yu", "ya" };
            string[] rus_up = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
            string[] rus_low = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };
            for (int i = 0; i <= 32; i++)
            {
                str = str.Replace(rus_up[i], lat_up[i]);
                str = str.Replace(rus_low[i], lat_low[i]);
            }
            return str;
        }

        public static string ToFriendlyCase(string EnumString)
        {
            //return Regex.Replace(EnumString, "(?!^)([A-Z])", " $1");
            return Regex.Replace(EnumString, "(?!^)([А-Я])", " $1");
        }

        /// <summary>
        /// Converts a datetime value to w3c format
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDateToW3CTime(DateTime date)
        {
            //Get the utc offset from the date value
            var utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(date);
            string w3CTime = date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");
            //append the offset e.g. z=0, add 1 hour is +01:00
            w3CTime += utcOffset == TimeSpan.Zero ? "Z" :
                String.Format("{0}{1:00}:{2:00}", (utcOffset > TimeSpan.Zero ? "+" : "-")
                , utcOffset.Hours, utcOffset.Minutes);

            return w3CTime;
        }
    }

}