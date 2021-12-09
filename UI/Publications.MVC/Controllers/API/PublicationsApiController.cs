using Microsoft.AspNetCore.Mvc;

using Publications.Domain.Entities;
using Publications.Interfaces.Repositories;

namespace Publications.MVC.Controllers.API
{
    [ApiController, Route("api/publications")]
    public class PublicationsApiController : ControllerBase
    {
        private readonly IRepository<Publication> _Repository;

        public PublicationsApiController(IRepository<Publication> Repository) { _Repository = Repository; }

        [HttpGet]
        public async Task<IEnumerable<Publication>> GetAllAsync()
        {
            return await _Repository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<Publication?> GetAsync(int id)
        {
            return await _Repository.GetAsync(id);
        }

        [HttpPost]
        public async Task<int> AddAsync(Publication item)
        {
            return await _Repository.AddAsync(item);
        }

        [HttpPut]
        public async Task<bool> UpdateAsync(Publication item)
        {
            return await _Repository.UpdateAsync(item);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteAsync(int id)
        {
            return await _Repository.DeleteAsync(new() { Id = id });
        }
    }
}
