using Microsoft.AspNetCore.Identity;

namespace main.Database.Storage.MainUser.Model;

public class MainUser : IdentityUser
{
    public int FavoriteNumber
    {
        get;
        set;
    }
}