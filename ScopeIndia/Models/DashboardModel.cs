namespace ScopeIndia.Models
{
    public class DashboardModel
    {
        public string Id { get; set; }
        public string CourseName { get; set; }
        public string Duration { get; set; }
        public int Fee { get; set; }

        public StudentModel Student { get; set; }
        public List<CourseModel> Courses { get; set; }

    }
}
