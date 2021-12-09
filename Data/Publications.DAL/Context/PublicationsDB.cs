using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Publications.Domain.Entities;
using Publications.Domain.Entities.Identity;

namespace Publications.DAL.Context;

public class PublicationsDB : IdentityDbContext<User, Role, string>
{
    public DbSet<Publication> Publications { get; set; }

    public DbSet<Person> Persons { get; set; }

    public PublicationsDB(DbContextOptions<PublicationsDB> Options) : base(Options)
    {
            
    }
}