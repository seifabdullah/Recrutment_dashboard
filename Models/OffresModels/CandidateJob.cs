using Recrutement_plateforme.Enum;

namespace Recrutement_plateforme.Models.OffresModels
{
    public class CandidateJob
    {
        // Foreign key for the Candidate entity
        public int CandidateId { get; set; }

        // Navigation property for the Candidate entity
        public Candidate Candidate { get; set; }

        // Foreign key for the Job entity
        public int JobId { get; set; }

        // Navigation property for the Job entity
        public Job Job { get; set; }

        // Status of the job application (e.g., Pending, Accepted, Rejected)
        public ApplicationStatus Status { get; set; }
    }
}