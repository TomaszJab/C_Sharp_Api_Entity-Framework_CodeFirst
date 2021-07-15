using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.DTOs.Responses
{
    public class GetPrescriptionsResponseDto
    {
        public int IdPrescription { get; set; }

        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        public IEnumerable Medicaments { get; set; }
    }
}
