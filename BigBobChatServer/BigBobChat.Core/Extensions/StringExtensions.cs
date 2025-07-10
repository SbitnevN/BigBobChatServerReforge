using System.Text;

namespace BigBobChat.Core.Extensions;

public static class StringExtensions
{
    public static bool IsEmpty(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static bool IsNotEmpty(this string value)
    {
        return !value.IsEmpty();
    }

    public static string ToSnakeCase(this string input)
    {
        if (input.IsEmpty())
            return input;

        StringBuilder sb = new StringBuilder(input.Length * 2);
        sb.Append(char.ToLower(input[0]));

        for (int i = 1; i < input.Length; i++)
        {
            char current = input[i];
            char previous = input[i - 1];
            char next = (i < input.Length - 1) ? input[i + 1] : '\0';

            if (char.IsUpper(current) && char.IsLower(previous))
            {
                sb.Append('_');
            }
            else if (char.IsDigit(previous) && char.IsLetter(current) ||
                     char.IsLetter(previous) && char.IsDigit(current))
            {
                sb.Append('_');
            }
            else if (char.IsUpper(previous) &&
                     char.IsUpper(current) &&
                     next != '\0' &&
                     char.IsLower(next))
            {
                sb.Append('_');
            }

            sb.Append(char.IsUpper(current) ? char.ToLower(current) : current);
        }

        return sb.ToString();
    }
}