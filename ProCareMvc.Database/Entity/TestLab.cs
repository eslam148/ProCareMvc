using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class TestLab
    {
        [Key]
        public Guid Id { get; set; }

        public string Result { get; set; }

        [ForeignKey(nameof(Lab))]
        public Guid LabId { get; set; }

        [ForeignKey(nameof(patientHestory))]
        public Guid PatientHestoryId { get; set; }

        public Lab Lab { get; set; }
        public PatientHestory patientHestory { get; set; }

    }
}
