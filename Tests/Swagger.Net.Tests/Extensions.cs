namespace Swagger.Net.Tests
{
    public static class StringExtensions
    {
        public static string Strip(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Replace("\n", "").Replace("\r", "").Replace(" ", "");
        }
    }
}
