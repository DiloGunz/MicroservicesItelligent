using System.Text;
using System.Text.RegularExpressions;

namespace BlogApp.Shared.Extensions;

public static class StringExtensions
{
    public static bool HasValue(this string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    public static bool IsBase64String(this string? value)
    {
        if (!value.HasValue())
        {
            return false;
        }
        value = value.Trim();
        return value.Length % 4 == 0 && Regex.IsMatch(value, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
    }

    public static string ToBase64String(this string s)
    {
        if (s.IsBase64String())
        {
            return s;
        }

        int length = s.Length;

        int conteo = 1;

        do
        {
            if ((length + conteo) % 4 == 0)
            {
                break;
            }

            conteo++;
        }
        while (conteo <= 3);

        for (int i = 0; i < conteo; i++)
        {
            s = s + "=";
        }

        return s;
    }

    public static string DecodeBase64Url(this string base64Url)
    {
        string padded = base64Url
            .Replace('-', '+')
            .Replace('_', '/');

        switch (padded.Length % 4)
        {
            case 2: padded += "=="; break;
            case 3: padded += "="; break;
            case 0: break;
            default: throw new FormatException("Invalid Base64Url string");
        }

        var byteArray = Convert.FromBase64String(padded);
        return Encoding.UTF8.GetString(byteArray);
    }
}
