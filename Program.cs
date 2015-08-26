using System;
using System.Collections.Generic;

namespace DrWayne {
    public class Program {
		private static List<Doctor> _doctorList;
        public static void Main(string[] args) {
            var did = new Doctor("P' Did", DoctorExperience.Oldie, 0.6);
			var nui = new Doctor("P' Nui", DoctorExperience.Oldie);
			var ja = new Doctor("Ja+", DoctorExperience.Intern3);
			var bean = new Doctor("Bean", DoctorExperience.Intern3);
			var gofilm = new Doctor("Gofilm", DoctorExperience.Intern1);
			var saran = new Doctor("Saran", DoctorExperience.Intern1);
			
			var year = 2015;
			var month = 9;
			
			nui.RegisterAbsence(year, month, new List<int> { 11, 12, 13 });
			ja.RegisterAbsence(year, month, new List<int> {  });
			bean.RegisterAbsence(year, month, new List<int> {25, 26, 27 });
			did.RegisterAbsence(year, month, new List<int> {  });
			gofilm.RegisterAbsence(year, month, new List<int> { 12, 13, 19, 20 });
			saran.RegisterAbsence(year, month, new List<int> {  });
			
			_doctorList = new List<Doctor> { bean, ja, gofilm, saran, nui, did };
			
			//DateTimeExtension.AddSpecialHoliday(new DateTime(year, month, 12));
			
			Console.WriteLine("Thinking...");
			var wt = new WayneTable(_doctorList, year, month);
			
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 5), ja, gofilm, did));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 6), nui, ja));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 12), bean, saran, did));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 13), did, bean));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 19), bean, nui, saran));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 20), saran, bean));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 26), ja, nui, gofilm));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 27), gofilm, ja));
			wt.Fill();
        }
    }
}
