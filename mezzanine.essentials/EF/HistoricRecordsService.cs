using System;
using System.Linq;
using mezzanine.Utility;
using Microsoft.EntityFrameworkCore;

namespace mezzanine.EF
{
    /// <summary>
    /// Base repository class where historic records are required.
    /// The model must derive from DbModel, the historic model must derive from DbHistoricModel.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="THistoricModel"></typeparam>
    public abstract class HistoricRecordsService<TModel, TKey, THistoricModel> : IHistoricRepository<TModel, TKey, THistoricModel>, IDisposable
    {        
        private IRepository<TModel, TKey> CurrentRecordRepository { get; set; }

        private IRepository<THistoricModel, long> HistoricRecordRepository { get; set; }

        public HistoricRecordsService(IRepository<TModel, TKey> currentRecordRepository, IRepository<THistoricModel, long> historicRecordRepository)
        {
            if (! (typeof(TModel).BaseType == typeof(DbModel<TKey>)))
            {
                throw new ApplicationException("The model must derive from DbModel.");
            }

            if ( ! (typeof(THistoricModel).BaseType == typeof(HistoricDbModel<TKey>)))
            {
                throw new ApplicationException("The model must derive from HistoricDbModel.");
            }

            this.HistoricRecordRepository = historicRecordRepository;
            this.CurrentRecordRepository = currentRecordRepository;
        }

        public IQueryable<TModel> FetchAll
        {
            get
            {
                return this.CurrentRecordRepository.FetchAll.Where(r => ((IDbModel<TKey>)r).Deleted == false);
            }
        }

        public void Commit() => this.CurrentRecordRepository.Commit();

        private void CreateHistory(TModel model, EFActionType action, DateTime? startDate, DateTime? endDate)
        {
            // 1 - find current history record if any and end-date it (startDate - 1)
            IHistoricDbModel<TKey> currentRecord = (IHistoricDbModel<TKey>)this.FetchHistoric(((IDbModel<TKey>)model).RowId, startDate ?? DateTime.Now);

            if (currentRecord != null)
            {
                currentRecord.EndDate = (startDate ?? DateTime.Now).AddTicks(-1);

                this.HistoricRecordRepository.Update((THistoricModel)currentRecord);
            }
            else
            {
                action = EFActionType.Create; // when a current record cannot be found, the action must be create as we are creating the first historic record.
            }

            // 2 - create new history record
            THistoricModel historic = Activator.CreateInstance<THistoricModel>();

            using (Transposition transposition = new Transposition())
            {
                historic = transposition.Transpose(model, historic);
            }

            IHistoricDbModel<TKey> commonItems = (IHistoricDbModel<TKey>)historic;

            commonItems.RowId = default(long);
            commonItems.ModelId = ((IDbModel<TKey>)model).RowId;
            commonItems.Action = action;
            commonItems.StartDate = startDate ?? DateTime.Now;
            commonItems.EndDate = endDate;

            this.HistoricRecordRepository.Create((THistoricModel)commonItems);

            this.HistoricRecordRepository.Commit();
        }

        public void Create(TModel item)
        {
            IDbModel<TKey> createRowId = (IDbModel<TKey>)item; // create the next row Id - we can't save because we are in a transaction.

            if (this.CurrentRecordRepository.FetchAll.Count() > 0)
            {
                createRowId.RowId = (TKey)this.CurrentRecordRepository.FetchAll.Max(m => ((IDbModel<TKey>)m).RowId).Increment(createRowId.RowId.GetType());
            }
            else
            {
                createRowId.RowId = (TKey)createRowId.RowId.Increment(createRowId.RowId.GetType());
            }
            

            this.CurrentRecordRepository.Create((TModel)createRowId);

            this.CreateHistory((TModel)createRowId, EFActionType.Create, DateTime.Now, null);
        }

        public void Delete(TModel item)
        {
            //this.CurrentRecordRepository.Delete(item); Records must not be deleted.

            IDbModel<TKey> deletedItem = (IDbModel<TKey>)item;
            deletedItem.Deleted = true;
            this.CurrentRecordRepository.Update((TModel)deletedItem);

            this.CreateHistory(item, EFActionType.Delete, DateTime.Now, null);
        }

        public void Dispose()
        {
            this.CurrentRecordRepository.Dispose();
            this.CurrentRecordRepository = null;
            this.HistoricRecordRepository.Dispose();
            this.HistoricRecordRepository = null;
        }

        public TModel Fetch(TKey id)
        {
            IDbModel<TKey> result = (IDbModel<TKey>)this.CurrentRecordRepository.Fetch(id);

            if (result.Deleted == true)
            {
                return default(TModel);
            }
            else
            {
                return (TModel)result;
            }            
        }

