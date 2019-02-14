using System.Threading.Tasks;
using TodoApp.Contracts.Models;

namespace TodoApp.Contracts.Services
{
    public interface IItemUpdatingService
    {
        Task UpdateAsync(Item item);
    }
}
