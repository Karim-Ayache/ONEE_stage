using System.Collections.Generic;

namespace ONEE_Stage.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<OfferViewModel> FeaturedOffers { get; set; } = new List<OfferViewModel>();
        public int TotalOpenOffers { get; set; }
    }
}