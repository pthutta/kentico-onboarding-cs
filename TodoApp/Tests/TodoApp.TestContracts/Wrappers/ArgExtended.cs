using NSubstitute;
using TodoApp.Contracts.Models;
using TodoApp.TestContracts.Comparers;

namespace TodoApp.TestContracts.Wrappers
{
    public static class ArgExtended
    {
        public static Item IsItem(Item otherItem)
            => Arg.Is<Item>(newItem => ItemComparer.Instance.Equals(newItem, otherItem));
    }
}
