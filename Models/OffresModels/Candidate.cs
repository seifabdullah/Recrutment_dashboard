using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recrutement_plateforme.Models.OffresModels
{
    public class Candidate
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public String Phone { get; set; }

        [Url(ErrorMessage = "Invalid LinkedIn URL.")]
        public string LinkedInUrl { get; set; }



        // Navigation property representing a collection of job applications for the candidate
        public ICollection<CandidateJob> Applications { get; set; }
        public ICollection<Job> Jobs { get; set; }
        public ICollection<CandidateJob> CandidateJobs { get; set; }
        public CompleteProfile CompleteProfile { get; set; }
    }
}