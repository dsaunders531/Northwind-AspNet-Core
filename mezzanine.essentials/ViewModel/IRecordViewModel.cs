namespace mezzanine.ViewModel
{
    public interface IRecordViewModel<T> : IViewModel
    {
        T ViewData { get; set; }
    }
}
