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
		public WayneTable(int maxN) {
			_wayneTable = new List<Wayne>();
			
		}
		public List<Wayne> _wayneTable;
		public int _maxN;
		
		public bool AddWayneIfAcceptable(Wayne wayne) {
			if (wayne.IsAcceptable() && 
				!IsThisDoctorAtERYesterday(wayne.ERDoctor) &&
				!IsThisDoctorInWayneTheLastTwoDays(wayne.ERDoctor) &&
				!IsThisDoctorInWayneTheLastTwoDays(wayne.WardDoctor) && 
				(wayne.OPDDoctor != null && !IsThisDoctorInWayneTheLastTwoDays(wayne.OPDDoctor))) {
				_wayneTable.Add(wayne);
				return true;
			}
			return false;			
		}
		
		public bool IsDone() {
			return _wayneTable.Count() == _maxN;
		}
		
		public override string ToString() {
			var sb = new StringBuilder();
			foreach (var wayne in _wayneTable) {
				sb.AppendFormat("{0}\n", wayne.WayneDate.ToString("dd MMM yyyy"));
			}
			return sb.ToString();
		}
		
		public WayneTable Copy() {
			var wayneTableCopy = new WayneTable(this._maxN);
			wayneTableCopy._wayneTable = this._wayneTable.ToList();
			return wayneTableCopy;
		}
		
		private bool IsThisDoctorAtERYesterday(Doctor d) {
			return _wayneTable.Last().ERDoctor == d;
		}
		
		private bool IsThisDoctorInWayneTheLastTwoDays(Doctor d) {
			var wayneTableCopy = _wayneTable.ToList();
			wayneTableCopy.Reverse();
			return wayneTableCopy.Take(2).All(x => x.ERDoctor == d || x.WardDoctor == d);
		}
	}
}