using Microsoft.Data.SqlClient;
using Org.BouncyCastle.Crypto.Generators;
using ScopeIndia.Models;
using System.Data;
using System.Diagnostics.Metrics;

namespace ScopeIndia.Data
{
    public class StudentSql : IStudent
    {
        private readonly string Scopestring;

        public StudentSql(IConfiguration configuration)
        {
            Scopestring = configuration.GetConnectionString("Scopestring");
        }

        public void Insert(StudentModel student)
        {
            using (SqlConnection connection = new SqlConnection(Scopestring))
            {
                connection.Open();
                string InsertQuery = "INSERT INTO Studenttable (FirstName,LastName,Gender,DateOfBirth,Email,PhoneNumber,Country,State,City,Hobbies,Avatar,Password) VALUES (@FirstName,@LastName,@Gender,@DateOfBirth,@Email,@PhoneNumber,@Country,@State,@City,@Hobbies,@Avatar,@Password)";
                using (SqlCommand command = new SqlCommand(InsertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", student.StudentFirstName);
                    command.Parameters.AddWithValue("@LastName", student.StudentLastName);
                    command.Parameters.AddWithValue("@Gender", student.Gender);
                    command.Parameters.AddWithValue("@DateOfBirth", student.StudentDateOfBirth);
                    command.Parameters.AddWithValue("@Email", student.StudentEmail);
                    command.Parameters.AddWithValue("@PhoneNumber", student.StudentPhoneNumber);
                    command.Parameters.AddWithValue("@Country", student.StudentCountry);
                    command.Parameters.AddWithValue("@State", student.StudentState);
                    command.Parameters.AddWithValue("@City", student.StudentCity);
                    command.Parameters.AddWithValue("@Hobbies", student.StudentHobbies);
                    command.Parameters.AddWithValue("@Avatar", string.IsNullOrEmpty(student.Avatarpath) ? DBNull.Value : student.Avatarpath);
                    command.Parameters.AddWithValue("@Password", string.IsNullOrEmpty(student.password) ? DBNull.Value : student.password);
                    //command.Parameters.AddWithValue("@CourseId", student.CourseId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }

        }
        public StudentModel GetById(int id)
        {
            StudentModel student = new StudentModel();

            using (SqlConnection conn = new SqlConnection(Scopestring))
            {
                conn.Open();
                string selquery = "SELECT * FROM StudentTable WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(selquery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id",id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            student.Id = reader.GetInt32(0);
                            student.StudentFirstName = reader.IsDBNull(1) ? null : reader.GetString(1);
                            student.StudentLastName = reader.IsDBNull(2) ? null : reader.GetString(2);
                            student.Gender = reader.IsDBNull(3) ? null : reader.GetString(3);
                            student.StudentDateOfBirth = reader.GetDateTime(4);
                            student.StudentEmail = reader.IsDBNull(5) ? null : reader.GetString(5);
                            student.StudentPhoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6);
                            student.StudentCountry = reader.IsDBNull(7) ? null : reader.GetString(7);
                            student.StudentState = reader.IsDBNull(8) ? null : reader.GetString(8);
                            student.StudentCity = reader.IsDBNull(9) ? null : reader.GetString(9);
                            student.StudentHobbies = reader.IsDBNull(10) ? null : reader.GetString(10);
                            student.Avatarpath = reader.IsDBNull(11) ? null : reader.GetString(11);
                            student.password = reader.IsDBNull(12) ? null : reader.GetString(12);
                            student.CourseId = reader.IsDBNull(13) ? null : reader.GetInt32(13);
                        }

                    }
                }
                conn.Close();
            }
            return student;
        }

        //public void Update(StudentModel student,PasswordSetting password)
        //{


        //    using (SqlConnection conn = new SqlConnection(Scopestring))
        //    {
        //        string query = "UPDATE Users SET PasswordHash = @PasswordHash WHERE Id = @UserId";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);
        //        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
        //        cmd.Parameters.AddWithValue("@UserId", 1); // Or pass ID dynamically

        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //}


