namespace Highscores.Website.Extensions
{
    public static class StringExtensions
    {
        public static string Slugify(this string name) => 
            name.Trim()
                .Replace("-", "")
                .Replace(" ", "-")
                .ToLower();
    }
}
