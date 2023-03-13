using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using RestAPI.Models;

namespace RestAPI.Services;

public class UserService
{
    private static Dictionary<int, User> users = new Dictionary<int, User>();
    private static int lastId;

    public UserService() {}

    public User CreateNewUser(CreateUser request)
    {
        User user = new User(request);
        if(users.ContainsKey(user.id))
            throw new Exception("user id already exist");
        users.Add(user.id, user);
        lastId = user.id;
        return user;
    }

    public User GetNewUser()
    {
        if(users.Count == 0)
            throw new Exception("No user entered");

        return users[lastId];
    }

    public User UpdateUserData(CreateUser request)
    {
        User user = new User(request);
        if(!users.ContainsKey(user.id))
            throw new Exception("user id not exist");
        users[user.id] = user;
        return user;
    }

}
