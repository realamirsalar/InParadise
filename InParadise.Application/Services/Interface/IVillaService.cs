using InParadise.Domain.Entities;

namespace InParadise.Application.Services.Interface
{
    public interface IVillaService
    {
        IEnumerable<Villa> GetAllVillas(string? includeProperties);
        Villa GetVillaById(int id, string? includeProperties);
        void CreateVilla(Villa villa);
        void UpdateVilla(Villa villa);
        bool DeleteVilla(int id);
    }
}