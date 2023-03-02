using System.Text.RegularExpressions;

namespace BHSNetCoreLib.EmailUtil
{
    public static class Util
    {
        public static bool ValidateEmailFormat(string email)
        {
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(email.Trim()))
            {
                return regex.IsMatch(email);
            }
            else
            {
                return false;
            }
        }
    }
}
