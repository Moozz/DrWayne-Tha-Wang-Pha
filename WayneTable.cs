using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrWayne {
	public class WayneTable {
		public WayneTable(int year, int month) {
			_wayneTable = new List<Wayne>();
			if (month < 1 || month > 12)
				throw new Exception("Month must be 1-12");
			Month = month;
			Year = year;
			
			_totalDaysInMonth = DateTime.DaysInMonth(year, month);
			_totalWorkingDay = 0;
			for (var i = 1; i <= _totalDaysInMonth; i++) {
				_totalWorkingDay += new DateTime(year, month, i).IsHoliday() ? 0 : 1;
			}
		}
		public List<Wayne> _wayneTable;
		
		public int Month { get; private set; }
		
		public int Year { get; private set; }
		
		private int _totalDaysInMonth;
		private int _totalWorkingDay;
		
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
			sb.AppendFormat("Wayne of Month {0}, {1}\n", Month, Year);
			sb.AppendFormat("{0, -7} {1, 10} {2, 10} {3, 10}\n", "Date", "ER", "WARD", "OPD");
			foreach (var wayne in _wayneTable) {
				sb.AppendFormat("{0, -7} {1, 10} {2, 10} {3, 10}\n", 
					wayne.WayneDate.ToString("ddd dd"), 
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
			return _wayneTable.Count == _totalDaysInMonth;
		}
		
		public int GetLastWayneDay() {
			return _wayneTable.Count;
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
		
		public bool FairEnough() {
			var wayneGroupByDoctor = _wayneTable.SelectMany(x => new[] {
					new {
						Doctor = x.ERDoctor,
						WayneType = Wayne.Type.ER,
						Day = x.WayneDate
					},
					new {
						Doctor = x.WardDoctor,
						WayneType = Wayne.Type.Ward,
						Day = x.WayneDate
					},
					new {
						Doctor = x.OPDDoctor,
						WayneType = Wayne.Type.OPD,
						Day = x.WayneDate
					}
				})
				.Where(x => x.Doctor != null)
				.GroupBy(x => x.Doctor);
				
			var restList = wayneGroupByDoctor.Select(g => new {
				  	Doctor = g.Key,
				  	RestOnWorkingDayCount = _totalWorkingDay - g.Select(x => x.Day).Distinct().Count(x => !x.IsHoliday()),
					RestOnHolidayCount = (_totalDaysInMonth - _totalWorkingDay) - g.Select(x => x.Day).Distinct().Count(x => x.IsHoliday())
				}).ToList();
			var countList =	wayneGroupByDoctor.Select(g => new {
					Doctor = g.Key,
					ERCount = g.Count(x => x.WayneType == Wayne.Type.ER),
					WardCount = g.Count(x => x.WayneType == Wayne.Type.Ward),
					OPDCount = g.Count(x => x.WayneType == Wayne.Type.OPD)
				}).ToList();
			
			if (countList.Max(x => x.ERCount) - countList.Min(x => x.ERCount) <= 2 &&
				countList.Max(x => x.WardCount) - countList.Min(x => x.WardCount) <= 2 &&
				restList.Max(x => x.RestOnHolidayCount) - restList.Min(x => x.RestOnHolidayCount) <= 2 && 
				restList.Max(x => x.RestOnWorkingDayCount) - restList.Min(x => x.RestOnWorkingDayCount) <= 4) {
				Console.WriteLine("{0, -10} : {1, 5} {2, 5} {3, 5}", "Name", "ER", "Ward", "OPD");
				foreach (var p in countList) {
					Console.WriteLine("{0, -10} : {1, 5} {2, 5} {3, 5}", p.Doctor.Name, p.ERCount, p.WardCount, p.OPDCount);	
				}
				Console.WriteLine();
				Console.WriteLine("{0, -8} : {1, 10} {2, 10}", "Name", "RestWDay", "RestHoliday");
				foreach (var p in restList) {
					Console.WriteLine("{0, -8} : {1, 10} {2, 10}", p.Doctor.Name,
						 p.RestOnWorkingDayCount,
						 p.RestOnHolidayCount);
				}
				return true;	
			}
			return false;
		}
	}	
}
