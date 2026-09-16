namespace ONEE_Stage.Models.Entities
{
    public class Employee : User
    {
        public string RegistrationNumber { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;
    }
}