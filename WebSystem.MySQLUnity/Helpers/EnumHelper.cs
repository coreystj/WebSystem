using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSystem.MySQLUnity.Helpers
{
    /// <summary>
    /// This static class contains methods for managing enums.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Parses an enum string to a type result.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="enumString">The string value of the enum.</param>
        /// <returns>Returns the parsed value.</returns>
        public static T ParseEnum<T>(this string enumString)
                where T : struct, IConvertible
        {
            if (TryParse<T>(enumString, out T result))
                return result;
            else
                return default(T);
        }

        /// <summary>
        /// Tries parses an enum string.
        /// </summary>
        /// <typeparam name="T">The type of the enum to be parsed.</typeparam>
        /// <param name="enumString">The raw enum string.</param>
        /// <param name="result">The out result value.</param>
        /// <returns>Returns true if the enum has been parsed successfully.</returns>
        public static bool TryParse<T>(this string enumString, out T result)
                where T : struct, IConvertible
        {
            try
            {
                result = (T)Enum.Parse(typeof(T), enumString);
                return true;
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }

        /// <summary>
        /// Parses an enum string to a type result.
        /// </summary>
        /// <param name="enumString">The string value of the enum.</param>
        /// <param name="propType">The type of the enum to be parsed.</param>
        /// <returns>Returns the object result.</returns>
        public static object ParseEnum(this string enumString, Type propType)
        {
            return Enum.Parse(propType, enumString, true);
        }

        /// <summary>
        /// Retrieves an array of strings from this enum.
        /// </summary>
        /// <typeparam name="T">The enum type to read from.</typeparam>
        /// <returns>Returns the string array of enum types.</returns>
        public static string[] GetStringArray<T>()
                where T : struct, IConvertible
        {
            return Enum.GetNames(typeof(T));
        }

        /// <summary>
        /// Retrieves an array of all enum types.
        /// </summary>
        /// <typeparam name="T">The type of the enum to be retrieved.</typeparam>
        /// <returns>Returns an array of all enum values.</returns>
        public static T[] GetArray<T>()
        where T : struct, IConvertible
        {
            T[] values = Enum.GetValues(typeof(T)) as T[];
            return values;
        }

        /// <summary>
        /// Retrieves an array of all enum ints.
        /// </summary>
        /// <typeparam name="T">The type of the enum to be retrieved.</typeparam>
        /// <returns>Returns an array of all enum ints.</returns>
        public static int[] GetIntArray<T>()
            where T : struct, IConvertible
        {
            int[] result;
            T[] values = GetArray<T>();

            result = new int[values.Length];
            int i = 0;
            foreach (T item in values)
            {
                Enum test = Enum.Parse(typeof(T), item.ToString()) as Enum;
                int x = Convert.ToInt32(test);

                result[i] = x;
                i++;
            }

            return result;
        }
    }
}
