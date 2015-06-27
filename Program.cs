using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DrWayne {
    public class Program {
		private static List<Doctor> _doctorList;
        public static void Main(string[] args) {
            var did = new Doctor("P' Did", DoctorExperience.Oldie, 0.4);
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
			
			_doctorList = new List<Doctor> { did, nui, ja, bean, pua, golf };
			
			Solve(1, new WayneTable());
			
			//  using (var t = Task.Run(() => Solve(1, doctorList, new WayneTable()))) {
			//  	t.Wait();
			//  	Console.WriteLine(t.Status);
			//  }
        }

		public static void Solve(int day, WayneTable wayneTable) {
			if (day >= 31) {
				Console.WriteLine("{0}", wayneTable);
				Console.WriteLine("Tireness level at the end of the month");
				Console.WriteLine(string.Join("\n", _doctorList.Select(x => x.Name + " : " + x.Tireness)));
				Console.ReadLine();
				return;
			}
			var currentDate = new DateTime(2015, 7, day);
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
