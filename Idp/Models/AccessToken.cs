namespace Idp.Models;

public class AccessToken(string token, int expiresAt, int userId)
{
    public string Token { get; set; } = token;

    public int ExpiresAt { get; set; } = expiresAt;

    public int UserId { get; set; } = userId;

    private const int OneDay = 60 * 60 * 24;

    public static AccessToken Build(int userId)
    {
        var token = GenerateRandomString();
        const int expiresIn = OneDay * 1000;

        // Int32 に変換するためにエポック形式にする
        return new AccessToken(token, Convert.ToInt32((DateTime.Now - DateTime.UnixEpoch).TotalSeconds) + expiresIn, userId);
    }

    public void Save(List<AccessToken> db)
    {
        var index = db.FindIndex(x => x.UserId == UserId);
        if (index != -1)
        {
            db[index] = this;
        }
        else
        {
            db.Add(this);
        }
    }

    private static string GenerateRandomString()
    {
        var random = new Random();
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
