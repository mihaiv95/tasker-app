using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasker_app.ServiceLayer.Utils
{
    public static class GlobalFunctions
    {
        public static DateTime GetCurrentDateTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"));
        }
        public static string ParseDateTime(DateTime dateTime)
        {
            {
                var date = new DateTime().ToString("yyyy-MM-dd HH:mm");
                if ((dateTime.ToString("yyyy-MM-dd HH:mm").Equals(date)))
                {
                    return null;
                }
                return dateTime.ToString("yyyy-MM-dd HH:mmZ");
            }
           
        }
        public static string ParseNullableDateTime(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }else
            {
                DateTime data = (DateTime)dateTime;
                return data.ToString("yyyy-MM-dd HH:mmZ");
            }
        }

        public static DateTime? ParseStringToDateTime(string dateTime)
        {
            if(dateTime == null)
            {
                return null;
            }
            DateTime date = Convert.ToDateTime(dateTime).ToUniversalTime();
            return date;
        }


        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static string GetFileNameHashed(string fileName)
        {
            return Guid.NewGuid().ToString("N") + Path.GetExtension(fileName);
        }

        public static string SeparateStringByCapitals(string stringForEnum)
        {
            string theString = stringForEnum;
            StringBuilder builder = new StringBuilder();
            foreach (char c in theString)
            {
                if (Char.IsUpper(c) && builder.Length > 0) builder.Append(' ');
                builder.Append(c);
            }
            theString = builder.ToString();
            return theString;
        }
    }
}
