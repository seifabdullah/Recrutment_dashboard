using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recrutement_plateforme.Models.OffresModels
{
    public class Job
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "CompetencesRequired is required.")]
        public string CompetencesRequired { get; set; }

        [Required(ErrorMessage = "Responsibilities are required.")]
        public string Responsibilities { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative value.")]
        public float Salary { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime PublishedAt { get; set; }

        public bool? IsAccepted { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "RecruteurId is required.")]
        public int RecruteurId { get; set; }

        // Navigation property for the Recruteur entity
        public Recruteur Recruteur { get; set; }

        // Collection of candidates who applied for the job
        public ICollection<CandidateJob> Candidates { get; set; }
    }
}