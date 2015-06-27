using System;
using System.Collections.Generic;
using System.Linq;

namespace DrWayne {
	public class DrWaynceRunner {
		
		enum WayneType {
			WARD,
			ER,
			OPD
		}
		
		static List<Doctor> _doctorList;
		
		public static void Initialize() {
			var did = new Doctor("P' Did", DoctorExperience.Oldie);
			var nui = new Doctor("P' Nui", DoctorExperience.Oldie);
			var ja = new Doctor("Ja+", DoctorExperience.Intern3);
			var bean = new Doctor("Bean", DoctorExperience.Intern3);
			var pua = new Doctor("Pua", DoctorExperience.Intern1);
			var golf = new Doctor("Golf", DoctorExperience.Intern1);
			
			nui.RegisterAbsence(DateTime.Parse("13 Jul 2015"));
			nui.RegisterAbsence(DateTime.Parse("22 Jul 2015"));
			nui.RegisterAbsence(DateTime.Parse("23 Jul 2015"));
			nui.RegisterAbsence(DateTime.Parse("24 Jul 2015"));
			
			ja.RegisterAbsence(DateTime.Parse("18 Jul 2015"));
			ja.RegisterAbsence(DateTime.Parse("19 Jul 2015"));
			ja.RegisterAbsence(DateTime.Parse("31 Jul 2015"));
			
			pua.RegisterAbsence(DateTime.Parse("31 Jul 2015"));
			
			did.RegisterAbsence(DateTime.Parse("18 Jul 2015"));
			did.RegisterAbsence(DateTime.Parse("20 Jul 2015"));
			did.RegisterAbsence(DateTime.Parse("21 Jul 2015"));
			did.RegisterAbsence(DateTime.Parse("22 Jul 2015"));
			did.RegisterAbsence(DateTime.Parse("23 Jul 2015"));
			
			var _doctorList = new List<Doctor> { did, nui, ja, bean, pua, golf }.Select(x => new KeyValuePair<Doctor, int>(x, 0)).ToList();
		}

		public static async void Run() {
			Initialize();
		}
		
		public static void Solve(int day, List<KeyValuePair<Doctor, int>> tirenessState, WayneTable wayneTable) {
			if (wayneTable.IsDone()) {
				Console.WriteLine(wayneTable);
				return;
			}
			var currentDate = new DateTime(2015, 7, day);
			var isSaturday = currentDate.DayOfWeek == DayOfWeek.Saturday;			
			
			var wayne = new Wayne(currentDate);
			foreach (var erDoctor in tirenessState) {
				wayne.ERDoctor = erDoctor.Key;
				foreach (var wardDocter in tirenessState.Where(x => x.Key != erDoctor.Key)) {
					wayne.WardDoctor = wardDocter.Key;
					if (!isSaturday) {
						var wayneTableCopy = wayneTable.Copy();
						if (wayneTableCopy.AddWayneIfAcceptable(wayne))
							Solve(day + 1, tirenessState, wayneTableCopy);	
					}
					else {
						foreach (var OPDDoctor in tirenessState.Where(x => x.Key != erDoctor.Key && x.Key != wardDocter.Key)) {
							wayne.OPDDoctor = OPDDoctor.Key;
							var wayneTableCopy = wayneTable.Copy();
							if (wayneTableCopy.AddWayneIfAcceptable(wayne))
								Solve(day + 1, tirenessState, wayneTableCopy);	
						}
					}
				}
			}
		}
	}
}