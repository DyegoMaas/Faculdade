using System;

namespace JogoCartas21.Core.Utils
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string valorString)
        {
            return (T)Enum.Parse(typeof(T), valorString);
        }

        public static string FormatWith(this string @string, params object[] parametros)
        {
            return string.Format(@string, parametros);
        }
    }
}