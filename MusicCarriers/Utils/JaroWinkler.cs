﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicCarriers.Utils
{
    /// <summary>
    /// Copy-paste algorithm to check similarity of strings. 
    /// </summary>
    public class JaroWinkler : IComparer<string>
    {
        private const double DefaultThreshold = 0.7;
        private const int Three = 3;
        private const double JwCoef = 0.1d;
        private const int MaxSimilarityPercents = 100;


        int IComparer<string>.Compare(string str1, string str2)
        {
            return Convert.ToInt32(Similarity(str1, str2) * MaxSimilarityPercents);
        }

        /// <summary>
        ///     Compute Jaro-Winkler similarity.
        /// </summary>
        /// <param name="str1">The first string to compare.</param>
        /// <param name="str2">The second string to compare.</param>
        /// <returns>The Jaro-Winkler similarity in the range [0, 1]</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Similarity(string str1, string str2)
        {
            Guarantee.IsStringNotNullOrEmpty(str1, nameof(str1));
            Guarantee.IsStringNotNullOrEmpty(str2, nameof(str2));

            if (str1.Equals(str2)) return 1f;

            var mtp = _macthes(str1, str2);
            float m = mtp[0];
            if (m == 0) return 0f;
            double j = (m / str1.Length + m / str2.Length + (m - mtp[1]) / m)
                       / Three;
            var jw = j;

            if (j > DefaultThreshold) jw = j + Math.Min(JwCoef, 1.0 / mtp[Three]) * mtp[2] * (1 - j);
            return jw;
        }

        private int[] _macthes(string str1, string str2)
        {
            string max, min;
            if (str1.Length > str2.Length)
            {
                max = str1;
                min = str2;
            }
            else
            {
                max = str2;
                min = str1;
            }

            var range = Math.Max(max.Length / 2 - 1, 0);

            //int[] matchIndexes = new int[min.Length];
            //Arrays.fill(matchIndexes, -1);
            var matchIndexes = Enumerable.Repeat(-1, min.Length).ToArray();

            var matchFlags = new bool[max.Length];
            var matches = 0;
            for (var mi = 0; mi < min.Length; mi++)
            {
                var c1 = min[mi];
                for (int xi = Math.Max(mi - range, 0),
                    xn = Math.Min(mi + range + 1, max.Length);
                    xi < xn;
                    xi++)
                    if (!matchFlags[xi] && c1 == max[xi])
                    {
                        matchIndexes[mi] = xi;
                        matchFlags[xi] = true;
                        matches++;
                        break;
                    }
            }

            var ms1 = new char[matches];
            var ms2 = new char[matches];
            for (int i = 0, si = 0; i < min.Length; i++)
                if (matchIndexes[i] != -1)
                {
                    ms1[si] = min[i];
                    si++;
                }

            for (int i = 0, si = 0; i < max.Length; i++)
                if (matchFlags[i])
                {
                    ms2[si] = max[i];
                    si++;
                }

            var transpositions = 0;
            for (var mi = 0; mi < ms1.Length; mi++)
                if (ms1[mi] != ms2[mi])
                    transpositions++;
            var prefix = 0;
            for (var mi = 0; mi < min.Length; mi++)
                if (str1[mi] == str2[mi])
                    prefix++;
                else
                    break;
            return new[] {matches, transpositions / 2, prefix, max.Length};
        }
    }
}