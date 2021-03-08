using System;
using System.Collections.Generic;

namespace WebHandlers.Utils
{
    public static class CommonUtils
    {
        /// <summary>
        /// Splits your Enumerable into lists with the specified count of max elements.
        /// </summary>
        /// <param name="collection">Your collection.</param>
        /// <param name="nSize">Max size of one list.</param>
        /// <typeparam name="T">It doesn't make a sense. I mean whatever you want :D</typeparam>
        /// <returns></returns>
        public static IEnumerable<List<T>> SplitOnLists<T>(IEnumerable<T> collection, int nSize = 30)
        {
            Guarantee.IsEnumerableNotNullOrEmpty(collection, nameof(collection));

            //transform to list cause you can get there a range without any problems :)
            var listToSplit = new List<T>(collection);
            var listOfLists = new List<List<T>>();

            for (int i = 0; i < listToSplit.Count; i += nSize)
                listOfLists.Add(listToSplit.GetRange(i, Math.Min(nSize, listToSplit.Count - i)));

            return listOfLists;
        }
    }
}