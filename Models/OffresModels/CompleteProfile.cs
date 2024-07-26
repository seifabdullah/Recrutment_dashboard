namespace Recrutement_plateforme.Models.OffresModels
{
    public class CompleteProfile
    {
        public int CandidateId { get; set; }

        // Additional properties for a more complete profile
        public int Id { get; set; }
        public string Experience { get; set; }
        public string Education { get; set; }
        public string Skills { get; set; }
        public string Competencies { get; set; }
        public string Languages { get; set; }
        public string Hobbies { get; set; }

        public Candidate Candidate { get; set; }
    }
}
