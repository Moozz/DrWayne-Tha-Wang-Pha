using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrWayne {
	public class Wayne {
		public Wayne(DateTime wayneDate) {
			WayneDate = wayneDate;
		}
		
		public DateTime WayneDate { get; set; }
		
		public Doctor ERDoctor { get; set; }
		
		public Doctor WardDoctor { get; set; }
		
		public Doctor OPDDoctor { get; set; }
		
		public bool IsAcceptable () {
			return  ERDoctor != null && WardDoctor != null &&
					(ERDoctor.Exp != DoctorExperience.Intern1 || ERDoctor.Exp != WardDoctor.Exp) &&
					(WayneDate.DayOfWeek != DayOfWeek.Saturday || OPDDoctor != null);
		}
	}
	
	public class WayneTable {
		public WayneTable(int year, int month) {
			_wayneTable = new List<Wayne>();
			if (month < 1 || month > 12)
				throw new Exception("Month must be 1-12");
			Month = month;
			Year = year;
		}
		public List<Wayne> _wayneTable;
		
		public int Month { get; private set; }
		
		public int Year { get; private set; }
		
		public bool AddWayneIfAcceptable(Wayne wayne) {
			if (wayne.IsAcceptable() && 
				!IsThisDoctorAtERYesterday(wayne.ERDoctor) &&
				!IsThisDoctorInWayneTheLastTwoDays(wayne.ERDoctor) &&
				!IsThisDoctorInWayneTheLastTwoDays(wayne.WardDoctor) && 
				(wayne.OPDDoctor == null || !IsThisDoctorInWayneTheLastTwoDays(wayne.OPDDoctor))) {
				_wayneTable.Add(wayne);
				return true;
			}
			return false;			
		}
		
		public override string ToString() {
			var sb = new StringBuilder();
			sb.AppendFormat("{0} {1, 10} {2, 10}, {3, 10}\n", "Date", "ER", "WARD", "OPD");
			foreach (var wayne in _wayneTable) {
				sb.AppendFormat("{0} {1, 10} {2, 10}, {3, 10}\n", 
					wayne.WayneDate.ToString("dd MMM yyyy"), 
					wayne.ERDoctor.Name, 
					wayne.WardDoctor.Name, 
					wayne.OPDDoctor == null ? "-" : wayne.OPDDoctor.Name);
			}
			return sb.ToString();
		}
		
		public WayneTable Copy() {
			var wayneTableCopy = new WayneTable(Year, Month);
			wayneTableCopy._wayneTable = this._wayneTable.ToList();
			return wayneTableCopy;
		}
		
		public bool IsDone() {
			return _wayneTable.Count == DateTime.DaysInMonth(Year, Month);
		}
		
		private bool IsThisDoctorAtERYesterday(Doctor d) {
			return _wayneTable.Count > 0 && _wayneTable.Last().ERDoctor == d;
		}
		
		private bool IsThisDoctorInWayneTheLastTwoDays(Doctor d) {
			if (_wayneTable.Count < 2)
				return false;

			var wayneTableCopy = _wayneTable.ToList();
			wayneTableCopy.Reverse();
			return wayneTableCopy.Take(2).All(x => x.ERDoctor == d || x.WardDoctor == d);
		}
	}
}