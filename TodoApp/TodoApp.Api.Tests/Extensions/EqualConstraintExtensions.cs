using NUnit.Framework.Constraints;

namespace TodoApp.Api.Tests.Extensions
{
    public static class EqualConstraintExtensions
    {
        private static readonly ItemComparer ItemComparer = new ItemComparer();

        public static EqualConstraint UsingItemComparer(this EqualConstraint constraint)
        {
            return constraint.Using(ItemComparer);
        }

        public static CollectionItemsEqualConstraint UsingItemComparer(this CollectionEquivalentConstraint constraint)
        {
            return constraint.Using(ItemComparer);
        }
    }
}
