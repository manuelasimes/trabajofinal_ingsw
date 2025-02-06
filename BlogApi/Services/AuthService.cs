namespace BlogApi.AuthServices;
public interface IAuthService
{
    bool ValidateUser(string username, string password);
}

public class AuthService : IAuthService
{
    private readonly Dictionary<string, string> _validUsers = new()
    {
        { "admin", "password" }
    };

    public bool ValidateUser(string username, string password)
    {
        return _validUsers.ContainsKey(username) && _validUsers[username] == password;
    }
}
