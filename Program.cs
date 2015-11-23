using System;
using System.Collections.Generic;

namespace DrWayne {
    public class Program {
		private static List<Doctor> _doctorList;
        public static void Main(string[] args) {
            var did = new Doctor("P' Did", DoctorExperience.Oldie, 99, 99, 0.7);
			var nui = new Doctor("P' Nui", DoctorExperience.Oldie);
			var ja = new Doctor("Ja+", DoctorExperience.Intern3);
			var bean = new Doctor("Bean", DoctorExperience.Intern3);
			var ohm = new Doctor("Ohm", DoctorExperience.Intern1);
			var tong = new Doctor("Tong", DoctorExperience.Intern1);
			_doctorList = new List<Doctor> { bean, ja, ohm, tong, nui, did };
			
			var year = 2015;
			var month = 12;
			DateTimeExtension.AddSpecialHoliday(new DateTime(year, month, 7));
			DateTimeExtension.AddSpecialHoliday(new DateTime(year, month, 10));
			DateTimeExtension.AddSpecialHoliday(new DateTime(year, month, 31));
			
			nui.RegisterAbsence(year, month, new List<int> { 5, 6, 7, 30, 31 });
			ja.RegisterAbsence(year, month, 4, 8);
			bean.RegisterAbsence(year, month, 6, 7);
			//did.RegisterAbsence(year, month, new List<int> { 14, 16, 17, 18, 22, 26, 27 });
			ohm.RegisterAbsence(year, month, 11, 13);
			tong.RegisterAbsence(year, month, 11, 13);
			
			var wt = new WayneTable(_doctorList, year, month);
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 5), bean, tong, did));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 6), ohm, did));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 7), tong, ohm, did));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 10), ja, bean, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 12), bean, ja, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 13), ja, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 19), tong, ohm, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 20), ohm, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 26), nui, did, ja));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 27), did, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 31), tong, bean, ohm));
			
			Console.WriteLine("Thinking...");
			wt.Fill();
        }
    }
}
