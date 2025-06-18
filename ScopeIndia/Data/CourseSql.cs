using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Data.SqlClient;
using ScopeIndia.Models;

namespace ScopeIndia.Data
{
    public class CourseSql : ICourse
    {
        private readonly string Scopestring;

        public CourseSql(IConfiguration configuration)
        {
            Scopestring = configuration.GetConnectionString("Scopestring");
        }
        public void Insert(DashboardModel dashboard)
        {
            using (SqlConnection connection = new SqlConnection(Scopestring))
            {
                connection.Open();
                string InsertQuery = "INSERT INTO Coursetable (CourseName,Duration,Fee) VALUES (@CourseName,@CourseDuration,@CourseFee)";
                using (SqlCommand command = new SqlCommand(InsertQuery, connection))
                {
                    command.Parameters.AddWithValue("@CourseName", dashboard.CourseName);
                    command.Parameters.AddWithValue("@Duration", dashboard.Duration);
                    command.Parameters.AddWithValue("@Fee", dashboard.Fee);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }

        }

        public CourseModel GetById(int courseId)
        {
            CourseModel course = null;

            string query = "SELECT CourseId, CourseName, Fee, Duration FROM Coursetable WHERE CourseId = @CourseId";

            using (SqlConnection conn = new SqlConnection(Scopestring))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseId", courseId);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        course = new CourseModel
                        {
                            CourseId = reader["CourseId"] != DBNull.Value ? (int)reader["CourseId"] : 0,
                            CourseName = reader["CourseName"] != DBNull.Value ? reader["CourseName"].ToString() : string.Empty,
                            Fee = reader["Fee"] != DBNull.Value ? Convert.ToInt32(reader["Fee"]) : 0,
                            Duration = reader["Duration"] != DBNull.Value ? reader["Duration"].ToString() : string.Empty
                        };
                    }
                }
            }

            return course;
        }
        public List<CourseModel> GetAll()
        {
            List<CourseModel> courses = new List<CourseModel>();

            using (SqlConnection connection = new SqlConnection(Scopestring))
            {
                connection.Open();
                string query = "SELECT * FROM Coursetable";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        
                        while (reader.Read())
                        {
                            var course = new CourseModel
                            {
                                CourseId = reader.GetInt32(0),
                                CourseName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Duration = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Fee = reader.GetInt32(3)
                                

                            };

                            courses.Add(course);
                        }
                    }
                }

            }
            return courses;
        }
    }

}

