using System;
using System.Collections.Generic;
namespace CSharpProgramming.Repositories
{
    internal class Repository<T>
    {
        private List<T> items = new List<T>();
        public event Action<T> ItemAdded;
        public List<T> Items { get { return this.items; } }
        public void Add(T item)
        {
            this.items.Add(item);
            if (this.ItemAdded != null) this.ItemAdded(item);
        }
        public IEnumerable<T> Iterate()
        {
            foreach (T item in this.items) yield return item;
        }
    }
}
