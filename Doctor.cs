using System;
using System.Collections.Generic;

namespace MyNamespace
{
    public enum DoctorExperience
    {
        INTERN1,
        INTERN2,
        INTERN3,
        OLDIE
    }
    
    public class Doctor {       
        public Doctor(string name, DoctorExperience exp) {
            Name = name;
            Exp = exp;
            Tireness = 0;
        }
        
        public string Name { get; private set; }
        public DoctorExperience Exp { get; private set; }
        private int Tireness { get; set; }
        
        public void RegisterAbsence(ICollection<DateTime> dateList) {
            
        }        
    }
}