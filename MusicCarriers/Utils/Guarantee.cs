using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MusicCarriers.Utils
{
    /// <summary>
    /// The shortest way to check your variables on something common.
    /// </summary>
    public static class Guarantee
    {
        #region Checks

        /// <summary>
        ///     Checking for null condition
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="caller"></param>
        public static void IsArgumentNotNull(object value, string name, [CallerMemberName] string caller = "")
        {
            if (value != null)
                return;

            throw new ArgumentNullException($"Variable - {name}. Method - {caller}");
        }

        /// <summary>
        ///     Checking for null or empty string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="caller"></param>
        public static void IsStringNotNullOrEmpty(string value, string name, [CallerMemberName] string caller = "")
        {
            if (!string.IsNullOrEmpty(value))
                return;

            throw new ArgumentException($"Variable - {name}. Method - {caller}");
        }

        /// <summary>
        ///     Checking for empty or null enumerable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="name"></param>
        /// <param name="caller"></param>
        public static void IsEnumerableNotNullOrEmpty<T>(IEnumerable<T> enumerable, string name,
            [CallerMemberName] string caller = "")
        {
            if (enumerable != null && enumerable.Any())
                return;

            throw new ArgumentException($"Enumerable with name - {name} is null or empty. Method - {caller}");
        }

        #endregion

        #region GreaterThan

        /// <summary>
        ///     Throw an <see cref="ArgumentException" /> if <paramref name="value" /> less or equal
        ///     <paramref name="comparingValue" />
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="comparingValue"></param>
        /// <param name="caller"></param>
        public static void IsGreaterThan(int value, int comparingValue, string name, 
            [CallerMemberName] string caller = "")
        {
            if (value > comparingValue)
                return;

            throw new ArgumentException(
                $"Variable - {name} should be greater than {comparingValue} in the method {caller}.");
        }

        /// <summary>
        ///     Throw an <see cref="ArgumentException" /> if <paramref name="value" /> less or equal
        ///     <paramref name="comparingValue" />
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="comparingValue"></param>
        /// <param name="caller"></param>
        public static void IsGreaterThan(uint value, uint comparingValue, string name, 
            [CallerMemberName] string caller = "")
        {
            if (value > comparingValue)
                return;

            throw new ArgumentException(
                $"Variable - {name} should be greater than {comparingValue} in the method {caller}.");
        }

        #endregion

        #region GreaterOrEqual
        /// <summary>
        ///     Throw an <see cref="ArgumentException" /> if <paramref name="value" /> less or equal
        ///     <paramref name="comparingValue" />
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="comparingValue"></param>
        /// <param name="caller"></param>
        public static void IsGreaterOrEqual(int value, string name, int comparingValue,
            [CallerMemberName] string caller = "")
        {
            if (value >= comparingValue)
                return;

            throw new ArgumentException(
                $"Variable - {name} should be greater than {comparingValue} in the method {caller}.");
        }
        #endregion

        #region LessOrEqual

        /// <summary>
        ///     Throw an <see cref="ArgumentException" /> if <paramref name="value" /> greater than <paramref name="comparingValue" />
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="comparingValue"></param>
        /// <param name="caller"></param>
        public static void IsLessOrEqual(int value,  int comparingValue, string name,
            [CallerMemberName] string caller = "")
        {
            if (value <= comparingValue)
                return;

            throw new ArgumentException(
                $"Variable - {name} should be less or equal {comparingValue} in the method {caller}.");
        }

        /// <summary>
        ///     Throw an <see cref="ArgumentException" /> if <paramref name="value" /> greater than <paramref name="comparingValue" />
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="comparingValue"></param>
        /// <param name="caller"></param>
        public static void IsLessOrEqual(uint value, uint comparingValue, string name, 
            [CallerMemberName] string caller = "")
        {
            if (value <= comparingValue)
                return;

            throw new ArgumentException(
                $"Variable - {name} should be less or equal {comparingValue} in the method {caller}.");
        }

        #endregion

        #region LessThan

        #endregion
    }
}