using System.Data.Common;
using Microsoft.VisualBasic;

namespace Employees.Model;

public class Employee
{
    public int id { get; set; }
    public string first_name { get; set; } = "";
    public string last_name { get; set; } = "";
    public string designation { get; set; } = "";
    public int salary { get; set; }
    public int department_id { get; set; }
    public string department_name { get; set; } = "";
    // public int is_delete { get; set; }
}

public class PostEmployee
{
    public string first_name { get; set; } = "";
    public string last_name { get; set; } = "";
    public string designation { get; set; } = "";
    public int salary { get; set; }
    public int department_id { get; set; }
}

public class PutEmployee
{
    public int inputId { get; set; }
    public string first_name { get; set; } = "";
    public string last_name { get; set; } = "";
    public string designation { get; set; } = "";
    public int salary { get; set; }
    public int department_id { get; set; }
}

public class Response
{
    public string message { get; set; } = "";
    public bool status { get; set; }
    public dynamic data { get; set; } = "";
}
