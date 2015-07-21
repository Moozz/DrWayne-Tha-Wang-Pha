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
        public Doctor(string name, DoctorExperience exp, double factor = 1) {
            Name = name;
            Exp = exp;
            AbsenceList = new HashSet<DateTime>();
            Tireness = 0;
            Factor = factor;
        }
        
        public string Name { get; private set; }
        public DoctorExperience Exp { get; private set; }
        public HashSet<DateTime> AbsenceList { get; private set; }
        public int Tireness { get; set; }
        
        public double Factor { get; private set; }
        
        public void RegisterAbsence(DateTime date) {
            AbsenceList.Add(date);
        }
        
        public void RegisterAbsence(int year, int month, List<int> day) {
            day.Select(d => new DateTime(year, month, d))
                .ToList()
                .ForEach(d => AbsenceList.Add(d));
        }
    }
}