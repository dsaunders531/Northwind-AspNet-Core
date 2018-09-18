namespace mezzanine.Models
{
    /// <summary>
    /// An object to store the results of a rest request.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestResult<T> : WorkerResult
    {
        public int StatusCode { get; set; } = -1;
        public string Content { get; set; } = string.Empty;
        public T Result { get; set; } = default(T);
    }
}
