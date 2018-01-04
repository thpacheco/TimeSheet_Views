namespace Extension
{
    public static class Extension
    {
        public static string FormatarDataEnvio(this string data)
        {
            return data.Replace("/", "-");
        }
    }
}
