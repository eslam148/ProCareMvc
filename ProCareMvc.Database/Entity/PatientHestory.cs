﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class PatientHestory
    {


        public Guid Record_ID { get; set; }
        public string Treatment { get; set; }

        public string Diagnosis { get; set; }

        public string Medication { get; set; }

        public DateTime VisitDate { get; set; }

        public DateTime NextAppointment { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

     

    }
}
