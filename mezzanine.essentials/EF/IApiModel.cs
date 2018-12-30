namespace mezzanine.EF
{
    /// <summary>
    /// Interface for api models which are simplified versions of IDbModel
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IApiModel<TKey>
    {
        TKey RowId { get; set; }
    }
}
