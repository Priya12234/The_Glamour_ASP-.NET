using System;
using System.ComponentModel.DataAnnotations.Schema;
using Npgsql;

namespace The_Glamour.Models
{
    public class Appointments
    {
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Service { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Details { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [NotMapped] // This tells Entity Framework to ignore this property
        public string TimeString { get; set; }

        public static string ConnectionString { get; set; }

        // Create new appointment
        public bool Create()
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            try
            {

                conn.Open();

                string query = @"INSERT INTO appointments 
                        (userid, name, service, date, time, details, contact_email, contact_phone, created_at, updated_at)
                        VALUES (@uid, @n, @s, @d, @t, @det, @ce, @cp, @ca, @ua)
                        RETURNING appointmentid"; // Important for getting the ID back

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("uid", UserId);
                cmd.Parameters.AddWithValue("n", Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("s", Service ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("d", Date);
                cmd.Parameters.AddWithValue("t", Time);
                cmd.Parameters.AddWithValue("det", Details ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("ce", ContactEmail ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("cp", ContactPhone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("ca", CreatedAt);
                cmd.Parameters.AddWithValue("ua", UpdatedAt);

                // Execute and get the returned ID
                var id = cmd.ExecuteScalar();
                if (id != null)
                {
                    AppointmentId = Convert.ToInt32(id);
                    return true;
                }
                return false;
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"PostgreSQL Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        // Get all appointments
        public static List<Appointments> GetAll()
        {
            var appointments = new List<Appointments>();

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string query = "SELECT * FROM appointments";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                appointments.Add(new Appointments
                {
                    AppointmentId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Service = reader.GetString(3),
                    Date = reader.GetDateTime(4),
                    Time = reader.GetTimeSpan(5),
                    Details = reader.IsDBNull(6) ? null : reader.GetString(6),
                    ContactEmail = reader.GetString(7),
                    ContactPhone = reader.GetString(8),
                    CreatedAt = reader.GetDateTime(9),
                    UpdatedAt = reader.GetDateTime(10)
                });
            }

            return appointments;
        }

        // Get appointments by user ID
        public static List<Appointments> GetByUserId(int userId)
        {
            var appointments = new List<Appointments>();

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string query = "SELECT * FROM appointments WHERE userid = @uid";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("uid", userId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                appointments.Add(new Appointments
                {
                    AppointmentId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Service = reader.GetString(3),
                    Date = reader.GetDateTime(4),
                    Time = reader.GetTimeSpan(5),
                    Details = reader.IsDBNull(6) ? null : reader.GetString(6),
                    ContactEmail = reader.GetString(7),
                    ContactPhone = reader.GetString(8),
                    CreatedAt = reader.GetDateTime(9),
                    UpdatedAt = reader.GetDateTime(10)
                });
            }

            return appointments;
        }

        // Get single appointment by ID
        public static Appointments GetById(int id)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string query = "SELECT * FROM appointments WHERE appointmentid = @id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Appointments
                {
                    AppointmentId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Service = reader.GetString(3),
                    Date = reader.GetDateTime(4),
                    Time = reader.GetTimeSpan(5),
                    Details = reader.IsDBNull(6) ? null : reader.GetString(6),
                    ContactEmail = reader.GetString(7),
                    ContactPhone = reader.GetString(8),
                    CreatedAt = reader.GetDateTime(9),
                    UpdatedAt = reader.GetDateTime(10)
                };
            }

            return null;
        }

        // Update appointment
        public bool Update()
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string query = @"UPDATE appointments 
                            SET name = @n, service = @s, date = @d, time = @t, 
                                details = @det, contact_email = @ce, contact_phone = @cp, 
                                updated_at = @ua
                            WHERE appointmentid = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("n", Name);
            cmd.Parameters.AddWithValue("s", Service);
            cmd.Parameters.AddWithValue("d", Date);
            cmd.Parameters.AddWithValue("t", Time);
            cmd.Parameters.AddWithValue("det", Details ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("ce", ContactEmail);
            cmd.Parameters.AddWithValue("cp", ContactPhone);
            cmd.Parameters.AddWithValue("ua", DateTime.Now);
            cmd.Parameters.AddWithValue("id", AppointmentId);

            return cmd.ExecuteNonQuery() > 0;
        }

        // Delete appointment
        public static bool Delete(int id)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string query = "DELETE FROM appointments WHERE appointmentid = @id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}