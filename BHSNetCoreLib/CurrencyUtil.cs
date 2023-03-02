
namespace BHSNetCoreLib
{
    public class CurrencyUtil
    {
        public static string convertCurrencyVND(double chuoi)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            return chuoi.ToString("#,###", info.NumberFormat);
        }
        public static string convertCurrencyUSD(double chuoi)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            return chuoi.ToString("#,###.##", info.NumberFormat);
        }
    }
}
