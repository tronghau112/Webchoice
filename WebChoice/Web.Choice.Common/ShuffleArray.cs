using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Choice.Common
{
    public class ShuffleArray
    {
        private static readonly Random Random = new Random();

        public static string[] Randomize(string[] arr)
        {
            List<KeyValuePair<int, string>> list =
                new List<KeyValuePair<int, string>>();
            foreach (string s in arr)
            {
                list.Add(new KeyValuePair<int, string>(Random.Next(), s));
            }
            var sorted = from item in list
                orderby item.Key
                select item;
            string[] result = new string[arr.Length];
            int index = 0;
            foreach (KeyValuePair<int, string> pair in sorted)
            {
                result[index] = pair.Value;
                index++;
            }
            return result;
        }
    }
}
