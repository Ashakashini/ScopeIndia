using ScopeIndia.Models;

namespace ScopeIndia.Data
{
    public interface IStudent
    {
        void Insert(StudentModel student);

        //void Delete(int Id);
        void Update(StudentModel student);

        //StudentModel GetByToken(string token);
        StudentModel GetById(int Id);


        StudentModel GetByEmail(string Email);

        List<StudentModel> GetAll();
    }
}
