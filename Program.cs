using System.ComponentModel.Design;
using Employees.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapSwagger();
app.MapSwaggerUI();

app.MapGet(
    "/api/users",
    static async (int? id, ILogger<Program> logger) =>
    {
        try
        {
            string connStr =
                @"Data Source=10.61.18.8;Integrated Security=True;Pooling=False;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Name=vscode-mssql;Application Intent=ReadWrite;Command Timeout=30";

            List<Employee> employeesList = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query =
                    "SELECT E.ID, D.Name, E.FirstName, E.LastName, E.Designation, E.Salary, E.DepartmentID, E.isDeleted "
                    + "FROM [dbo].[Employee_5] as E "
                    + "JOIN [dbo].[Department_5] as D ON E.DepartmentID = D.ID "
                    + "WHERE (@Id IS NULL OR @Id = 0 OR E.ID = @Id)"
                    + "AND E.isDeleted = 0";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (id == null || id == 0)
                    {
                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
                    }

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employeesList.Add(
                                new Employee
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    first_name = reader["FirstName"].ToString() ?? "",
                                    last_name = reader["LastName"].ToString() ?? "",
                                    designation = reader["Designation"].ToString() ?? "",
                                    salary = Convert.ToInt32(reader["Salary"]),
                                    department_id = Convert.ToInt32(reader["DepartmentId"]),
                                    // is_delete = Convert.ToInt32(reader["isDeleted"]),
                                    department_name = reader["Name"].ToString() ?? "",
                                }
                            );
                        }
                    }
                }
            }
            if (employeesList.Count == 0)
            {
                return Results.NotFound(
                    new Response
                    {
                        message = "No active employees found matching the criteria.",
                        status = false,
                    }
                );
            }
            return Results.Ok(new Response { status = true, data = employeesList });
        }
        catch (Exception ex)
        {
            return Results.Problem($"DataBase Error: {ex.Message}");
        }
    }
);

app.MapPost(
    "api/users/post",
    async (PostEmployee postEmployee) =>
    {
        try
        {
            string connStr =
                @"Data Source=10.61.18.8;Integrated Security=True;Pooling=False;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Name=vscode-mssql;Application Intent=ReadWrite;Command Timeout=30";

            if (String.IsNullOrWhiteSpace(postEmployee.first_name))
            {
                return Results.BadRequest(
                    new Response { message = "First name is required fields.", status = false }
                );
            }
            if (String.IsNullOrWhiteSpace(postEmployee.last_name))
            {
                return Results.BadRequest(
                    new Response { message = "Last name is required fields.", status = false }
                );
            }

            if (String.IsNullOrWhiteSpace(postEmployee.designation))
            {
                return Results.BadRequest(
                    new Response { message = "Designation is required fields.", status = false }
                );
            }
            if (postEmployee.department_id <= 0)
            {
                return Results.BadRequest(
                    new Response
                    {
                        message = "A valid department identifier must be supplied.",
                        status = false,
                    }
                );
            }
            if (postEmployee.salary <= 0)
            {
                return Results.BadRequest(
                    new Response { message = "Salary must be greater than 0.", status = false }
                );
            }

            // if(!int.TryParse(postEmployee.department_id),out int)

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                await conn.OpenAsync();
                string checkQuery =
                    "SELECT COUNT(1) FROM [dbo].[Employee_5]  WHERE FirstName = @first_name AND LastName = @last_name AND DepartmentID = @department_id  AND isDeleted = 0";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@first_name", postEmployee.first_name.Trim());
                    checkCmd.Parameters.AddWithValue("@last_name", postEmployee.last_name.Trim());
                    checkCmd.Parameters.AddWithValue("@department_id", postEmployee.department_id);

                    int count = Convert.ToInt32(await checkCmd.ExecuteScalarAsync() ?? 0);

                    if (count > 0)
                    {
                        // Returns 409 Conflict to signal a duplicate entry constraint violation
                        return Results.Conflict(
                            new Response
                            {
                                message =
                                    $"An active employee named '{postEmployee.first_name} {postEmployee.last_name}' in department {postEmployee.department_id} already exists.",
                                status = false,
                            }
                        );
                    }
                }

                string query =
                    @"insert into [dbo].[Employee_5] ( FirstName, LastName, Designation, Salary, DepartmentID, isDeleted)"
                    + "values (@first_name,@last_name,@designation,@salary,@department_id,0)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // cmd.Parameters.AddWithValue("@id", inputId);
                    cmd.Parameters.AddWithValue("@first_name", postEmployee.first_name);
                    cmd.Parameters.AddWithValue("@last_name", postEmployee.last_name);
                    cmd.Parameters.AddWithValue("@designation", postEmployee.designation);
                    cmd.Parameters.AddWithValue("@salary", postEmployee.salary);
                    cmd.Parameters.AddWithValue("@department_id", postEmployee.department_id);

                    cmd.ExecuteNonQuery();
                    return Results.Ok(
                        new Response { message = "Employee created successfully", status = true }
                    );
                }
            }
        }
        catch (Exception ex)
        {
            return Results.Problem($"DataBase Error: {ex.Message}");
        }
    }
);