        public THistoricModel FetchHistoric(TKey modelId, DateTime onDate)
        {
            THistoricModel result = default(THistoricModel);

            IQueryable<THistoricModel> allItems = this.FetchHistory(modelId);

            if (allItems != null)
            {
                if (allItems.Count() > 0)
                {
                    result = allItems.Where(h => (((IHistoricDbModel<TKey>)h).StartDate ?? DateTime.Now) <= onDate
                                        && (((IHistoricDbModel<TKey>)h).EndDate ?? DateTime.Now) >= onDate).First();
                }                
            }

            return result;
        }

        public IQueryable<THistoricModel> FetchHistory(TKey modelId)
        {
            return this.HistoricRecordRepository.FetchAll.Where(h => ((IHistoricDbModel<TKey>)h).ModelId.Equals(modelId));
        }

        public void Update(TModel item)
        {
            this.CurrentRecordRepository.Update(item);    
            
            this.CreateHistory(item, EFActionType.Update, DateTime.Now, null);
        }
    }

    /// <summary>
    /// Base repository class where historic records are required.
    /// The model must derive from DbModel, the historic model must derive from DbHistoricModel.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="THistoricModel"></typeparam>
    //public abstract class HistoricRepository<TModel, TKey, THistoricModel> : IHistoricRepository<TModel, TKey, THistoricModel>, IDisposable
    //{
    //    private IRepository<TModel, TKey> CurrentRecordRepository { get; set; }

    //    private DbContext Context { get; set; } = null;

    //    public Func<IHistoricDbModel<TKey>, bool> HistoricAllRecordSelector(TKey modelId)
    //    {
    //        return new Func<IHistoricDbModel<TKey>, bool>(h => h.ModelId.Equals(modelId));
    //    }

    //    public Func<IHistoricDbModel<TKey>, bool> HistoricRecordSelector(TKey modelId, DateTime? startDate, DateTime? endDate)
    //    {
    //        return new Func<IHistoricDbModel<TKey>, bool>(h => h.ModelId.Equals(modelId) 
    //                                                        && (startDate ?? DateTime.Now) <= h.StartDate 
    //                                                        && (endDate ?? DateTime.Now) >= h.EndDate);
    //    }

    //    public HistoricRepository(DbContext context, IRepository<TModel, TKey> currentRecordRepository)
    //    {
    //        this.CurrentRecordRepository = currentRecordRepository;
    //        this.Context = context;
    //    }

    //    public IQueryable<TModel> FetchAll => this.CurrentRecordRepository.FetchAll;

    //    public void Commit()
    //    {
    //        this.Context.SaveChanges();
    //    }

    //    public void Create(TModel item)
    //    {
    //        this.CurrentRecordRepository.Create(item);

    //        this.CreateHistory(item, EFActionType.Create, DateTime.Now, null);
    //    }

    //    private void CreateHistory(TModel model, EFActionType action, DateTime? startDate, DateTime? endDate)
    //    {
    //        // 1 - find current history record if any and end-date it (startDate - 1)
    //        IHistoricDbModel<TKey> currentRecord = (IHistoricDbModel<TKey>)this.FetchHistoric(((IDbModel<TKey>)model).RowId, startDate ?? DateTime.Now);

    //        if (currentRecord != null)
    //        {
    //            currentRecord.EndDate = (startDate ?? DateTime.Now).AddTicks(-1);

    //            this.Context.Update(currentRecord);
    //        }
    //        else
    //        {
    //            action = EFActionType.Create; // when a current record cannot be found, the action must be create as we are creating the first historic record.
    //        }

    //        // 2 - create new history record
    //        IHistoricDbModel<TKey> historic = (IHistoricDbModel<TKey>)Activator.CreateInstance(typeof(THistoricModel));

    //        using (Transposition transposition = new Transposition())
    //        {
    //            historic = transposition.Transpose(model, historic);
    //        }

    //        historic.RowId = 0;
    //        historic.ModelId = ((IDbModel<TKey>)model).RowId;
    //        historic.Action = action;
    //        historic.StartDate = startDate ?? DateTime.Now;
    //        historic.EndDate = endDate;

    //        this.Context.Add(historic);
    //    }

    //    public void Delete(TModel item)
    //    {
    //        this.CurrentRecordRepository.Delete(item);

    //        this.CreateHistory(item, EFActionType.Delete, DateTime.Now, null);
    //    }

    //    public TModel Fetch(TKey id)
    //    {
    //        return this.CurrentRecordRepository.Fetch(id);
    //    }

    //    public abstract THistoricModel FetchHistoric(TKey modelId, DateTime onDate);

    //    public abstract IQueryable<THistoricModel> FetchHistory(TKey modelId);
      
    //    public void Update(TModel item)
    //    {
    //        this.CurrentRecordRepository.Update(item);

    //        this.CreateHistory(item, EFActionType.Update, DateTime.Now, null);
    //    }

    //    public virtual void Dispose()
    //    {
    //        this.CurrentRecordRepository.Dispose();
    //    }
    //}
}
