using System.Data.Common;

namespace Api_Model.Model;

public class User
{
    public int id { get; set; }
    public string first_name { get; set; } = "";
    public string last_name { get; set; } = "";

    public string email { get; set; } = "";

    // public User(int user_id, string First_Name, string Last_Name, string Email)
    // {
    //     id = user_id;
    //     first_name = First_Name;
    //     last_name = Last_Name;
    //     email = Email;
    // }
}
