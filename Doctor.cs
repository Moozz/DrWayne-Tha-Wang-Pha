using System;
using System.Collections.Generic;

namespace DrWayne {
    public enum DoctorExperience {
        Intern1,
        Intern2,
        Intern3,
        Oldie
    }
    
    public class Doctor {       
        public Doctor(string name, DoctorExperience exp) {
            Name = name;
            Exp = exp;
            AbsenceList = new HashSet<DateTime>();
            Tireness = 0;
        }
        
        public string Name { get; private set; }
        public DoctorExperience Exp { get; private set; }
        public HashSet<DateTime> AbsenceList { get; private set; }
        public int Tireness { get; set; }
        
        public void RegisterAbsence(DateTime date) {
            AbsenceList.Add(date);
        }
    }
}