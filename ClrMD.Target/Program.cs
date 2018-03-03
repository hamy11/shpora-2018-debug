using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ClrMD.Target
{
    internal class Program
    {
        private static List<Dictionary<string, string>> dicts = new List<Dictionary<string, string>>();
        private static List<ConcurrentDictionary<string, string>> concurrentDicts = new List<ConcurrentDictionary<string, string>>();
        
        public static void Main(string[] args)
        {
            var dict1 = new Dictionary<string, string>();
            SetupDictionary1(dict1);
            dicts.Add(dict1);
            var dict2 = new Dictionary<string, string>();
            SetupDictionary2(dict2);
            dicts.Add(dict2);
            var cDict1 = new ConcurrentDictionary<string, string>();
            SetupDictionary1(cDict1);
            concurrentDicts.Add(cDict1);
            var cDict2 = new ConcurrentDictionary<string, string>();
            SetupDictionary2(cDict2);
            concurrentDicts.Add(cDict2);
            Console.ReadKey();
        }

        private static void SetupDictionary1(IDictionary<string, string> dict)
        {
            dict["aa"] = "11";
            dict["bb"] = "22";
            dict["cc"] = "33";
            dict["dd"] = "44";
            dict["ee"] = "55";
            dict.Remove("bb");
        }

        private static void SetupDictionary2(IDictionary<string, string> dict)
        {
            for (var i = 'a'; i <= 'z'; ++i)
                dict[i.ToString()] = i.ToString();
            dict.Remove("a");
            dict.Remove("e");
            dict.Remove("i");
            dict.Remove("o");
            dict.Remove("u");
        }
    }
}