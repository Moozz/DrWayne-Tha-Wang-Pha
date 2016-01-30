using System;
using System.Collections.Generic;

namespace DrWayne {
    public class Program {
		private static List<Doctor> _doctorList;
        public static void Main(string[] args) {
            var did = new Doctor("P' Did", DoctorExperience.Oldie, 4, 5);
			var nui = new Doctor("P' Nui", DoctorExperience.Oldie, 5, 5);
			var ja = new Doctor("Ja+", DoctorExperience.Intern3, 5, 5);
			var bean = new Doctor("Bean", DoctorExperience.Intern3, 5, 5);
			var bum = new Doctor("Bum", DoctorExperience.Oldie, 5, 5);
			var fang = new Doctor("Fang", DoctorExperience.Intern1, 5, 5);
			_doctorList = new List<Doctor> { bean, ja, bum, fang, nui, did };
			
			var year = 2016;
			var month = 2;
			DateTimeExtension.AddSpecialHoliday(new DateTime(year, month, 22));
			
			bum.RegisterAbsence(year, month, 17, 22);
			ja.RegisterAbsence(year, month, new List<int> {19, 26, 27, 28});
			bean.RegisterAbsence(year, month, 5, 8);
			did.RegisterAbsence(year, month, 21, 22);
			nui.RegisterAbsence(year, month, new List<int> {5,6,7,12,13,14});
			fang.RegisterAbsence(year, month, 25, 28);
			
			var wt = new WayneTable(_doctorList, year, month);
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 6), bum, did, fang));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 7), fang, bum));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 13), ja, bean, bum));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 14), did, fang));			
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 20), nui, fang, ja));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 21), bean, ja));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 22), ja, nui, bean));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 27), bean, did, nui));
            wt.AddFixWayne(new Wayne(new DateTime(year, month, 28), nui, bum));
			
			Console.WriteLine("Thinking...");
			wt.Fill();
        }
    }
}