        //public void Update(StudentModel student)
        //{
        //    using (SqlConnection conn = new SqlConnection(Scopestring))
        //    {
                
        //        string query = "UPDATE Student SET NewPassword = @Password WHERE Email = @Email";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(student.password);
        //        cmd.Parameters.AddWithValue("@Password", student.password);
        //        cmd.Parameters.AddWithValue("@Email", student.StudentEmail); // or use Id if you prefer

        //        conn.Open();
        //        int rowsAffected = cmd.ExecuteNonQuery(); // Execute the query and get the number of rows affected
        //        if (rowsAffected == 0)
        //        {
        //            // Log or handle when no rows are updated (email might not exist in the database)
        //            Console.WriteLine("No rows were updated. Check if the email exists in the database.");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Password updated successfully for student: " + student.StudentEmail);
        //        }
        //    }
        //}



        public void Update(StudentModel student)
        {
            using (SqlConnection conn = new SqlConnection(Scopestring))
            {
                conn.Open();
                string query = "UPDATE Studenttable SET FirstName=@FirstName,LastName=@LastName,Gender=@Gender,DateOfBirth=@DateOfBirth,Email=@Email,PhoneNumber=@PhoneNumber,Country=@Country,State=@State,City=@City,Hobbies=@Hobbies,Avatar=@Avatar,Password=@Password,CourseId=@CourseId WHERE Id=@Id";
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    command.Parameters.AddWithValue("@Id", student.Id);
                    command.Parameters.AddWithValue("@FirstName", student.StudentFirstName);
                    command.Parameters.AddWithValue("@LastName", student.StudentLastName);
                    command.Parameters.AddWithValue("@Gender", student.Gender);
                    command.Parameters.AddWithValue("@DateOfBirth", student.StudentDateOfBirth);
                    command.Parameters.AddWithValue("@Email", student.StudentEmail);
                    command.Parameters.AddWithValue("@PhoneNumber", student.StudentPhoneNumber);
                    command.Parameters.AddWithValue("@Country", student.StudentCountry);
                    command.Parameters.AddWithValue("@State", student.StudentState);
                    command.Parameters.AddWithValue("@City", student.StudentCity);
                    command.Parameters.AddWithValue("@Hobbies", student.StudentHobbies);
                    command.Parameters.AddWithValue("@Avatar", string.IsNullOrEmpty(student.Avatarpath) ? DBNull.Value : student.Avatarpath);
                    command.Parameters.AddWithValue("@Password", string.IsNullOrEmpty(student.password) ? DBNull.Value : student.password);
                    command.Parameters.AddWithValue("@CourseId", ((object)student.CourseId) ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        public void UpdateCourseId(int studentId, int courseId)
        {
            using (SqlConnection conn = new SqlConnection(Scopestring))
            {
                conn.Open();
                string query = "UPDATE Studenttable SET CourseId = @CourseId WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@CourseId", courseId);
                    command.Parameters.AddWithValue("@Id", studentId);

                    command.ExecuteNonQuery();
                }
            }
        }



        public List<StudentModel> GetAll()
        {
            List<StudentModel> students = new List<StudentModel>();

            using (SqlConnection conn = new SqlConnection(Scopestring))
            {
                conn.Open();

                string selquery = "SELECT * FROM Studenttable"; 

                using (SqlCommand cmd = new SqlCommand(selquery, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentModel student = new StudentModel();

                            student.Id = reader.GetInt32(0);
                            student.StudentFirstName = reader.IsDBNull(1) ? null : reader.GetString(1);
                            student.StudentLastName = reader.IsDBNull(2) ? null : reader.GetString(2);
                            student.Gender = reader.IsDBNull(3) ? null : reader.GetString(3);
                            student.StudentDateOfBirth = reader.GetDateTime(4);
                            student.StudentEmail = reader.IsDBNull(5) ? null : reader.GetString(5);
                            student.StudentPhoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6);
                            student.StudentCountry = reader.IsDBNull(7) ? null : reader.GetString(7);
                            student.StudentState = reader.IsDBNull(8) ? null : reader.GetString(8);
                            student.StudentCity = reader.IsDBNull(9) ? null : reader.GetString(9);
                            student.StudentHobbies = reader.IsDBNull(10) ? null : reader.GetString(10);
                            student.Avatarpath = reader.IsDBNull(11) ? null : reader.GetString(11);
                            student.password = reader.IsDBNull(12) ? null : reader.GetString(12);
                            student.CourseId = reader.IsDBNull(13) ? (int?)null : reader.GetInt32(13);

                            students.Add(student);
                        }
                    }
                }
                conn.Close();
            }
            return students;
        }

        public StudentModel GetByEmail(string Email)
        {
            StudentModel student = new StudentModel();
            try
            {
                using (SqlConnection conn = new SqlConnection(Scopestring))
                {
                    conn.Open();
                    string sqlquery = "SELECT * FROM StudentTable WHERE Email=@Email";
                    using (SqlCommand cmd = new SqlCommand(sqlquery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", Email);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                student.Id = reader.GetInt32(0);
                                student.StudentFirstName = reader.IsDBNull(1) ? null : reader.GetString(1);
                                student.StudentLastName = reader.IsDBNull(2) ? null : reader.GetString(2);
                                student.Gender = reader.IsDBNull(3) ? null : reader.GetString(3);
                                student.StudentDateOfBirth = reader.GetDateTime(4);
                                student.StudentEmail = reader.IsDBNull(5) ? null : reader.GetString(5);
                                student.StudentPhoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6);
                                student.StudentCountry = reader.IsDBNull(7) ? null : reader.GetString(7);
                                student.StudentState = reader.IsDBNull(8) ? null : reader.GetString(8);
                                student.StudentCity = reader.IsDBNull(9) ? null : reader.GetString(9);
                                student.StudentHobbies = reader.IsDBNull(10) ? null : reader.GetString(10);
                                student.Avatarpath = reader.IsDBNull(11) ? null : reader.GetString(11);
                                student.password = reader.IsDBNull(12) ? null : reader.GetString(12);
                                student.CourseId = reader.IsDBNull(12) ? null : reader.GetInt32(13);
                            }

                        }
                    }
                    conn.Close();
                }
            }
            catch(Exception Ex)
            {

            }
            return student;
        }

        public void AssignCourseToStudent(int studentId, int courseId)
        {
            using (SqlConnection conn = new SqlConnection(Scopestring))
            {
                string query = "UPDATE Student SET CourseId = @CourseId WHERE StudentId = @StudentId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@CourseId", courseId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<StudentModel> GetStudents()
        {
            List<StudentModel> students = new List<StudentModel>();

            using (SqlConnection conn = new SqlConnection(Scopestring))
            {
                conn.Open();
                string query = "SELECT * FROM StudentTable";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new StudentModel
                            {
                                Id = reader.GetInt32(0),
                                StudentFirstName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                StudentLastName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Gender = reader.IsDBNull(3) ? null : reader.GetString(3),
                                StudentDateOfBirth = reader.GetDateTime(4),
                                StudentEmail = reader.IsDBNull(5) ? null : reader.GetString(5),
                                StudentPhoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6),
                                StudentCountry = reader.IsDBNull(7) ? null : reader.GetString(7),
                                StudentState = reader.IsDBNull(8) ? null : reader.GetString(8),
                                StudentCity = reader.IsDBNull(9) ? null : reader.GetString(9),
                                StudentHobbies = reader.IsDBNull(10) ? null : reader.GetString(10),
                                Avatarpath = reader.IsDBNull(11) ? null : reader.GetString(11),
                                password = reader.IsDBNull(12) ? null : reader.GetString(12),
                                CourseId = reader.IsDBNull(13) ? null : reader.GetInt32(13)
                            });
                        }
                    }
                }
                conn.Close();
            }

            return students;
        }
        public bool IsEmailExists(string email)
            {
            bool exists = false;

            using (SqlConnection conn = new SqlConnection(Scopestring))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM studenttable WHERE Email=@Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    exists = (int)cmd.ExecuteScalar() > 0; // If count > 0, email exists
                }
            }
            return exists;
        }
    }

}
    

