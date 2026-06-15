using System.Net.Http.Headers;
using Api_Model.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;

// 1. Initialize the Web Application Builder inside the console environment
var builder = WebApplication.CreateBuilder(args);

// 2. Register required API Exploration and Swagger generator services
builder.Services.AddEndpointsApiExplorer(); // Required for Minimal APIs
builder.Services.AddSwaggerGen(); // Generates the Swagger JSON documentation

var app = builder.Build();

if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.MapGet("/api/hello", () => new { Message = "Hello World!" });

string ConnStr =
    @"Data Source=10.61.18.8;Integrated Security=True;Pooling=False;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Name=vscode-mssql;Application Intent=ReadWrite;Command Timeout=30";

app.MapGet(
    "/api/user",
    async () =>
    {
        List<User> userList = new List<User>();

        try
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                string query =
                    "SELECT user_id, first_name, last_name, email FROM [dbo].[user_khushi]";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Add each database row as an object into our list
                            userList.Add(
                                new User
                                {
                                    id = Convert.ToInt32(reader["user_id"]),
                                    first_name = reader["first_name"].ToString() ?? "",
                                    last_name = reader["last_name"].ToString() ?? "",
                                    email = reader["email"].ToString() ?? "",
                                }
                            );
                        }
                    }
                }
            }

            // Return the list directly. ASP.NET automatically turns this into clean JSON.
            return Results.Ok(userList);
        }
        catch (Exception ex)
        {
            // If the database fails, return an HTTP 500 error code with the message
            return Results.Problem($"DataBase Error: {ex.Message}");
        }
    }
);

app.MapPut("/api/user/put", async () => { });

app.Urls.Add("http://localhost:5000");

Console.WriteLine("Server starting... Open http://localhost:5000/swagger");
app.Run();
