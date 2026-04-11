using InParadise.Domain.Entities;

namespace InParadise.Application.Services.Interface
{
    public interface IVillaNumberService
    {
        IEnumerable<VillaNumber> GetAllVillaNumbers(string? includeProperties = null);

        VillaNumber GetVillaNumberById(int id, string? includeProperties = null);
        void CreateVillaNumber(VillaNumber villaNumber);
        void UpdateVillaNumber(VillaNumber villaNumber);
        bool DeleteVillaNumber(int id);

        bool CheckVillaNumberExist(int villaNumberId);
    }
}