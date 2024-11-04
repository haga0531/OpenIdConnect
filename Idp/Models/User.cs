namespace Idp.Models;

public class User(int id, string email, string password, string clientId)
{
    public int Id { get; set; } = id;

    public string Email { get; set; } = email;

    public string Password { get; set; } = password;

    public string ClientId { get; set; } = clientId;

    public static User FindByEmail(List<User> db, string email)
    {
        var result = db.FirstOrDefault(x => x.Email == email);
        if (result == null)
        {
            throw new Exception("User not found.");
        }

        return result;
    }

    public static bool Login(List<User> db, string email, string password)
    {
        return db.Any(x => x.Email == email && x.Password == password);
    }
}
