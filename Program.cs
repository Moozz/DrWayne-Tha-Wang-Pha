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
			var kao = new Doctor("Kao", DoctorExperience.Intern1);
			var golf = new Doctor("Golf", DoctorExperience.Intern1);
			
			var year = 2015;
			var month = 8;
			
			nui.RegisterAbsence(year, month, new List<int> { 5, 6 });
			ja.RegisterAbsence(year, month, new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 25, 26, 27, 28 });
			bean.RegisterAbsence(year, month, new List<int> {2, 9, 10, 16, 17, 18, 19, 20, 21, 22 });
			did.RegisterAbsence(year, month, new List<int> { 11, 12, 13, 14, 16 });
			kao.RegisterAbsence(year, month, new List<int> { 1, 2, 15, 16 });
			
			_doctorList = new List<Doctor> { did, nui, ja, bean, kao, golf };
			
			DateTimeExtension.AddSpecialHoliday(new DateTime(year, month, 12));
			
			var wayneTable = new WayneTable(_doctorList, year, month);
			Solve(wayneTable);
        }

		public static void Solve(WayneTable wayneTable) {
			if (wayneTable.IsDone()) {
				if (!wayneTable.FairEnough()) {
					return;
				}
				Console.WriteLine("{0}", wayneTable);
				Console.WriteLine("Tireness level at the end of the month");
				Console.WriteLine(string.Join("\n", _doctorList.Select(x => x.Name + " : " + x.Tireness)));
				Console.ReadLine();
				return;
			}
			var currentDate = new DateTime(wayneTable.Year, wayneTable.Month, wayneTable.GetLastWayneDay() + 1);
			var erTireness = 10;
			var wardTireness = 5;
			var OPDTireness = 4;
			if (currentDate.IsHoliday()) {
				erTireness = (erTireness * 3) / 2;
				wardTireness = (wardTireness * 3) / 2;
			}
			
			var wayne = new Wayne(currentDate);
			var rnd = new Random();
			var doctorListCopy = _doctorList.Where(x => !x.AbsenceList.Contains(currentDate))
											.OrderBy(x => x.Tireness / x.Factor)
											.ThenBy(x => rnd.Next())
											.ToList();					
			foreach (var erDoctor in doctorListCopy) {
				wayne.ERDoctor = erDoctor;
				erDoctor.ERWayne.Add(wayne);
				erDoctor.Tireness += erTireness;
				foreach (var wardDoctor in doctorListCopy.Where(x => x != erDoctor)) {
					wayne.WardDoctor = wardDoctor;
					wardDoctor.WardWayne.Add(wayne);
					wardDoctor.Tireness += wardTireness;
					if (!currentDate.NeedOPD()) {
						var wayneTableCopy = wayneTable.Copy();
						if (wayneTableCopy.AddWayneIfAcceptable(wayne)) {
							Solve(wayneTableCopy);
						}
					}
					else {
						foreach (var OPDDoctor in doctorListCopy.Where(x => x != erDoctor && x != wardDoctor)) {
							wayne.OPDDoctor = OPDDoctor;
							OPDDoctor.OPDWayne.Add(wayne);
							OPDDoctor.Tireness += OPDTireness;
							var wayneTableCopy = wayneTable.Copy();
							if (wayneTableCopy.AddWayneIfAcceptable(wayne)) {
								Solve(wayneTableCopy);
							}
							OPDDoctor.Tireness -= OPDTireness;
							OPDDoctor.OPDWayne.Remove(wayne);
						}
					}
					wardDoctor.Tireness -= wardTireness;
					wardDoctor.WardWayne.Remove(wayne);
				}
				erDoctor.Tireness -= erTireness;
				erDoctor.ERWayne.Remove(wayne);
			}
		}
    }
}
