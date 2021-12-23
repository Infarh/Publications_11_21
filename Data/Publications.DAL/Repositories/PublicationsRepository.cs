using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Publications.DAL.Context;
using Publications.Domain.Entities;

namespace Publications.DAL.Repositories
{
    public class PublicationsRepository : DbRepository<Publication>
    {
        protected override IQueryable<Publication> Items => Set
           .Include(p => p.Place)
           .Include(p => p.Authors);

        public PublicationsRepository(PublicationsDB db, ILogger<PublicationsRepository> Logger) : base(db, Logger) { }
    }
}
