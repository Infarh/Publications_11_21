using Microsoft.Extensions.Logging;
using Publications.Domain.Entities;
using Publications.Interfaces;
using Publications.Interfaces.Repositories;

namespace Publications.Services
{
    public class DatabasePublicationManager : IPublicationManager
    {
        private readonly IRepository<Publication> _Publications;
        private readonly IRepository<Person> _Persons;
        private readonly IRepository<Place> _Places;
        private readonly ILogger<DatabasePublicationManager> _Logger;

        public DatabasePublicationManager(
            IRepository<Publication> Publications,
            IRepository<Person> Persons, 
            IRepository<Place> Places, 
            ILogger<DatabasePublicationManager> Logger)
        {
            _Publications = Publications;
            _Persons = Persons;
            _Places = Places;
            _Logger = Logger;
        }

        //public Publication Craete(string Name, DateTime Date, string Place, params (string LastName, string FirstName, string Patronymic)[] Authors)
        //{

        //}
    }
}
