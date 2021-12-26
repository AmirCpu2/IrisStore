using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Enums
    {
        public enum ItemType
        {
            ///<summary>رنگ کالا</summary>
            ProductColor ,
            ///<summary>فروشنده</summary>
            Seller ,
            ///<summary>نام تجاری</summary>
            Brand ,
        }
        
        public enum CommentState
        {
            ///<summary>در حال بررسی</summary>
            Pending = 0,
            ///<summary>تایید شده</summary>
            Confirmed = 1,
            ///<summary>حذف شده</summary>
            Deleted = 2,
            /// <summary>بررسی ویرایش</summary>
            PendingForEdit = 3,
        }
        
        public enum Rate
        {
            VeryBad = 1,
            Bad = 2,
            Normal = 3,
            Good = 4,
            VeryGood = 5
        }
        
        public enum SuggestStatus
        {
            Yes = 1,
            No = 2,
            Nop = 3,
        }

        public enum CoinPacket
        {
            [Description("200")]
            TwoHundred = 1, //200
            [Description("500")]
            FiveHundred = 2, //500
            [Description("1000")]
            OneThousand = 3, //1000
            [Description("2400")]
            TwoThousandFourHundred = 4, //2400
            [Description("3200")]
            ThreeThousandTwoHundred = 5, //3200
            [Description("5000")]
            FiveThousand = 6, //5000
        }
        
        public enum CoinPacketPrice
        {
            [Description("10")]
            TwoHundred = 1, //200
            [Description("30")]
            FiveHundred = 2, //500
            [Description("60")]
            OneThousand = 3, //1000
            [Description("130")]
            TwoThousandFourHundred = 4, //2400
            [Description("180")]
            ThreeThousandTwoHundred = 5, //3200
            [Description("250")]
            FiveThousand = 6, //5000
        }

        public enum CoinFactorStatus
        {
            [Description("لغو شده")]
            Cancelled = -1,
            [Description("در حال پرداخت")]
            Paying = 0,
            [Description("پرداخت شده")]
            Paid = 1,
        }

    }
}
