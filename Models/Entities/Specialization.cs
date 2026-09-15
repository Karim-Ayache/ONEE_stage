namespace ONEE_Stage.Models.Entities
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}