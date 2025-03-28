using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProCareMvc.Database.Entity
{
    public class Appointment:BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [ForeignKey(nameof(Patient))]
        public Guid PatienId { get; set; }

        [ForeignKey(nameof(Doctor))]
        public Guid DoctorId { get; set; }

        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
