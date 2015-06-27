using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DrWayne {
    public class Program {
        public static void Main(string[] args) {
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
			var doctorList = new List<Doctor> { did, nui, ja, bean, pua, golf }.Select(x => new KeyValuePair<Doctor, int>(x, 0)).ToList();
			
			
			Solve(1, doctorList, new WayneTable());
			
			//  using (var t = Task.Run(() => Solve(1, doctorList, new WayneTable()))) {
			//  	t.Wait();
			//  	Console.WriteLine(t.Status);
			//  }
        }

		public static void Solve(int day, List<KeyValuePair<Doctor, int>> tirenessState, WayneTable wayneTable) {
			if (day >= 31) {
				Console.WriteLine("{0}", wayneTable);
				Console.ReadLine();
				return;
			}
			var currentDate = new DateTime(2015, 7, day);
			var isSaturday = currentDate.DayOfWeek == DayOfWeek.Saturday;			
			var wayne = new Wayne(currentDate);
			var rnd = new Random();
			var tirenessStateCopy = tirenessState.Where(x => !x.Key.AbsenceList.Contains(currentDate))
												.OrderBy(x => rnd.Next())
												.ToList();
			
			foreach (var erDoctor in tirenessStateCopy) {
				wayne.ERDoctor = erDoctor.Key;
				foreach (var wardDocter in tirenessStateCopy.Where(x => x.Key != erDoctor.Key)) {
					wayne.WardDoctor = wardDocter.Key;
					if (!isSaturday) {
						var wayneTableCopy = wayneTable.Copy();
						if (wayneTableCopy.AddWayneIfAcceptable(wayne)) {
							Solve(day + 1, tirenessState, wayneTableCopy);
						}
					}
					else {
						foreach (var OPDDoctor in tirenessStateCopy.Where(x => x.Key != erDoctor.Key && x.Key != wardDocter.Key)) {
							wayne.OPDDoctor = OPDDoctor.Key;
							var wayneTableCopy = wayneTable.Copy();
							if (wayneTableCopy.AddWayneIfAcceptable(wayne)) {
								Solve(day + 1, tirenessState, wayneTableCopy);
							}
						}
					}
				}
			}
		}
    }
}
