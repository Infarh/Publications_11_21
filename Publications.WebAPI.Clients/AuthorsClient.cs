using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Publications.Domain.Entities;
using Publications.Interfaces.Repositories;

namespace Publications.WebAPI.Clients;

public class AuthorsClient : IRepository<Person>
{
    private readonly HttpClient _Client;
    private readonly ILogger<AuthorsClient> _Logger;

    public AuthorsClient(HttpClient Client, ILogger<AuthorsClient> Logger)
    {
        _Client = Client;
        _Logger = Logger;
    }

    public async Task<IEnumerable<Person>> GetAllAsync(CancellationToken Cancel = default)
    {
        var result = await _Client.GetFromJsonAsync<IEnumerable<Person>>("api/authors", Cancel);
        return result!;
    }

    public async Task<Person?> GetAsync(int id, CancellationToken Cancel = default)
    {
        var result = await _Client.GetFromJsonAsync<Person>($"api/authors/{id}", Cancel).ConfigureAwait(false);
        return result;
    }

    public async Task<int> AddAsync(Person item, CancellationToken Cancel = default)
    {
        var response = await _Client.PostAsJsonAsync("api/authors", item, Cancel).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken: Cancel);
    }

    public async Task<bool> UpdateAsync(Person item, CancellationToken Cancel = default)
    {
        var response = await _Client.PutAsJsonAsync("api/authors", item, Cancel).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: Cancel);
    }

    public async Task<bool> DeleteAsync(Person item, CancellationToken Cancel = default)
    {
        var response = await _Client.DeleteAsync($"api/authors/{item.Id}", Cancel).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: Cancel);
    }
}