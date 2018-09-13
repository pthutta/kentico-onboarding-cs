using System;
using NUnit.Framework.Constraints;

namespace TodoApp.Api.Tests.Extensions
{
    public static class EqualConstraintExtensions
    {
        private static readonly Lazy<ItemComparer> ItemComparer = new Lazy<ItemComparer>();

        public static EqualConstraint UsingItemComparer(this EqualConstraint constraint)
        {
            return constraint.Using(ItemComparer.Value);
        }

        public static CollectionItemsEqualConstraint UsingItemComparer(this CollectionEquivalentConstraint constraint)
        {
            return constraint.Using(ItemComparer.Value);
        }
    }
}
