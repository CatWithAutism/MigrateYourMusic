using System;
using System.Collections.Generic;
using System.Text;

namespace WebHandlers.Utils
{
    public static class CommonUtils
    {
        public static IEnumerable<List<T>> SplitList<T>(IEnumerable<T> collection, int nSize = 30)
        {
            List<T> listToSplit = new List<T>(collection);
            List<List<T>> listOfLists = new List<List<T>>();

            for (int i = 0; i < listToSplit.Count; i += nSize)
            {
                listOfLists.Add(listToSplit.GetRange(i, Math.Min(nSize, listToSplit.Count - i)));
            }

            return listOfLists;
        }
    }
}
