using InParadise.Domain.Entities;

namespace InParadise.Web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa>? Villas { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public int Nights { get; set; }
    }
}