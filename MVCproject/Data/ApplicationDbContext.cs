

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCproject.Models;

namespace MVCproject.Data
{
    public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
    }
}
