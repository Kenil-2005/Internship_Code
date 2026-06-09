using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;

namespace InLineSQL;

class Program
{
    static readonly string connStr =
        "Data Source=10.61.18.8;Integrated Security=True;Pooling=False;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Name=vscode-mssql;Application Intent=ReadWrite;Command Timeout=30";

    public static void Main(string[] args)
    {
        bool running = true;
        do
        {
            Console.WriteLine("\n1. Insert into user table.");
            Console.WriteLine("2. Update into user table.");
            Console.WriteLine("3. Delete from user table.");
            Console.WriteLine("4. Get all data from user table.");
            Console.WriteLine("5. Update into user table(dis-connected).");
            Console.WriteLine("6. Get all data from user table(dis-connected).");
            Console.WriteLine("7. exit\n");

            Console.WriteLine("Select your choice: ");
            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    InsertData();
                    break;
                case "2":
                    UpdateData();
                    break;
                case "3":
                    DeleteData();
                    break;
                case "4":
                    GetAllData();
                    break;
                case "5":
                    UpdateDataDisConnected();
                    break;
                case "6":
                    ViewDataDisConnected();
                    break;
                case "7":
                    Console.WriteLine("Exiting...\n");
                    running = false;
                    break;
                default:
                    Console.WriteLine("Please select valid choice(1-8).\n");
                    break;
            }
        } while (running);
    }

    static void InsertData()
    {
        try
        {
            Console.WriteLine("Enter user id: ");
            if (!int.TryParse(Console.ReadLine(), out int inputId))
            {
                Console.WriteLine("Please enter valide id.\n");
                return;
            }

            Console.WriteLine("Enter user first Name: ");
            string inputFirstName = Console.ReadLine() ?? "";
            if (String.IsNullOrWhiteSpace(inputFirstName))
            {
                Console.WriteLine("Please enter valid first name.\n");
                return;
            }

            Console.WriteLine("Enter user Last Name: ");
            string inputLastName = Console.ReadLine() ?? "";
            if (String.IsNullOrWhiteSpace(inputLastName))
            {
                Console.WriteLine("Please enter valid last name.\n");
                return;
            }

            Console.WriteLine("Enter user email: ");
            string inputEmail = Console.ReadLine() ?? "";
            if (String.IsNullOrWhiteSpace(inputEmail))
            {
                Console.WriteLine("Please enter valid email");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                Console.WriteLine("Connection Opened...");
                string query =
                    "insert into [dbo].[user_khushi](user_id,first_name,last_name,email) values(@inputId,@inputFirstName,@inputLastName,@inputEmail)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@inputId", inputId);
                    cmd.Parameters.AddWithValue("@inputFirstName", inputFirstName);
                    cmd.Parameters.AddWithValue("@inputLastName", inputLastName);
                    cmd.Parameters.AddWithValue("@inputEmail", inputEmail);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("User inserted successfully!");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database Error: {ex.Message}");
        }
    }

    static void UpdateData()
    {
        try
        {
            Console.WriteLine("Enter user id to update: ");
            if (!int.TryParse(Console.ReadLine(), out int inputId))
            {
                Console.WriteLine("Please enter valide id.\n");
                return;
            }

            Console.WriteLine("Enter new user first Name: ");
            string inputFirstName = Console.ReadLine() ?? "";
            if (String.IsNullOrWhiteSpace(inputFirstName))
            {
                Console.WriteLine("Please enter valid first name.\n");
                return;
            }

            Console.WriteLine("Enter new user Last Name: ");
            string inputLastName = Console.ReadLine() ?? "";
            if (String.IsNullOrWhiteSpace(inputLastName))
            {
                Console.WriteLine("Please enter valid last name.\n");
                return;
            }

            Console.WriteLine("Enter new user email: ");
            string inputEmail = Console.ReadLine() ?? "";
            if (String.IsNullOrWhiteSpace(inputEmail))
            {
                Console.WriteLine("Please enter valid Marks");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                Console.WriteLine("Connection Opened...");
                string query =
                    "update [dbo].[user_khushi] set first_name = @inputFirstName, last_name = @inputLastName, email = @inputEmail where user_id = @inputId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@inputId", inputId);
                    cmd.Parameters.AddWithValue("@inputFirstName", inputFirstName);
                    cmd.Parameters.AddWithValue("@inputLastName", inputLastName);
                    cmd.Parameters.AddWithValue("@inputEmail", inputEmail);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("User updated successfully!");
                    }
                    else
                    {
                        Console.WriteLine("No user found with that ID. No changes made.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database Error: {ex.Message}");
        }
    }

    static void DeleteData()
    {
        try
        {
            Console.WriteLine("Enter user id to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int inputId))
            {
                Console.WriteLine("Please enter valide id.\n");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                Console.WriteLine("Connection Opened...");
                string query = "Delete from [dbo].[user_khushi] where user_id = @inputId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@inputId", inputId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("User deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("No user found with that ID. Nothing was deleted.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database Error: {ex.Message}");
        }
    }

    static void GetAllData()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                Console.WriteLine("Connection Opened...");
                string query = "Select * From [dbo].[user_khushi]";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(
                            $"ID: {reader["user_id"]} | First Name: {reader["first_name"]} | Last Name: {reader["last_name"]} | Email: {reader["email"]}"
                        );
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database Error: {ex.Message}");
        }
    }

    static void UpdateDataDisConnected()
    {
        try
        {
            Console.WriteLine("Enter user id to update: ");
            if (!int.TryParse(Console.ReadLine(), out int inputId))
            {
                Console.WriteLine("Please enter valide id.\n");
                return;
            }

            Console.WriteLine("Enter new user first Name: ");
            string inputFirstName = Console.ReadLine() ?? "";
            if (String.IsNullOrWhiteSpace(inputFirstName))
            {
                Console.WriteLine("Please enter valid first name.\n");
                return;
            }

            Console.WriteLine("Enter new user Last Name: ");
            string inputLastName = Console.ReadLine() ?? "";
            if (String.IsNullOrWhiteSpace(inputLastName))
            {
                Console.WriteLine("Please enter valid last name.\n");
                return;
            }

            Console.WriteLine("Enter new user email: ");
            string inputEmail = Console.ReadLine() ?? "";
            if (String.IsNullOrWhiteSpace(inputEmail))
            {
                Console.WriteLine("Please enter valid Marks");
                return;
            }

            string query = "select * from [dbo].[user_khushi]";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                DataSet ds = new DataSet();

                adapter.Fill(ds, "[dbo].[user_khushi]");

                DataTable dt = ds.Tables["[dbo].[user_khushi]"] ?? new();

                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row["user_id"]) == inputId)
                    {
                        row["first_name"] = inputFirstName;
                        row["last_name"] = inputLastName;
                        row["email"] = inputEmail;
                    }
                }

                adapter.Update(ds, "[dbo].[user_khushi]");
                Console.WriteLine("Data Updated successfully.\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database Error: {ex.Message}");
        }
    }

    static void ViewDataDisConnected()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                Console.WriteLine("Connection Opened...");

                string query = "select * from [dbo].[user_khushi]";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                DataSet ds = new DataSet();

                adapter.Fill(ds, "[dbo].[user_khushi]");

                DataTable dt = ds.Tables["[dbo].[user_khushi]"] ?? new();

                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine(
                        $"ID: {row["user_id"]} | First Name: {row["first_name"]} | Last Name: {row["last_name"]} | Email: {row["email"]}"
                    );
                }

                // adapter.Update(ds, "[dbo].[user_khushi]");
                Console.WriteLine("Data Updated successfully.\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database Error: {ex.Message}");
        }
    }
}
