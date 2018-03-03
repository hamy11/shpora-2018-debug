using System;
using System.Text;
using Microsoft.Diagnostics.Runtime;

namespace ClrMD
{
    public static class DictionariesPrinter
    {
        public static void PrintDictionaries(ClrHeap heap)
        {
            foreach (var clrObject in heap.EnumerateObjects())
            { 
                if (!string.Equals(clrObject.Type?.Name, "System.Collections.Generic.Dictionary<System.String,System.String>"))
                    continue;
                Console.WriteLine("** Dictionary<string, string> at " + clrObject.HexAddress);
                PrintDictionary(clrObject);
            }
        }

        private static void PrintDictionary(ClrObject dictClrObject)
        {
            var entries = dictClrObject.GetObjectField("entries");
         
            if (entries.IsNull)
                return;
            
            var count = dictClrObject.GetField<int>("count");
            var arrayType = entries.Type;
            var elementType = arrayType.ComponentType;
            var entriesLength = Math.Min(count, arrayType.GetArrayLength(entries.Address));
            var heap = arrayType.Heap;
            
            var keyField = elementType.GetFieldByName("key");
            var valueField = elementType.GetFieldByName("value");
            var hashField = elementType.GetFieldByName("hashCode");
            
            for (var i = 0; i < entriesLength; i++)
            {
                var entryAddr = arrayType.GetArrayElementAddress(entries.Address, i);
                var hash = (int) hashField.GetValue(entryAddr, true);
                if (hash < 0)
                    continue;

                var key = Repr(heap, keyField.GetValue(entryAddr, true));
                var value = Repr(heap, valueField.GetValue(entryAddr, true));
                Console.WriteLine(key + ": " + value);
            }
        }

        public static void PrintConcurrentDictionaries(ClrHeap heap)
        {
            const string concurrentDictionaryTypeName = "...";
            
            foreach (var clrObject in heap.EnumerateObjects())
            {
                if (!string.Equals(clrObject.Type?.Name, concurrentDictionaryTypeName))
                    continue;
                Console.WriteLine("** ConcurrentDictionary<string, string> at " + clrObject.HexAddress);
                PrintConcurrentDictionary(clrObject);
            }
        }

        private static void PrintConcurrentDictionary(ClrObject dictClrObject)
        {
            var tables = dictClrObject.GetObjectField("m_tables");
            //TODO: print items
        }


        private static string Repr(ClrHeap heap, object value)
        {
            if (value is ulong addr)
                return Repr(heap, addr);

            return Escape(value.ToString());
        }
        
        private static string Repr(ClrHeap heap, ulong addr)
        {
            var type = heap.GetObjectType(addr);
            return Escape(type?.GetValue(addr)?.ToString() ?? string.Empty);
        }

        private static string Escape(string s)
        {
            var sb = new StringBuilder(s.Length);
            foreach (var c in s)
                sb.Append(char.IsLetterOrDigit(c) || char.IsSymbol(c) ? c : '_');

            return sb.ToString();
        }
    }
}