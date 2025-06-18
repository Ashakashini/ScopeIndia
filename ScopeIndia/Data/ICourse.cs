using ScopeIndia.Models;

namespace ScopeIndia.Data
{
    public interface ICourse
    {
        void Insert(DashboardModel dashboard);

        List<CourseModel> GetAll();
    }
   
}
