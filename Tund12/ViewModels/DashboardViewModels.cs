using Tund12.Models;

namespace Tund12.ViewModels
{
    public class StudentDashboardViewModel
    {
        public int ActiveTrainingCount { get; set; }
        public int UpcomingTrainingCount { get; set; }
        public int PendingRegistrationCount { get; set; }
        public List<Registration> ActiveTrainings { get; set; } = new();
        public List<Training> RecommendedTrainings { get; set; } = new();
    }

    public class TeacherDashboardViewModel
    {
        public string TeacherName { get; set; } = string.Empty;
        public int ActiveTrainingCount { get; set; }
        public int UpcomingTrainingCount { get; set; }
        public int TotalStudents { get; set; }
        public int PendingRegistrationCount { get; set; }
        public List<Training> ActiveTrainings { get; set; } = new();
    }

    public class AdminDashboardViewModel
    {
        public int TotalCourses { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalTrainings { get; set; }
        public int TotalStudents { get; set; }
        public int PendingRegistrations { get; set; }
    }
}