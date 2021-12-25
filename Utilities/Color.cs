using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Color
    {

        private static Dictionary<string,string> ColorDict = new Dictionary<string, string>() {
            {"black", "#212121" },
            {"red", "#f32148" },
            {"white", "#f6f6f6" },
            {"blue", "#2196f3" },
        };

        public static string GetColorCode(string NameEn)
        {
            var colorCode = ColorDict.FirstOrDefault(q=> q.Key.ToLower() == NameEn.ToLower()).Value;

            return colorCode;
        }
    }
}
