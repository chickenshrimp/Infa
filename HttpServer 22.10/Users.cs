using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace HttpsSteam
{
    [HttpController("users")]
    internal class Users
    {
        [HttpGET("")]
        public User GetUser(int id)
        {
            List<User> users = new List<User>();
            users.Add(new User() { Id = 1, name = "Ivan" });

            return users.FirstOrDefault(t => t.Id == id);
        }
        [HttpGET("")]
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDB;Integrated Security = True";
            string sqlExpression = "SELECT * FROM [dbo].[Table]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            name = reader.GetString(1)
                        });
                    }
                }
                reader.Close();
            }
            return users;
        } 
    }
    internal class User
    {
        public int Id { get; set; }
        public string name { get; set; }
    }
}
