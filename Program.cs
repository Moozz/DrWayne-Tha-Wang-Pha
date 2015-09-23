using System;
using System.Collections.Generic;

namespace DrWayne {
    public class Program {
		private static List<Doctor> _doctorList;
        public static void Main(string[] args) {
            var did = new Doctor("P' Did", DoctorExperience.Oldie, 0.7);
			var nui = new Doctor("P' Nui", DoctorExperience.Oldie);
			var ja = new Doctor("Ja+", DoctorExperience.Intern3);
			var bean = new Doctor("Bean", DoctorExperience.Intern3);
			var gofilm = new Doctor("Gofilm", DoctorExperience.Intern1, 0.7);
			var saran = new Doctor("Saran", DoctorExperience.Intern1, 0.7);
			
			var year = 2015;
			var month = 10;
			
			nui.RegisterAbsence(year, month, new List<int> { 13 });
			ja.RegisterAbsence(year, month, new List<int> { 22, 23, 24, 25 });
			bean.RegisterAbsence(year, month, new List<int> {25, 26, 27 });
			did.RegisterAbsence(year, month, new List<int> {  });
			gofilm.RegisterAbsence(year, month, new List<int> { 11, 12, 13, 14, 15, 16, 17, 18, 19, 30, 31 });
			saran.RegisterAbsence(year, month, new List<int> { 18, 19, 20, 21, 22, 23, 24, 25, 26, 30, 31 });
			
			_doctorList = new List<Doctor> { bean, ja, gofilm, saran, nui, did };
			
			//DateTimeExtension.AddSpecialHoliday(new DateTime(year, month, 12));
			
			Console.WriteLine("Thinking...");
			var wt = new WayneTable(_doctorList, year, month);
			
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 3), gofilm, nui, bean));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 4), saran, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 10), gofilm, ja, saran));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 11), ja, saran));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 17), saran, did, ja));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 18), ja, did));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 23), did, gofilm, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 24), bean, did, gofilm));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 25), nui, bean));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 31), nui, bean, did));
			wt.Fill();
        }
    }
}
