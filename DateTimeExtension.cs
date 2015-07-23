using System;
using System.Collections.Generic;

namespace DrWayne {
	public static class DateTimeExtension {
		
		private static HashSet<DateTime> _holidays = new HashSet<DateTime>();
		
		public static void AddSpecialHoliday(DateTime d) {
			_holidays.Add(d);
		}
		
		public static bool IsSpecialHoliday(this DateTime d) {
			return _holidays.Contains(d);
		}
		
		public static bool IsHoliday(this DateTime d) {
			return d.IsSpecialHoliday() || d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday;
		}
		
		public static bool NeedOPD(this DateTime d) {
			return _holidays.Contains(d) || d.DayOfWeek == DayOfWeek.Saturday;
		}		
	}
}