using System;
using System.Collections.Generic;

namespace DrWayne {
	public class DrWaynceRunner {
		
		enum WayneType {
			WARD,
			ER,
			OPD
		}
		
		public async void Run() {
			
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
			
			var doctorList = new List<Doctor> { did, nui, ja, bean, pua, golf };
			
		}
	}
}