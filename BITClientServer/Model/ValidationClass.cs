using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BITClientServer.Model
{
    public static class ValidationClass
    {
        // If true then stop

        public static bool CheckIfNull(List<String> list)
        {
            foreach (string s in list)
            {
                if (string.IsNullOrEmpty(s))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckPostcode(string p)
        {
            //return p.Length != 4 || !Regex.IsMatch(p, @"^\d{3-4}$");
            return !Regex.IsMatch(p, @"^(((2|8|9)\d{2})|((02|08|09)\d{2})|([1-9]\d{3}))$");
        }

        public static bool CheckPhone(string p)
        {
            //return !Regex.IsMatch(p, @"/^0[0-8]\d{8}$");
            return !Regex.IsMatch(p, @"^(?:\(0[0-8]\)|0[0-8])[ ]?[0-9]{4}[ ]?[0-9]{4}$");
        }

        public static bool CheckEmail(string e)
        {
            return !Regex.IsMatch(e, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public static bool CheckIfNull<T>(List<T> list)
        {
            return list.Count == 0;
        }

        public static bool CheckIfNull(List<int> list)
        {
            foreach (int i in list)
            {
                if (i == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckDate(DateTime date)
        {
            return date < DateTime.Today || date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool CheckNumber(String s)
        {
            return !Regex.IsMatch(s, @"^([\s\d]+)$");
        }
    }
}
