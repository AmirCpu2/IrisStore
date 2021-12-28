using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class PersianDateUtils
    {
        public static string ToPersianDate(this DateTime date)
        {
            var dateTime = new DateTime(date.Year, date.Month, date.Day, new GregorianCalendar());
            var persianCalendar = new PersianCalendar();
            return
                $"{persianCalendar.GetYear(dateTime)}/{persianCalendar.GetMonth(dateTime).ToString("00")}/{persianCalendar.GetDayOfMonth(dateTime).ToString("00")}";
        }

        public static string ToPersianDateFa(this DateTime date)
        {
            var dateTime = new DateTime(date.Year, date.Month, date.Day, new GregorianCalendar());
            var persianCalendar = new PersianCalendar();

            return
                $"{persianCalendar.GetDayOfMonth(dateTime)} {PersianDate.ConvertDate.MapFarsiMonthNumToName(persianCalendar.GetMonth(dateTime))} ";

        }

        public static string StartDateFaFull(this DateTime date)
        {
            var dateTime = new DateTime(date.Year, date.Month, date.Day, new GregorianCalendar());
            var persianCalendar = new PersianCalendar();

            return
                $"{persianCalendar.GetDayOfMonth(dateTime)} {PersianDate.ConvertDate.MapFarsiMonthNumToName(persianCalendar.GetMonth(dateTime))} {persianCalendar.GetYear(dateTime)} - {date.Hour.ToString("00")}:{date.Minute.ToString("00")}:{date.Second.ToString("00")}";

        }
        public static string PersianNumberToEn(string input)
        {
            if (input == null || input.Trim() == "") return null;

            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            input = input.Replace( "۰", "0");
            input = input.Replace( "۱", "1");
            input = input.Replace("۲", "2");
            input = input.Replace("۳", "3");
            input = input.Replace("۴", "4");
            input = input.Replace("۵", "5");
            input = input.Replace("۶", "6");
            input = input.Replace("۷", "7");
            input = input.Replace("۸", "8");
            input = input.Replace("۹", "9");
            return input;
        }
        public static DateTime? ConvertPersianToGregorianDate(string persianDateTime)
        {
            try
            {

                if (persianDateTime == null)
                    return null;

                persianDateTime = PersianNumberToEn(persianDateTime);

                var textSplited = persianDateTime.Split('/', ' ', '-', ':');
                if (textSplited.Length < 3)
                    return null;
                int Year = int.Parse(textSplited[0]);
                int Month = int.Parse(textSplited[1]);
                int Day = int.Parse(textSplited[2]);
                int h = int.Parse(textSplited[5]);
                int m = int.Parse(textSplited[6]);
                int s = int.Parse(textSplited[7]);

                if (Year > 1900 || Day > 1900)
                    return Convert.ToDateTime(persianDateTime, new CultureInfo("en"));

                DateTime result = new DateTime();

                if (Year > 1000)
                    result = Persia.Calendar.ConvertToGregorian(Year, Month, Day, Persia.DateType.Persian);
                else if (Year <= 31)
                    //change day and year
                    result = Persia.Calendar.ConvertToGregorian(Year, Month, Day, Persia.DateType.Persian);
                else if (Year < 1000)
                    return null;
                
                var ampm = h > 12 ? "PM" : "AM";

                var v = $"{result.Month}/{result.Day}/{result.Year} {(h > 12 ? (h - 12) : h)}:{m}:{s} {ampm}";


                var c = DateTime.Parse(v);


                return c;
            }
            catch (Exception)
            {
                return null;
            }



        }

    }
}
