using System;
using System.Collections.Generic;
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
    }
}
