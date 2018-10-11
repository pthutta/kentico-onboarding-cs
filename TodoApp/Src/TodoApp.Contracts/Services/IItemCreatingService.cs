using System.Threading.Tasks;
using TodoApp.Contracts.Models;

namespace TodoApp.Contracts.Services
{
    public interface IItemCreatingService
    {
        Task<Item> CreateAsync(Item item);
    }
}
