using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectS3.Controllers.MyEngines
{
    public class MyStaticFunction
    {
        public static float MyFloatParse(string value)
        {
            System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            return float.Parse(value, System.Globalization.NumberStyles.Any, ci);
        }
    }
}