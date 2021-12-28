using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Iris.ViewModels
{
    public class SearchAuctionViewModel
    {
        public SearchAuctionViewModel()
        {
            AuctionItemList = new List<AuctionItemViewModel>();
        }

        public int PageNumber { get; set; }

        public int LastPageNumber { get; set; }
        
        public int AuctionAvailableItemCount { get; set; }

        public Enums.SortState SortState { get; set; } = Enums.SortState.DSNews;

        public List<AuctionItemViewModel> AuctionItemList { get; set; }


    }
}