app.MapPut(
    "/api/users/put",
    async (PutEmployee putEmployee) =>
    {
        try
        {
            string connStr =
                @"Data Source=10.61.18.8;Integrated Security=True;Pooling=False;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Name=vscode-mssql;Application Intent=ReadWrite;Command Timeout=30";

            if (String.IsNullOrWhiteSpace(putEmployee.first_name))
            {
                return Results.BadRequest(
                    new Response { message = "First name is required fields.", status = false }
                );
            }
            if (String.IsNullOrWhiteSpace(putEmployee.last_name))
            {
                return Results.BadRequest(
                    new Response { message = "Last name is required fields.", status = false }
                );
            }

            if (String.IsNullOrWhiteSpace(putEmployee.designation))
            {
                return Results.BadRequest(
                    new Response { message = "Designation is required fields.", status = false }
                );
            }
            if (putEmployee.department_id <= 0)
            {
                return Results.BadRequest(
                    new Response
                    {
                        message = "A valid department identifier must be supplied.",
                        status = false,
                    }
                );
            }
            if (putEmployee.salary <= 0)
            {
                return Results.BadRequest(
                    new Response { message = "Salary must be greater than 0.", status = false }
                );
            }
            if (putEmployee.inputId <= 0)
            {
                return Results.BadRequest(
                    new Response { message = "Id must be greater than 0.", status = false }
                );
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                await conn.OpenAsync();
                string checkQuery =
                    "SELECT COUNT(1) FROM [dbo].[Employee_5] WHERE FirstName = @first_name AND LastName = @last_name AND DepartmentID = @department_id AND isDeleted = 0";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@first_name", putEmployee.first_name.Trim());
                    checkCmd.Parameters.AddWithValue("@last_name", putEmployee.last_name.Trim());
                    checkCmd.Parameters.AddWithValue("@department_id", putEmployee.department_id);

                    int count = Convert.ToInt32(await checkCmd.ExecuteScalarAsync() ?? 0);

                    if (count > 0)
                    {
                        // Returns 409 Conflict to signal a duplicate entry constraint violation
                        return Results.Conflict(
                            new Response
                            {
                                message =
                                    $"An active employee named '{putEmployee.first_name} {putEmployee.last_name}' in department {putEmployee.department_id}  already exists.",
                                status = false,
                            }
                        );
                    }
                }

                string query =
                    @"update [dbo].[Employee_5] set FirstName = @first_name, LastName = @last_name, Designation = @designation, Salary=@salary, DepartmentID=@department_id where ID = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", putEmployee.inputId);
                    cmd.Parameters.AddWithValue("@first_name", putEmployee.first_name);
                    cmd.Parameters.AddWithValue("@last_name", putEmployee.last_name);
                    cmd.Parameters.AddWithValue("@designation", putEmployee.designation);
                    cmd.Parameters.AddWithValue("@salary", putEmployee.salary);
                    cmd.Parameters.AddWithValue("@department_id", putEmployee.department_id);

                    cmd.ExecuteNonQuery();
                    return Results.Ok(
                        new Response { message = "Employee created successfully", status = true }
                    );
                }
            }
        }
        catch (Exception ex)
        {
            return Results.Problem($"DataBase Error: {ex.Message}");
        }
    }
);

app.MapDelete(
    "api/users/delete",
    async (int inputId) =>
    {
        try
        {
            string connStr =
                @"Data Source=10.61.18.8;Integrated Security=True;Pooling=False;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Name=vscode-mssql;Application Intent=ReadWrite;Command Timeout=30";

            if (inputId <= 0)
            {
                return Results.BadRequest(
                    new Response { message = "Id must be greater than 0.", status = false }
                );
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query =
                    "update [dbo].[Employee_5] set isDeleted = @delete where id=@inputId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@inputId", inputId);
                    cmd.Parameters.AddWithValue("@delete", 1);
                    cmd.ExecuteNonQuery();
                }
            }
            return Results.Ok(
                new Response { message = "Employee Removed successfully", status = true }
            );
        }
        catch (Exception ex)
        {
            return Results.Problem($"DataBase Error: {ex.Message}");
        }
    }
);

app.Urls.Add("http://localhost:5000");
Console.WriteLine("Server starting... Open http://localhost:5000/swagger");
app.Run();
