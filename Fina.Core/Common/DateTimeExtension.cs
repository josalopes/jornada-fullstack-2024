using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fina.Core.Common
{
    public static class DateTimeExtension
    {
        public static DateTime GetFirstDay(this DateTime dateTime, int? year = null, int? month = null)
            => new(year ?? dateTime.Year, month ?? dateTime.Month, 1);

        public static DateTime GetLastDay(this DateTime dateTime, int? year = null, int? month = null)
            => new DateTime(year ?? dateTime.Year, month ?? dateTime.Month, 1)
            .AddMonths(1)
            .AddDays(-1);
    }
}
