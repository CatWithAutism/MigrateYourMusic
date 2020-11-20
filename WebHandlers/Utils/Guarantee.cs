using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WebHandlers.Utils
{
    public static class Guarantee
    {
        #region Checks
        /// <summary>
        /// Checking for null condition
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public static void IsArgumentNotNull(object value, string name, [CallerMemberName] string caller = "")
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
        /// <param name="enumerable"></param>
        /// <param name="name"></param>
        public static void IsEnumerableNotNullOrEmpty(IEnumerable<object> enumerable, string name, [CallerMemberName] string caller = "")
        {
            if (enumerable != null && enumerable.Any())
                return;

            throw new ArgumentException($"Enumerable with name - {name} is null or empty. Method - {caller}");
        }
        #endregion

        #region GreaterThan

        /// <summary>
        /// Throw an <see cref="ArgumentException"/> if <paramref name="value"/> less or equal <paramref name="compareValue"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="compareValue"></param>
        /// <param name="caller"></param>
        public static void IsGreaterThan(int value, string name, int compareValue, [CallerMemberName] string caller = "")
        {
            if (value > compareValue)
                return;

            throw new ArgumentException($"Variable - {name} should be greater than {compareValue} in the method {caller}.");
        }

        /// <summary>
        /// Throw an <see cref="ArgumentException"/> if <paramref name="value"/> less or equal <paramref name="compareValue"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="compareValue"></param>
        /// <param name="caller"></param>
        public static void IsGreaterThan(uint value, string name, uint compareValue, [CallerMemberName] string caller = "")
        {
            if (value > compareValue)
                return;

            throw new ArgumentException($"Variable - {name} should be greater than {compareValue} in the method {caller}.");
        }

        #endregion

        #region GreaterOrEqual

        #endregion

        #region LessOrEqual
        /// <summary>
        /// Throw an <see cref="ArgumentException"/> if <paramref name="value"/> greate than <paramref name="compareValue"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="compareValue"></param>
        /// <param name="caller"></param>
        public static void IsLessOrEqual(int value, string name, int compareValue, [CallerMemberName] string caller = "")
        {
            if (value <= compareValue)
                return;

            throw new ArgumentException($"Variable - {name} should be less or equal {compareValue} in the method {caller}.");
        }

        /// <summary>
        /// Throw an <see cref="ArgumentException"/> if <paramref name="value"/> greate than <paramref name="compareValue"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="compareValue"></param>
        /// <param name="caller"></param>
        public static void IsLessOrEqual(uint value, string name, uint compareValue, [CallerMemberName] string caller = "")
        {
            if (value <= compareValue)
                return;

            throw new ArgumentException($"Variable - {name} should be less or equal {compareValue} in the method {caller}.");
        }
        #endregion

        #region LessThan

        #endregion
    }
}
