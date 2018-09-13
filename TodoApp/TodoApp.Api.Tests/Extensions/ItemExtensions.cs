using TodoApp.Contracts.Models;

namespace TodoApp.Api.Tests.Extensions
{
    public static class ItemExtensions
    {
        public static bool IsEqual(this Item item1, Item item2)
        {
            return item1.Id == item2.Id && item1.Text == item2.Text;
        }
    }
}
