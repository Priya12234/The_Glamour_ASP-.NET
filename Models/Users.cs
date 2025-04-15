using System.Collections.Generic;
using Npgsql;

namespace The_Glamour.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Password { get; set; }

        public static string ConnectionString { get; set; }

        // ---------------- Register ----------------
        public bool Register()
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string query = "INSERT INTO users (email, name, number, password) VALUES (@e, @n, @num, @p)";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("e", Email);
            cmd.Parameters.AddWithValue("n", Name);
            cmd.Parameters.AddWithValue("num", Number);
            cmd.Parameters.AddWithValue("p", Password); // Store plain password

            return cmd.ExecuteNonQuery() > 0;
        }

        // ---------------- Login ----------------
        public static Users Login(string email, string password)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string query = "SELECT userid, email, name, number, password FROM users WHERE email = @e";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("e", email);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string storedPassword = reader.GetString(4);

                // Compare passwords directly (no hashing)
                if (password == storedPassword)
                {
                    return new Users
                    {
                        UserId = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        Name = reader.GetString(2),
                        Number = reader.GetString(3),
                        Password = storedPassword
                    };
                }
            }

            return null;
        }

        // ---------------- Get All Users ----------------
        public static List<Users> GetAll()
        {
            var list = new List<Users>();
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string query = "SELECT userid, email, name, number, password FROM users";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Users
                {
                    UserId = reader.GetInt32(0),
                    Email = reader.GetString(1),
                    Name = reader.GetString(2),
                    Number = reader.GetString(3),
                    Password = reader.GetString(4)
                });
            }

            return list;
        }

        // ---------------- Get One User ----------------
        public static Users GetById(int id)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string query = "SELECT userid, email, name, number, password FROM users WHERE userid = @id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Users
                {
                    UserId = reader.GetInt32(0),
                    Email = reader.GetString(1),
                    Name = reader.GetString(2),
                    Number = reader.GetString(3),
                    Password = reader.GetString(4)
                };
            }

            return null;
        }

        // ---------------- Update ----------------
        public bool Update()
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string query = "UPDATE users SET email = @e, name = @n, number = @num, password = @p WHERE userid = @id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("e", Email);
            cmd.Parameters.AddWithValue("n", Name);
            cmd.Parameters.AddWithValue("num", Number);
            cmd.Parameters.AddWithValue("p", Password);
            cmd.Parameters.AddWithValue("id", UserId);

            return cmd.ExecuteNonQuery() > 0;
        }

        // ---------------- Delete ----------------
        public static bool Delete(int id)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string query = "DELETE FROM users WHERE userid = @id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}