using System;
using System.Collections.Generic;
using System.Linq;

namespace DrWayne {
    public enum DoctorExperience {
        Intern1,
        Intern2,
        Intern3,
        Oldie
    }
    
    public class Doctor {       
        public Doctor(string name, DoctorExperience exp, int maxEr = 99, int maxWard = 99, double factor = 1) {
            Name = name;
            Exp = exp;
            AbsenceList = new HashSet<DateTime>();
            ERWayne = new HashSet<DateTime>();
            WardWayne = new HashSet<DateTime>();
            OPDWayne = new HashSet<DateTime>();
            Tireness = 0;
            Factor = factor;
            MaximumERWayne = maxEr;
            MaximumWardWayne = maxWard;
        }
        
        public string Name { get; private set; }
        public DoctorExperience Exp { get; private set; }
        public HashSet<DateTime> AbsenceList { get; private set; }
        public int Tireness { get; set; }
        
        public HashSet<DateTime> ERWayne { get; private set; }
        public HashSet<DateTime> WardWayne { get; private set; }
        public HashSet<DateTime> OPDWayne { get; private set; }
        
        public double Factor { get; private set; }

        public int MaximumERWayne { get; private set; }
        public int MaximumWardWayne { get; private set; }
        
        public void RegisterAbsence(DateTime date) {
            AbsenceList.Add(date);
        }
        
        public void RegisterAbsence(int year, int month, List<int> day) {
            day.Select(d => new DateTime(year, month, d))
                .ToList()
                .ForEach(d => AbsenceList.Add(d));
        }
        
        public void RegisterAbsence(int year, int month, int startDate, int endDate) {
            RegisterAbsence(year, month, Enumerable.Range(startDate, endDate-startDate + 1).ToList());
        }
        
        public bool AmIOKForERThisDay(DateTime d) {
            var yesterday = d.AddDays(-1);
            var tomorrow = d.AddDays(1);
            return ERWayne.Count() < MaximumERWayne && !ERWayne.Contains(yesterday) && !ERWayne.Contains(tomorrow);
        }
        
        public bool AmIOKForWardThisDay(DateTime d) {
            return WardWayne.Count() < MaximumWardWayne;
        }
        
		public bool AmInWayneMoreThanTwoConsecutiveDays(DateTime d) {
            var theDayBeforeYesterday   = d.AddDays(-2);
            var yesterday               = d.AddDays(-1);
            var tomorrow                = d.AddDays(1);
            var theDayAfterTomorrow     = d.AddDays(2);
            var wayneDay = GetAllWayneDate();
            return (wayneDay.Contains(theDayBeforeYesterday) && wayneDay.Contains(yesterday)) ||
                   (wayneDay.Contains(yesterday) && wayneDay.Contains(tomorrow)) ||
                   (wayneDay.Contains(tomorrow) && wayneDay.Contains(theDayAfterTomorrow));
		}
        
        public List<DateTime> GetAllWayneDate() {
            return ERWayne.Concat(WardWayne).Concat(OPDWayne).Distinct().ToList();
        }
    }
}