using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recrutement_plateforme.Models.OffresModels
{
    public class Recruteur
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        public string CompanyName { get; set; }

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
        // Collection of jobs associated with the recruiter
        public ICollection<Job> Jobs { get; set; }
    }
}