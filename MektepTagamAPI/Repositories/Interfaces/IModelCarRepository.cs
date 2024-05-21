using ApiAvtoMigNew.Models;
using MektepTagamAPI.Models;

namespace ApiAvtoMigNew.Repositories.Interfaces
{
    public interface IModelCarRepository
    {
        Task<Car> GetByIdAsync(int id);
        Task<IEnumerable<ModelCar>> GetAllAsync();
        Task AddAsync(ModelCar car);
        Task UpdateAsync(ModelCar car);
        Task DeleteAsync(int id);
        Task<bool> ExistsWithName(string name);
    }
}
