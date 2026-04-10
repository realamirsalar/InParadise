using InParadise.Domain.Entities;

namespace InParadise.Application.Services.Interface
{
    public interface IVillaService
    {
        IEnumerable<Villa> GetAllVillas(string? includeProperties = null);
        Villa GetVillaById(int id, string? includeProperties = null);
        void CreateVilla(Villa villa);
        void UpdateVilla(Villa villa);
        bool DeleteVilla(int id);

        IEnumerable<Villa> GetVillasAvailabilityByDate(int nights, DateOnly checkInDate);
    }
}