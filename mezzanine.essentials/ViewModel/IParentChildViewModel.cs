namespace mezzanine.ViewModel
{
    public interface IParentChildViewModel<TParent, TChild> : IRecordViewModel<TParent>
    {
        IRecordsListViewModel<TChild> Children { get; set; }
    }
}
