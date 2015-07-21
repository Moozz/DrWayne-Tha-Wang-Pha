using System;

namespace DrWayne {
	public class Wayne {
		public Wayne(DateTime wayneDate) {
			WayneDate = wayneDate;
		}
		
		public DateTime WayneDate { get; set; }
		
		public Doctor ERDoctor { get; set; }
		
		public Doctor WardDoctor { get; set; }
		
		public Doctor OPDDoctor { get; set; }
		
		public bool IsAcceptable () {
			return  ERDoctor != null && WardDoctor != null &&
					(ERDoctor.Exp != DoctorExperience.Intern1 || ERDoctor.Exp != WardDoctor.Exp) &&
					(WayneDate.DayOfWeek != DayOfWeek.Saturday || OPDDoctor != null);
		}
	}
}