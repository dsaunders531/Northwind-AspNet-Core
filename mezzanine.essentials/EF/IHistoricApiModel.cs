namespace mezzanine.EF
{
    /// <summary>
    /// Interface for api models which are based on historic records
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IHistoricApiModel<TKey> : IApiModel<long>, IHistoricDbModel<TKey>
    {
    }
}
