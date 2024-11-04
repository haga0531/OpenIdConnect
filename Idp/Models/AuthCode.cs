﻿namespace Idp.Models;

public class AuthCode(string code, int userId, string clientId, string redirectUri, DateTimeOffset expiredAt)
{
    public string Code { get; set; } = code;

    public int UserId { get; set; } = userId;

    public string ClientId { get; set; } = clientId;

    public string RedirectUri { get; set; } = redirectUri;

    public DateTimeOffset ExpiredAt { get; set; } = expiredAt;

    public DateTimeOffset? UsedAt { get; set; }

    public static AuthCode Build(int userId, string clientId, string redirectUri)
    {
        var code = Guid.NewGuid().ToString("N")[..8];
        var expiredAt = DateTime.Now.AddMinutes(1);

        return new AuthCode(code, userId, clientId, redirectUri, expiredAt);
    }

    public void Save(List<AuthCode> db)
    {
        var index = db.FindIndex(x => x.Code == Code);
        if (index != -1)
        {
            db[index] = this;
        }
        else
        {
            db.Add(this);
        }
    }
}