namespace Publications.Interfaces;

public interface IDbInitializer
{
    Task<bool> DeleteAsync(CancellationToken Cancel = default);

    Task Initialize(bool RemoveAtStart = false, CancellationToken Cancel = default);
}