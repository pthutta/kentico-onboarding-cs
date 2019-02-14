using System;
using System.Collections.Generic;
using TodoApp.Contracts.Models;

namespace TodoApp.TestContracts.Comparers
{
    internal class ItemComparer : IEqualityComparer<Item>
    {
        private static readonly Lazy<ItemComparer> Comparer = new Lazy<ItemComparer>(() => new ItemComparer());

        public static ItemComparer Instance => Comparer.Value;

        private ItemComparer() {}

        public bool Equals(Item x, Item y)
            => x != null &&
               y != null &&
               x.Id == y.Id &&
               x.Text == y.Text &&
               x.CreationTime == y.CreationTime &&
               x.LastUpdateTime == y.LastUpdateTime;

        public int GetHashCode(Item obj)
        {
            var hashCode = -1144598946;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(obj.Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(obj.Text);
            return hashCode;
        }
    }
}
