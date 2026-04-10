using InParadise.Domain.Entities;

namespace InParadise.Application.Services.Interface
{
    public interface IVillaNumberService
    {
        IEnumerable<VillaNumber> GetAllVillaNumbers(string? includeProperties);
        VillaNumber GetVillaNumberById(int id, string? includeProperties);
        void CreateVillaNumber(VillaNumber villaNumber);
        void UpdateVillaNumber(VillaNumber villaNumber);
        bool DeleteVillaNumber(int id);
    }
}