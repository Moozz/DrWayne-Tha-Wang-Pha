using System;

namespace DrWayne {
	public class Wayne {
		public enum Type {
			ER,
			Ward,
			OPD
		}
		
		public Wayne(DateTime wayneDate, Doctor erDoctor = null, Doctor wardDoctor = null, Doctor opdDoctor = null) {
			WayneDate = wayneDate;
			ERDoctor = erDoctor;
			WardDoctor = wardDoctor;
			OPDDoctor = opdDoctor;
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