namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Interfaces
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public interface IAppDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
