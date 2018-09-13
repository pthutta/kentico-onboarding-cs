using System;
using System.Collections.Generic;
using TodoApp.Contracts.Models;

namespace TodoApp.Api.Tests.Extensions
{
    public class ItemComparer : IEqualityComparer<Item>
    {
        public bool Equals(Item x, Item y)
        {
            return x.IsEqual(y);
        }

        public int GetHashCode(Item obj)
        {
            var hashCode = -1144598946;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(obj.Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(obj.Text);
            return hashCode;
        }
    }
}
