using System;
using System.Collections.Generic;

namespace DrWayne {
    public class Program {
		private static List<Doctor> _doctorList;
        public static void Main(string[] args) {
            var did = new Doctor("P' Did", DoctorExperience.Oldie, 6, 4, 0.7);
			var nui = new Doctor("P' Nui", DoctorExperience.Oldie, 6, 6);
			var ja = new Doctor("Ja+", DoctorExperience.Intern3, 5, 5);
			var bean = new Doctor("Bean", DoctorExperience.Intern3, 5, 6);
			var bum = new Doctor("Bum", DoctorExperience.Oldie, 5, 6);
			var fang = new Doctor("Fang", DoctorExperience.Intern1, 6, 5);
			_doctorList = new List<Doctor> { bean, ja, bum, fang, nui, did };
			
			var year = 2016;
			var month = 1;
			//DateTimeExtension.AddSpecialHoliday(new DateTime(year, month, 31));
			
			bum.RegisterAbsence(year, month, new List<int> { 11, 14, 15, 16, 17 });
			ja.RegisterAbsence(year, month, 15, 17);
			bean.RegisterAbsence(year, month, new List<int>{ 8, 9, 10, 16, 17 });
            bean.RegisterAbsence(year, month, 16, 17);
			did.RegisterAbsence(year, month, 22, 27);
			nui.RegisterAbsence(year, month, 26, 31);
			fang.RegisterAbsence(year, month, 19, 26);
			
			var wt = new WayneTable(_doctorList, year, month);
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 9), bum, ja, fang));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 10), ja, fang));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 16), nui, fang, did));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 17), did, nui));			
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 23), ja, bean, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 24), bean, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 30), bean, bum, did));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 31), fang, bum));
			
			Console.WriteLine("Thinking...");
			wt.Fill();
        }
    }
}
