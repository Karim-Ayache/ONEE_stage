namespace ONEE_Stage.Models.Enums
{
    public enum UserRole
    {
        Student = 0,    // External candidates applying for stages
        Supervisor = 1, // Encadrant (mentors/supervises interns)
        HRManager = 2,  // RH ONEE (manages offers & validates applications)
        Admin = 3
    }

    public enum InternshipType
    {
        Initiation,
        Advanced, // Perfectionnement
        PFA,      // Projet de Fin d'Année
        PFE       // Projet de Fin d'Études
    }

    public enum WorkMode
    {
        OnSite,   // Présentiel
        Remote,   // Distanciel
        Hybrid    // Hybride
    }

    public enum WorkPace
    {
        FullTime, // Temps plein
        PartTime  // Temps partiel
    }

    public enum DocumentType
    {
        CV,
        CoverLetter,
        InternshipReport,
        CertificateOfEnrollment,
        Agreement, // Convention
        Other
    }

    public enum ApplicationStatus
    {
        Draft,
        Submitted,
        Scored,
        InterviewScheduled,
        Preselected,
        Accepted,
        Ongoing,
        Completed,
        Closed,
        Rejected
    }

    public enum OfferStatus
    {
        Draft,
        Published,
        Closed,
        Archived
    }
}