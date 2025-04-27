using FLLVestSjaelland.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FLLVestSjaelland.Data
{
    public class DBContext(DbContextOptions<DBContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
    }
}
