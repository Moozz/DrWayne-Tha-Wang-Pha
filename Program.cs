using System;
using System.Collections.Generic;
using System.Linq;

namespace DrWayne {
    public class Program {
		private static List<Doctor> _doctorList;
        public static void Main(string[] args) {
            var did = new Doctor("P' Did", DoctorExperience.Oldie);
			var nui = new Doctor("P' Nui", DoctorExperience.Oldie);
			var ja = new Doctor("Ja+", DoctorExperience.Intern3);
			var bean = new Doctor("Bean", DoctorExperience.Intern3);
			var nong1 = new Doctor("nong1", DoctorExperience.Intern1);
			var golf = new Doctor("Golf", DoctorExperience.Intern1);
			
			var year = 2015;
			var month = 8;
			
			nui.RegisterAbsence(DateTime.Parse("5 Aug 2015"));
			nui.RegisterAbsence(DateTime.Parse("6 Aug 2015"));
			
			ja.RegisterAbsence(DateTime.Parse("1 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("2 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("3 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("4 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("5 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("6 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("7 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("8 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("9 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("25 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("26 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("27 Aug 2015"));
			ja.RegisterAbsence(DateTime.Parse("28 Aug 2015"));
			
			bean.RegisterAbsence(DateTime.Parse("2 Aug 2015"));
			bean.RegisterAbsence(DateTime.Parse("9 Aug 2015"));
			bean.RegisterAbsence(DateTime.Parse("10 Aug 2015"));
			bean.RegisterAbsence(DateTime.Parse("16 Aug 2015"));
			bean.RegisterAbsence(DateTime.Parse("17 Aug 2015"));
			bean.RegisterAbsence(DateTime.Parse("18 Aug 2015"));
			bean.RegisterAbsence(DateTime.Parse("19 Aug 2015"));
			bean.RegisterAbsence(DateTime.Parse("20 Aug 2015"));
			bean.RegisterAbsence(DateTime.Parse("21 Aug 2015"));
			bean.RegisterAbsence(DateTime.Parse("22 Aug 2015"));
			
			did.RegisterAbsence(DateTime.Parse("11 Jul 2015"));
			did.RegisterAbsence(DateTime.Parse("12 Jul 2015"));
			did.RegisterAbsence(DateTime.Parse("13 Jul 2015"));
			did.RegisterAbsence(DateTime.Parse("14 Jul 2015"));
			did.RegisterAbsence(DateTime.Parse("16 Jul 2015"));
			
			_doctorList = new List<Doctor> { did, nui, ja, bean, nong1, golf };
			
			Solve(1, new WayneTable(year, month));
        }

		public static void Solve(int day, WayneTable wayneTable) {
			if (wayneTable.IsDone()) {
				Console.WriteLine("{0}", wayneTable);
				Console.WriteLine("Tireness level at the end of the month");
				Console.WriteLine(string.Join("\n", _doctorList.Select(x => x.Name + " : " + x.Tireness)));
				Console.ReadLine();
				return;
			}
			var currentDate = new DateTime(wayneTable.Year, wayneTable.Month, day);
			var isSaturday = currentDate.DayOfWeek == DayOfWeek.Saturday;			
			var wayne = new Wayne(currentDate);
			var rnd = new Random();
			var doctorListCopy = _doctorList.Where(x => !x.AbsenceList.Contains(currentDate))
											.OrderBy(x => x.Tireness / x.Factor)
											.ThenBy(x => rnd.Next())
											.ToList();
			foreach (var erDoctor in doctorListCopy) {
				wayne.ERDoctor = erDoctor;
				erDoctor.Tireness += 10;
				foreach (var wardDocter in doctorListCopy.Where(x => x != erDoctor)) {
					wayne.WardDoctor = wardDocter;
					wardDocter.Tireness += 5;
					if (!isSaturday) {
						var wayneTableCopy = wayneTable.Copy();
						if (wayneTableCopy.AddWayneIfAcceptable(wayne)) {
							Solve(day + 1, wayneTableCopy);
						}
					}
					else {
						foreach (var OPDDoctor in doctorListCopy.Where(x => x != erDoctor && x != wardDocter)) {
							wayne.OPDDoctor = OPDDoctor;
							OPDDoctor.Tireness += 5;
							var wayneTableCopy = wayneTable.Copy();
							if (wayneTableCopy.AddWayneIfAcceptable(wayne)) {
								Solve(day + 1, wayneTableCopy);
							}
							OPDDoctor.Tireness -= 5;
						}
					}
					wardDocter.Tireness -= 5;
				}
				erDoctor.Tireness -= 10;
			}
		}
    }
}
