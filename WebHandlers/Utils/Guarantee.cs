using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WebHandlers.Utils
{
    public static class Guarantee
    {
        /// <summary>
        /// Checking for null condition
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public static void IsArgumentNotNull(object value, string name, [CallerMemberName]string caller = "")
        {
            if (value != null)
                return;

            throw new ArgumentNullException($"Variable - {name}. Method - {caller}");
        }

        /// <summary>
        /// Checking for null or empty string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public static void IsStringNotNullOrEmpty(string value, string name, [CallerMemberName] string caller = "")
        {
            if (!string.IsNullOrEmpty(value))
                return;

            throw new ArgumentException($"Variable - {name}. Method - {caller}");
        }

        /// <summary>
        /// Checking for empty or null enumerable
        /// </summary>
        /// <typeparam name="T">Whatever you want :)</typeparam>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public static void IsEnumerableNotNullOrEmpty<T>(IEnumerable<T> value, string name, [CallerMemberName] string caller = "")
        {
            if (value != null && value.Any())
                return;

            throw new ArgumentException($"Enumerable with name - {name} is null or empty. Method - {caller}");
        }
    }
}
