using System.Text.RegularExpressions;

namespace TimeSheet.Forms.Util
{
    public static class Validation
    {
        public static bool ValidarHoraValida(string hora)
        {
            var strpattern = @"^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"; //Pattern is Ok

            var regex = new Regex(strpattern);

            return regex.Match(hora).Success;
        }
    }
}
