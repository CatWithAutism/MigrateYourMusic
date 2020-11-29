using System;
using System.Linq;

namespace WebHandlers.Utils
{
    public static class JaroWinkler
    {
        private const double DefaultThreshold = 0.7;
        private const int Three = 3;
        private const double JwCoef = 0.1;

        /// <summary>
        /// Compute Jaro-Winkler similarity.
        /// </summary>
        /// <param name="str1">The first string to compare.</param>
        /// <param name="str2">The second string to compare.</param>
        /// <returns>The Jaro-Winkler similarity in the range [0, 1]</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public static double Similarity(string str1, string str2)
        {
            Guarantee.IsStringNotNullOrEmpty(str1, nameof(str1));
            Guarantee.IsStringNotNullOrEmpty(str2, nameof(str2));

            if (str1.Equals(str2))
            {
                return 1f;
            }

            int[] mtp = _macthes(str1, str2);
            float m = mtp[0];
            if (m == 0)
            {
                return 0f;
            }
            double j = ((m / str1.Length + m / str2.Length + (m - mtp[1]) / m))
                    / Three;
            double jw = j;

            if (j > DefaultThreshold)
            {
                jw = j + Math.Min(JwCoef, 1.0 / mtp[Three]) * mtp[2] * (1 - j);
            }
            return jw;
        }

        /// <summary>
        /// Return 1 - similarity.
        /// </summary>
        /// <param name="str1">The first string to compare.</param>
        /// <param name="str2">The second string to compare.</param>
        /// <returns>1 - similarity</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public static double Distance(string str1, string str2)
            => 1.0 - Similarity(str1, str2);

        private static int[] _macthes(string str1, string str2)
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
            int range = Math.Max(max.Length / 2 - 1, 0);

            //int[] matchIndexes = new int[min.Length];
            //Arrays.fill(matchIndexes, -1);
            int[] matchIndexes = Enumerable.Repeat(-1, min.Length).ToArray();

            bool[] matchFlags = new bool[max.Length];
            int matches = 0;
            for (int mi = 0; mi < min.Length; mi++)
            {
                char c1 = min[mi];
                for (int xi = Math.Max(mi - range, 0),
                        xn = Math.Min(mi + range + 1, max.Length); xi < xn; xi++)
                {
                    if (!matchFlags[xi] && c1 == max[xi])
                    {
                        matchIndexes[mi] = xi;
                        matchFlags[xi] = true;
                        matches++;
                        break;
                    }
                }
            }
            char[] ms1 = new char[matches];
            char[] ms2 = new char[matches];
            for (int i = 0, si = 0; i < min.Length; i++)
            {
                if (matchIndexes[i] != -1)
                {
                    ms1[si] = min[i];
                    si++;
                }
            }
            for (int i = 0, si = 0; i < max.Length; i++)
            {
                if (matchFlags[i])
                {
                    ms2[si] = max[i];
                    si++;
                }
            }
            int transpositions = 0;
            for (int mi = 0; mi < ms1.Length; mi++)
            {
                if (ms1[mi] != ms2[mi])
                {
                    transpositions++;
                }
            }
            int prefix = 0;
            for (int mi = 0; mi < min.Length; mi++)
            {
                if (str1[mi] == str2[mi])
                {
                    prefix++;
                }
                else
                {
                    break;
                }
            }
            return new[] { matches, transpositions / 2, prefix, max.Length };
        }
    }
}
