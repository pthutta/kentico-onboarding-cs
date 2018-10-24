using NUnit.Framework.Constraints;
using TodoApp.TestContracts.Comparers;

namespace TodoApp.TestContracts.Extensions
{
    public static class EqualConstraintExtensions
    {
        public static EqualConstraint UsingItemComparer(this EqualConstraint constraint) 
            => constraint.Using(ItemComparer.Instance);

        public static CollectionItemsEqualConstraint UsingItemComparer(this CollectionEquivalentConstraint constraint) 
            => constraint.Using(ItemComparer.Instance);
    }
}
