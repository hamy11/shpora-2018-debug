using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JPEG
{
    public class SortedMultiSet<T> : IEnumerable<T>
    {
        private readonly SortedDictionary<T, int> dict;

        public SortedMultiSet()
        {
            dict = new SortedDictionary<T, int>();
        }

        public SortedMultiSet(IEnumerable<T> items) : this()
        {
            Add(items);
        }

        public bool Contains(T item)
        {
            return dict.ContainsKey(item);
        }

        public int Count => dict.Count;

        public void Add(T item)
        {
            if (dict.ContainsKey(item))
                dict[item]++;
            else
                dict[item] = 1;
        }

        public void Add(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void Remove(T item)
        {
            if (!dict.ContainsKey(item))
                throw new ArgumentException();
            if (--dict[item] == 0)
                dict.Remove(item);
        }

        public T Peek()
        {
            if (!dict.Any())
                throw new NullReferenceException();
            return dict.First().Key;
        }

        public T Pop()
        {
            var item = Peek();
            Remove(item);
            return item;
        }

        public T PopMinOrDefault()
        {
            return !dict.Any() ? default(T) : Pop();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var kvp in dict)
                for (var i = 0; i < kvp.Value; i++)
                    yield return kvp.Key;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}