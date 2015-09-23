using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrWayne {
	public class WayneTable {
		public WayneTable(List<Doctor> doctorList, int year, int month) {
			_wayneTable = new List<Wayne>();
			_doctorList = doctorList;
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
		private List<Wayne> _wayneTable;
		private List<Doctor> _doctorList;
		
		public int Month { get; private set; }
		
		public int Year { get; private set; }
		
		private int _totalDaysInMonth;
		private int _totalWorkingDay;
		
		private bool AddWayneIfAcceptable(Wayne wayne) {
			if (wayne.IsAcceptable()) { 
				_wayneTable.Add(wayne);
				return true;
			}
			return false;			
		}
		
		public void AddFixWayne(Wayne wayne) {
			if (!wayne.IsAcceptable())
				throw new Exception("Your fixed wayne is not correct, please recheck it");
			
			_doctorList.First(x => x == wayne.ERDoctor).ERWayne.Add(wayne.WayneDate);
			_doctorList.First(x => x == wayne.WardDoctor).WardWayne.Add(wayne.WayneDate);
			if (wayne.WayneDate.NeedOPD())
				_doctorList.First(x => x == wayne.OPDDoctor).OPDWayne.Add(wayne.WayneDate);

			_wayneTable.Add(wayne);
		}
		
		public override string ToString() {
			var sb = new StringBuilder();
			sb.AppendFormat("Wayne of Month {0}, {1}\n", Month, Year);
			sb.AppendFormat("{0, -7} {1, 10} {2, 10} {3, 10}\n", "Date", "ER", "WARD", "OPD");
			foreach (var wayne in _wayneTable.OrderBy(x => x.WayneDate)) {
				sb.AppendFormat("{0, -7} {1, 10} {2, 10} {3, 10}\n", 
					wayne.WayneDate.ToString("ddd dd"), 
					wayne.ERDoctor.Name, 
					wayne.WardDoctor.Name, 
					wayne.OPDDoctor == null ? "-" : wayne.OPDDoctor.Name);
			}
			return sb.ToString();
		}
		
		public string ToCsvFormat() {
			var sb = new StringBuilder();
			sb.AppendLine(string.Join(",", new [] { "Date" }.Concat(_doctorList.Select(x => x.Name))));
			foreach (var wayne in _wayneTable.OrderBy(x => x.WayneDate)) {				
				sb.AppendLine(string.Join(",", new [] {wayne.WayneDate.ToString("ddd dd")}.Concat(_doctorList.Select(x => x.ERWayne.Contains(wayne.WayneDate) 
						? "ER"
						: x.WardWayne.Contains(wayne.WayneDate)
							? "WA"
							: x.OPDWayne.Contains(wayne.WayneDate) 
								? "OPD"
								: "-"))));
			}
			return sb.ToString();
		}
		
		public void ShowResult() {
			Console.WriteLine("{0, -10} : {1, 5} {2, 5} {3, 5}", "Name", "ER", "Ward", "OPD");
	  		foreach (var p in _doctorList) {
	  			Console.WriteLine("{0, -10} : {1, 5} {2, 5} {3, 5}", p.Name, p.ERWayne.Count(), p.WardWayne.Count(), p.OPDWayne.Count());	
	  		}
			Console.WriteLine("{0}", ToCsvFormat());
			//Console.WriteLine("{0}", ToString());
			//Console.WriteLine("Tireness level at the end of the month");
			//Console.WriteLine(string.Join("\n", _doctorList.Select(x => x.Name + " : " + x.Tireness)));
			//Console.WriteLine("=== END ===\n");
			Console.ReadLine();
		}
		
		private WayneTable Copy() {
			var wayneTableCopy = new WayneTable(_doctorList, Year, Month);
			wayneTableCopy._wayneTable = this._wayneTable.ToList();
			return wayneTableCopy;
		}
		
		private bool IsDone() {
			return _wayneTable.Count == _totalDaysInMonth;
		}
		
		private int GetFirstDayToFill() {
			return Enumerable.Range(1, _totalDaysInMonth)
							.Except(_wayneTable.Select(x => x.WayneDate.Day).OrderBy(x => x))
							.Min();
		}
		
		private bool FairEnough() {
			var doctorListWithoutHandicap = _doctorList.Where(d => d.Factor == 1);
			var restList = doctorListWithoutHandicap
				.Select(x => new {
					Name = x.Name,
					WorkDayList = x.GetAllWayneDate()
				})
				.Select(x => new {
				  	Name = x.Name,
				  	RestOnWorkingDayCount = _totalWorkingDay - x.WorkDayList.Count(y => !y.IsHoliday()),
					RestOnHolidayCount = (_totalDaysInMonth - _totalWorkingDay) - x.WorkDayList.Count(y => y.IsHoliday())
				}).ToList();
			if (//// Special case for each month
				//_doctorList.Single(d => d.Name == "P' Did").GetAllWayneDate().Count() == 10 &&
				//_doctorList.Single(d => d.Name == "Saran").GetAllWayneDate().Count() == 9 &&
				//_doctorList.Single(d => d.Name == "Gofilm").GetAllWayneDate().Count() == 9 &&
				////
				doctorListWithoutHandicap.Max(x => x.ERWayne.Count()) - doctorListWithoutHandicap.Min(x => x.ERWayne.Count()) <= 2 &&
				doctorListWithoutHandicap.Max(x => x.WardWayne.Count()) - doctorListWithoutHandicap.Min(x => x.WardWayne.Count()) <= 2 &&
				restList.Max(x => x.RestOnHolidayCount) - restList.Min(x => x.RestOnHolidayCount) <= 2 && 
			  	restList.Max(x => x.RestOnWorkingDayCount) - restList.Min(x => x.RestOnWorkingDayCount) <= 2) {
				
				Console.WriteLine();
				Console.WriteLine("{0, -8} : {1, 10} {2, 10} {3, 10}", "Name", "RestWDay", "RestHoliday", "Sum");
				foreach (var p in restList) {
					Console.WriteLine("{0, -8} : {1, 10} {2, 10} {3, 10}", p.Name,
						 p.RestOnWorkingDayCount,
						 p.RestOnHolidayCount,
						 p.RestOnHolidayCount + p.RestOnWorkingDayCount);
				}
				return true;
			}
			return false;
		}
		
		public void Fill() {
			if (IsDone()) {
				if (!FairEnough()) return;
				ShowResult();
				return;
			}
			var currentDate = new DateTime(Year, Month, GetFirstDayToFill());
			var erTireness = 10;
			var wardTireness = 5;
			var OPDTireness = 4;
			if (currentDate.IsHoliday()) {
				erTireness = (erTireness * 3) / 2;
				wardTireness = (wardTireness * 3) / 2;
			}
			
			var wayne = new Wayne(currentDate);
			var rnd = new Random();
			var doctorListCopy = _doctorList
				.Where(x => !x.AbsenceList.Contains(currentDate))
				.Where(x => !x.AmInWayneMoreThanTwoConsecutiveDays(currentDate))
				.OrderBy(x => (x.ERWayne.Count() + x.WardWayne.Count() + x.OPDWayne.Count()) / x.Factor)
				.ThenBy(x => x.Tireness / x.Factor)
				.ThenBy(x => rnd.Next())
				.Take(currentDate.NeedOPD() ? 4 : 3)
				.ToList();
			foreach (var erDoctor in doctorListCopy.Where(d => d.AmIOKForERThisDay(currentDate))) {
				wayne.ERDoctor = erDoctor;
				erDoctor.ERWayne.Add(currentDate);
				erDoctor.Tireness += erTireness;
				foreach (var wardDoctor in doctorListCopy.Where(x => x != erDoctor)) {
					wayne.WardDoctor = wardDoctor;
					wardDoctor.WardWayne.Add(currentDate);
					wardDoctor.Tireness += wardTireness;
					if (!currentDate.NeedOPD()) {
						var wayneTableCopy = Copy();
						if (wayneTableCopy.AddWayneIfAcceptable(wayne)) {
							wayneTableCopy.Fill();
						}
					}
					else {
						foreach (var OPDDoctor in doctorListCopy.Where(x => x != erDoctor && x != wardDoctor)) {
							wayne.OPDDoctor = OPDDoctor;
							OPDDoctor.OPDWayne.Add(currentDate);
							OPDDoctor.Tireness += OPDTireness;
							var wayneTableCopy = Copy();
							if (wayneTableCopy.AddWayneIfAcceptable(wayne)) {
								wayneTableCopy.Fill();
							}
							OPDDoctor.Tireness -= OPDTireness;
							OPDDoctor.OPDWayne.Remove(currentDate);
						}
					}
					wardDoctor.Tireness -= wardTireness;
					wardDoctor.WardWayne.Remove(currentDate);
				}
				erDoctor.Tireness -= erTireness;
				erDoctor.ERWayne.Remove(currentDate);
			}
		}
	}	
}
