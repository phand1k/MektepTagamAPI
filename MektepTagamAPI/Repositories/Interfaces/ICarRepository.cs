using MektepTagamAPI.Models;

namespace MektepTagamAPI.Repositories.Interfaces
{
    public interface ICarRepository
    {
        Task<Car> GetByIdAsync(int id);
        Task<IEnumerable<Car>> GetAllAsync();
        Task AddAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(int id);
        Task<bool> ExistsWithName(string name);
    }
}
