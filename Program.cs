using System;
using System.Collections.Generic;

namespace DrWayne {
    public class Program {
		private static List<Doctor> _doctorList;
        public static void Main(string[] args) {
            var did = new Doctor("P' Did", DoctorExperience.Oldie, 3, 4, 0.7);
			var nui = new Doctor("P' Nui", DoctorExperience.Oldie, 4, 7, 0.9);
			var ja = new Doctor("Ja+", DoctorExperience.Intern3, 6, 4);
			var bean = new Doctor("Bean", DoctorExperience.Intern3, 6, 4);
			var ohm = new Doctor("Ohm", DoctorExperience.Intern1, 5, 5);
			var tong = new Doctor("Tong", DoctorExperience.Intern1, 5, 5);
			
			var year = 2015;
			var month = 11;
			
			nui.RegisterAbsence(year, month, 19, 22);
			ja.RegisterAbsence(year, month, new List<int> { 6, 7, 8, 9, 20, 21, 22 });
			bean.RegisterAbsence(year, month, 14, 21);
			did.RegisterAbsence(year, month, new List<int> { 14, 16, 17, 18, 22, 26, 27 });
			ohm.RegisterAbsence(year, month, 6, 8);
			tong.RegisterAbsence(year, month, new List<int> { });
			
			_doctorList = new List<Doctor> { bean, ja, ohm, tong, nui, did };
			
			//DateTimeExtension.AddSpecialHoliday(new DateTime(year, month, 12));
			
			Console.WriteLine("Thinking...");
			var wt = new WayneTable(_doctorList, year, month);
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 1), ja, bean));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 7), tong, nui, bean));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 8), nui, tong));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 14), ja, nui, ohm));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 15), ohm, ja));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 16), nui, tong));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 17), ja, tong));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 18), nui, ohm));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 19), ja, tong));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 20), ohm, did));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 21), did, ohm, tong));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 22), bean, tong));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 28), bean, ohm, ja));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 29), ja, bean));
			wt.Fill();
        }
    }
}
