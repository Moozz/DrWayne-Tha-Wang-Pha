using System;
using System.Collections.Generic;

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
			kao.RegisterAbsence(year, month, new List<int> { 1, 2, 14, 15, 16, 31 });
			golf.RegisterAbsence(year, month, new List<int> { 31 });
			
			_doctorList = new List<Doctor> { bean, ja, golf, kao, nui, did };
			
			DateTimeExtension.AddSpecialHoliday(new DateTime(year, month, 12));
			
			Console.WriteLine("Thinking...");
			var wt = new WayneTable(_doctorList, year, month);
			
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 1), bean, did, golf));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 2), golf, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 8), did, bean, kao));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 9), ja, kao));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 12), nui, golf, ja));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 15), bean, ja, nui));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 16), nui, golf));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 22), kao, ja, did));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 23), ja, did));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 29), golf, kao, bean));
			wt.AddFixWayne(new Wayne(new DateTime(year, month, 30), kao, bean));
			
			wt.Fill();
        }
    }
}
