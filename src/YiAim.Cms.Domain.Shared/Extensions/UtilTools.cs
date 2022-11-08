using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


namespace YiAim.Cms.Extensions
{
    public static class UtilTools
    {
        /// <summary>
        /// The string time format is converted to DateTime
        /// </summary>
        /// <param name="time"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string time, DateTime defaultValue = default)
        {
            if (time.IsNullOrEmpty())
                return defaultValue;

            return DateTime.TryParse(time, out var dateTime) ? dateTime : defaultValue;
        }





        /// <summary>
        /// Check the ip address
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIp(this string ip)
        {
            var regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");

            return regex.IsMatch(ip);
        }

        /// <summary>
        /// Convert <paramref name="dic"/> to query string
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string ToQueryString(this Dictionary<string, string> dic)
        {
            return dic.Select(x => $"{x.Key}={x.Value}").JoinAsString("&");
        }



        /// <summary>
        /// Generate random code
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomCode(this int length)
        {
            int rand;
            char code;
            var randomcode = string.Empty;
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                rand = random.Next();

                if (rand % 3 == 0)
                {
                    code = (char)('A' + (char)(rand % 26));
                }
                else
                {
                    code = (char)('0' + (char)(rand % 10));
                }

                randomcode += code.ToString();
            }
            return randomcode;
        }
    }
}
