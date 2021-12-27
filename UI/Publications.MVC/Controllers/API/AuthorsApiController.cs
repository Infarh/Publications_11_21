using Microsoft.AspNetCore.Mvc;
using Publications.Domain.Entities;
using Publications.Interfaces.Repositories;

namespace Publications.MVC.Controllers.API;

[ApiController, Route("api/authors")]
public class AuthorsApiController : ControllerBase
{
    private readonly IRepository<Person> _Repository;

    public AuthorsApiController(IRepository<Person> Repository) { _Repository = Repository; }

    [HttpGet]
    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await _Repository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<Person?> GetAsync(int id)
    {
        return await _Repository.GetAsync(id);
    }

    [HttpPost]
    public async Task<int> AddAsync(Person item)
    {
        return await _Repository.AddAsync(item);
    }

    [HttpPut]
    public async Task<bool> UpdateAsync(Person item)
    {
        return await _Repository.UpdateAsync(item);
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteAsync(int id)
    {
        return await _Repository.DeleteAsync(new() { Id = id });
    }
}