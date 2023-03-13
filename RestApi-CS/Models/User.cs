namespace RestAPI.Models;

public class User
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string gender { get; set; }
    public string phone { get; set; }
    public string country { get; set; }
 
    public User(CreateUser request)
    {
        id = request.id;
        name = request.name;
        email = request.email;
        gender = request.gender;
        phone = request.phone;
        country = request.country;
    }
}

public record CreateUser(
    int id,
    string name,
    string email,
    string gender,
    string phone,
    string country);