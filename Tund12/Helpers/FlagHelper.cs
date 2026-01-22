namespace Tund12.Helpers
{
    public static class FlagHelper
    {
        private static readonly Dictionary<string, string> LanguageFlags = new()
        {
            { "Inglise", "🇬🇧" },
            { "English", "🇬🇧" },
            { "Saksa", "🇩🇪" },
            { "German", "🇩🇪" },
            { "Prantsuse", "🇫🇷" },
            { "French", "🇫🇷" },
            { "Hispaania", "🇪🇸" },
            { "Spanish", "🇪🇸" },
            { "Itaalia", "🇮🇹" },
            { "Italian", "🇮🇹" },
            { "Vene", "🇷🇺" },
            { "Russian", "🇷🇺" },
            { "Jaapani", "🇯🇵" },
            { "Japanese", "🇯🇵" },
            { "Hiina", "🇨🇳" },
            { "Chinese", "🇨🇳" },
            { "Portugal", "🇵🇹" },
            { "Portuguese", "🇵🇹" },
            { "Rootsi", "🇸🇪" },
            { "Swedish", "🇸🇪" },
            { "Norra", "🇳🇴" },
            { "Norwegian", "🇳🇴" },
            { "Taani", "🇩🇰" },
            { "Danish", "🇩🇰" },
        };

        public static string GetFlag(string language)
        {
            if (string.IsNullOrEmpty(language))
                return "🌐";

            return LanguageFlags.TryGetValue(language, out var flag)
                ? flag
                : "🌐";
        }

        public static string GetFlagImage(string language)
        {
            // SVG failid kodeerida base64-na
            return language switch
            {
                "Inglise" or "English" => GetSvgFlag("gb"),
                "Saksa" or "German" => GetSvgFlag("de"),
                "Prantsuse" or "French" => GetSvgFlag("fr"),
                "Hispaania" or "Spanish" => GetSvgFlag("es"),
                "Itaalia" or "Italian" => GetSvgFlag("it"),
                "Vene" or "Russian" => GetSvgFlag("ru"),
                "Jaapani" or "Japanese" => GetSvgFlag("jp"),
                "Hiina" or "Chinese" => GetSvgFlag("cn"),
                "Portugal" or "Portuguese" => GetSvgFlag("pt"),
                "Rootsi" or "Swedish" => GetSvgFlag("se"),
                "Norra" or "Norwegian" => GetSvgFlag("no"),
                "Taani" or "Danish" => GetSvgFlag("dk"),
                _ => GetSvgFlag("world")
            };
        }

        private static string GetSvgFlag(string code)
        {
            // Kasuta Bootstrap Icons või ikoone CDN-st
            return code switch
            {
                "gb" => "<i class=\"bi bi-flag-gb\"></i>",
                "de" => "<i class=\"bi bi-flag-de\"></i>",
                "fr" => "<i class=\"bi bi-flag-fr\"></i>",
                "es" => "<i class=\"bi bi-flag-es\"></i>",
                "it" => "<i class=\"bi bi-flag-it\"></i>",
                "ru" => "<i class=\"bi bi-flag-ru\"></i>",
                "jp" => "<i class=\"bi bi-flag-jp\"></i>",
                "cn" => "<i class=\"bi bi-flag-cn\"></i>",
                "pt" => "<i class=\"bi bi-flag-pt\"></i>",
                "se" => "<i class=\"bi bi-flag-se\"></i>",
                "no" => "<i class=\"bi bi-flag-no\"></i>",
                "dk" => "<i class=\"bi bi-flag-dk\"></i>",
                _ => "<i class=\"bi bi-globe\"></i>"
            };
        }
    }
}