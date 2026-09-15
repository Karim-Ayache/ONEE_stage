namespace ONEE_Stage.Models.Entities
{
    public class Employee : User
    {
        public string RegistrationNumber { get; set; } = string.Empty; // Matricule ONEE
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}