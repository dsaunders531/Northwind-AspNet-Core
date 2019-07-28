// <copyright file="PointInTimeService.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using mezzanine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mezzanine.EF
{
    public abstract class PointInTimeService<THistoricModel, TKey> : IPointInTimeService<THistoricModel, TKey>
    {
        public PointInTimeService(IRepository<THistoricModel, TKey> repository)
        {
            this.Repository = repository;
        }

        /// <summary>
        /// Enum to describe the type of existing record.
        /// </summary>
        internal enum ExistingType
        {
            /// <summary>
            /// Item does not exist.
            /// </summary>
            NotExist = 0,

            /// <summary>
            /// Item starts before StartDate and ends after EndDate.
            /// </summary>
            StartsBeforeEndsAfter = 1,

            /// <summary>
            /// Item starts before StartDate and ends before EndDate.
            /// </summary>
            StartsBeforeEndsDuring = 2,

            /// <summary>
            /// Item starts after StartDate And ends after EndDate.
            /// </summary>
            StartsDuringEndsAfter = 3,

            /// <summary>
            /// Item starts after StartDate and ends before EndDate.
            /// </summary>
            StartsDuringEndsDuring = 4,

            /// <summary>
            /// Both start and end parameters are equal to StartDate and EndDate values.
            /// </summary>
            StartsAndEndsEqual = 5,

            /// <summary>
            /// Both the start and end dates are before the StartDate.
            /// </summary>
            StartsAndEndsBefore = 6,

            /// <summary>
            /// Both the start and end dates are after EndDate
            /// </summary>
            StartsAndEndsAfter = 7
        }

        private IRepository<THistoricModel, TKey> Repository { get; set; }

        public void Commit()
        {
            this.Repository.Commit();
        }

        /// <summary>
        /// Fetch one record based on the datetime.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public THistoricModel Fetch(TKey modelId, DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
            }

            return this.Repository.FetchAll
                                    .Where(DateTrackSelector(dateTime))
                                    .Where(m => ((IHistoricDbModel<TKey>)m).ModelId.Equals(modelId))
                                    .FirstOrDefault();
        }

        /// <summary>
        /// Fetch all the records on a date.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public IQueryable<THistoricModel> FetchAll(DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
            }

            return this.Repository.FetchAll
                                        .Where(DateTrackSelector(dateTime))
                                        .OrderBy(o => ((IHistoricDbModel<TKey>)o).ModelId)
                                        .OrderBy(o => ((IHistoricDbModel<TKey>)o).StartDate)
                                        .AsQueryable();
        }

        /// <summary>
        /// All the items regardless of date. No filtering.
        /// </summary>
        /// <returns></returns>
        public IQueryable<THistoricModel> FetchRaw()
        {
            return this.Repository.FetchAll;
        }

        /// <summary>
        /// Update a historic record.
        /// The start and end dates in the input model MUST BE UTC.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public THistoricModel MakeHistory(IHistoricDbModel<TKey> model)
        {
            // Programmers have problems with dates.
            // Make sure you convert the start and end dates to UTC before making the api call.
            if (model.StartDate.HasValue)
            {
                if (model.StartDate.Value.Kind != DateTimeKind.Utc)
                {
                    model.StartDate = model.StartDate.Value.ToUniversalTime();
                }
            }

            if (model.EndDate.HasValue)
            {
                if (model.EndDate.Value.Kind != DateTimeKind.Utc)
                {
                    model.EndDate = model.EndDate.Value.ToUniversalTime();
                }
            }

            model.StartDate = model.StartDate == null ? DateTimeExtensions.DefaultLowDate() : model.StartDate;
            model.EndDate = model.EndDate == null ? DateTimeExtensions.DefaultHighDate() : model.EndDate;

            if (model.StartDate >= model.EndDate)
            {
                throw new ArgumentException("The start date is after the end date.");
            }

            if (model.Action == EFActionType.Delete)
            {
                model.Deleted = true;
            }

            if (model.Deleted == true)
            {
                model.Action = EFActionType.Delete;
            }
            else
            {
                model.Action = EFActionType.Update;
            }

            // Are there any records for this item?
            if (model.ModelId.Equals(default(TKey)))
            {
                model = (IHistoricDbModel<TKey>)this.CreateNewModelId((THistoricModel)model);
            }
            else
            {
                using (Transposition transposition = new Transposition())
                {
                    ExistingType existingType = this.FindExistingType(model.ModelId, model.StartDate.Value, model.EndDate.Value);

                    List<THistoricModel> existingRecords = null;

                    switch (existingType)
                    {
                        case ExistingType.StartsBeforeEndsDuring:
                            // End at the start date
                            existingRecords = this.Repository.FetchAll.Where(StartsBeforeEndsDuringSelector(model.ModelId, model.StartDate.Value, model.EndDate.Value)).ToList();

                            foreach (THistoricModel item in existingRecords)
                            {
                                ((IHistoricDbModel<TKey>)item).EndDate = model.StartDate.Value.AddTicks(-1);
                                this.Repository.Update(item);
                            }

                            // Create new record
                            model.RowId = 0;
                            this.Repository.Create((THistoricModel)model);

                            break;
                        case ExistingType.StartsBeforeEndsAfter:
                            // End at the start date
                            existingRecords = this.Repository.FetchAll.Where(StartsBeforeEndsAfterSelector(model.ModelId, model.StartDate.Value, model.EndDate.Value)).ToList();

                            foreach (THistoricModel item in existingRecords)
                            {
                                if (((IHistoricDbModel<TKey>)item).EndDate > model.EndDate.Value)
                                {
                                    // need to create a new record for period afterwards otherwise there will be a gap.
                                    THistoricModel gapFiller = transposition.Transpose<THistoricModel, THistoricModel>((THistoricModel)item, Activator.CreateInstance<THistoricModel>());
                                    ((IHistoricDbModel<TKey>)gapFiller).StartDate = model.EndDate.Value.AddTicks(1);
                                    this.Repository.Create(gapFiller);
                                }

                                ((IHistoricDbModel<TKey>)item).EndDate = model.StartDate.Value.AddTicks(-1);
                                this.Repository.Update(item);
                            }

                            // Create new record
                            model.RowId = 0;
                            this.Repository.Create((THistoricModel)model);

                            break;
                        case ExistingType.StartsDuringEndsAfter:
                            // Move the start date for the existing record.
                            existingRecords = this.Repository.FetchAll.Where(StartsDuringEndsAfterSelector(model.ModelId, model.StartDate.Value, model.EndDate.Value)).ToList();

                            foreach (THistoricModel item in existingRecords)
                            {
                                ((IHistoricDbModel<TKey>)item).StartDate = model.EndDate.Value.AddTicks(1);
                                this.Repository.Update(item);
                            }

                            // Create new record
                            model.RowId = 0;
                            this.Repository.Create((THistoricModel)model);

                            break;
                        case ExistingType.StartsDuringEndsDuring:
                            existingRecords = this.Repository.FetchAll.Where(StartsDuringEndsDuringSelector(model.ModelId, model.StartDate.Value, model.EndDate.Value)).ToList();

                            model.RowId = 0;

                            foreach (THistoricModel item in existingRecords)
                            {
                                THistoricModel beforeItem = transposition.Transpose((THistoricModel)model, Activator.CreateInstance<THistoricModel>());
                                THistoricModel afterItem = transposition.Transpose((THistoricModel)model, Activator.CreateInstance<THistoricModel>());

                                // Add new record from existing end date to new end date
                                ((IHistoricDbModel<TKey>)beforeItem).EndDate = ((IHistoricDbModel<TKey>)item).StartDate.Value.AddTicks(-1);
                                this.Repository.Create(beforeItem);

                                // Change new record upto existing start date
                                ((IHistoricDbModel<TKey>)afterItem).StartDate = ((IHistoricDbModel<TKey>)item).EndDate.Value.AddTicks(1);
                                this.Repository.Create(afterItem);

                                model = (IHistoricDbModel<TKey>)afterItem;
                            }

                            break;

                        case ExistingType.StartsAndEndsEqual:
                            // Update the existing record
                            List<THistoricModel> existing = this.Repository.FetchAll.Where(StartsAndEndsEqualSelector(model.ModelId, model.StartDate.Value, model.EndDate.Value)).ToList();

                            foreach (THistoricModel item in existing)
                            {
                                model.RowId = ((IHistoricDbModel<TKey>)item).RowId;
                                THistoricModel updateMe = transposition.Transpose((THistoricModel)model, item);
                                this.Repository.Update(item);
                            }

                            break;

                        case ExistingType.StartsAndEndsBefore:
                        // Create a new record. There will be a break in the data.
                        case ExistingType.StartsAndEndsAfter:
                        // Create a new record. There will be a break in the data.
                        case ExistingType.NotExist:
                            // Create a new record.
                            model.RowId = 0;

                            model = (IHistoricDbModel<TKey>)this.CreateNewModelId((THistoricModel)model);

                            break;
                    }
                }
            }

            this.Repository.Commit();

            return (THistoricModel)model;
        }

        /// <summary>
        /// Generic date track record selector.
        /// </summary>
        /// <param name="onDate"></param>
        /// <returns></returns>
        protected Func<THistoricModel, bool> DateTrackSelector(DateTime onDate)
        {
            if (onDate.Kind != DateTimeKind.Utc)
            {
                onDate = new DateTime(onDate.Ticks, DateTimeKind.Utc);
            }

            return new Func<THistoricModel, bool>(o =>
                                                ((IHistoricDbModel<TKey>)o).Deleted == false
                                                && (((IHistoricDbModel<TKey>)o).StartDate == null ? DateTime.UtcNow : ((IHistoricDbModel<TKey>)o).StartDate.Value) <= onDate
                                                && (((IHistoricDbModel<TKey>)o).EndDate == null ? DateTime.UtcNow : ((IHistoricDbModel<TKey>)o).EndDate.Value) >= onDate);
        }

        private Func<THistoricModel, bool> StartsBeforeEndsAfterSelector(TKey modelId, DateTime startDate, DateTime endDate)
        {
            if (startDate.Kind != DateTimeKind.Utc)
            {
                startDate = new DateTime(startDate.Ticks, DateTimeKind.Utc);
            }

            if (endDate.Kind != DateTimeKind.Utc)
            {
                endDate = new DateTime(endDate.Ticks, DateTimeKind.Utc);
            }

            return new Func<THistoricModel, bool>(h =>
                                                    ((IHistoricDbModel<TKey>)h).ModelId.Equals(modelId)
                                                    && ((IHistoricDbModel<TKey>)h).StartDate < startDate
                                                    && ((IHistoricDbModel<TKey>)h).EndDate > endDate);
        }

        private Func<THistoricModel, bool> StartsDuringEndsAfterSelector(TKey modelId, DateTime startDate, DateTime endDate)
        {
            if (startDate.Kind != DateTimeKind.Utc)
            {
                startDate = new DateTime(startDate.Ticks, DateTimeKind.Utc);
            }

            if (endDate.Kind != DateTimeKind.Utc)
            {
                endDate = new DateTime(endDate.Ticks, DateTimeKind.Utc);
            }

            return new Func<THistoricModel, bool>(h => ((IHistoricDbModel<TKey>)h).ModelId.Equals(modelId)
                                                && ((IHistoricDbModel<TKey>)h).EndDate > endDate
                                                && ((IHistoricDbModel<TKey>)h).StartDate >= startDate && ((IHistoricDbModel<TKey>)h).StartDate <= endDate);
        }

        private Func<THistoricModel, bool> StartsBeforeEndsDuringSelector(TKey modelId, DateTime startDate, DateTime endDate)
        {
            if (startDate.Kind != DateTimeKind.Utc)
            {
                startDate = new DateTime(startDate.Ticks, DateTimeKind.Utc);
            }

            if (endDate.Kind != DateTimeKind.Utc)
            {
                endDate = new DateTime(endDate.Ticks, DateTimeKind.Utc);
            }

            return new Func<THistoricModel, bool>(h => ((IHistoricDbModel<TKey>)h).ModelId.Equals(modelId)
                                                    && ((IHistoricDbModel<TKey>)h).StartDate < startDate
                                                    && (((IHistoricDbModel<TKey>)h).EndDate >= startDate && ((IHistoricDbModel<TKey>)h).EndDate <= endDate));
        }

        private Func<THistoricModel, bool> StartsDuringEndsDuringSelector(TKey modelId, DateTime startDate, DateTime endDate)
        {
            if (startDate.Kind != DateTimeKind.Utc)
            {
                startDate = new DateTime(startDate.Ticks, DateTimeKind.Utc);
            }

            if (endDate.Kind != DateTimeKind.Utc)
            {
                endDate = new DateTime(endDate.Ticks, DateTimeKind.Utc);
            }

            return new Func<THistoricModel, bool>(h => ((IHistoricDbModel<TKey>)h).ModelId.Equals(modelId)
                                                            && (((IHistoricDbModel<TKey>)h).StartDate > startDate && ((IHistoricDbModel<TKey>)h).StartDate < endDate)
                                                            && (((IHistoricDbModel<TKey>)h).EndDate < endDate && ((IHistoricDbModel<TKey>)h).EndDate > startDate));
        }

        private Func<THistoricModel, bool> StartsAndEndsEqualSelector(TKey modelId, DateTime startDate, DateTime endDate)
        {
            if (startDate.Kind != DateTimeKind.Utc)
            {
                startDate = new DateTime(startDate.Ticks, DateTimeKind.Utc);
            }

            if (endDate.Kind != DateTimeKind.Utc)
            {
                endDate = new DateTime(startDate.Ticks, DateTimeKind.Utc);
            }

            return new Func<THistoricModel, bool>(h =>
                                                    ((IHistoricDbModel<TKey>)h).ModelId.Equals(modelId)
                                                    && ((IHistoricDbModel<TKey>)h).StartDate == startDate
                                                    && ((IHistoricDbModel<TKey>)h).EndDate == endDate);
        }

        /// <summary>
        /// Find the existing type for the item.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private ExistingType FindExistingType(TKey modelId, DateTime startDate, DateTime endDate)
        {
            if (startDate.Kind != DateTimeKind.Utc)
            {
                startDate = new DateTime(startDate.Ticks, DateTimeKind.Utc);
            }

            if (endDate.Kind != DateTimeKind.Utc)
            {
                endDate = new DateTime(endDate.Ticks, DateTimeKind.Utc);
            }

            ExistingType result = ExistingType.NotExist;

            int startsBeforeEndsAfter = this.Repository.FetchAll.Where(StartsBeforeEndsAfterSelector(modelId, startDate, endDate)).Count();
            int startsBeforeEndsDuring = this.Repository.FetchAll.Where(StartsBeforeEndsDuringSelector(modelId, startDate, endDate)).Count();
            int startsDuringEndsAfter = this.Repository.FetchAll.Where(StartsDuringEndsAfterSelector(modelId, startDate, endDate)).Count();
            int startsDuringEndsDuring = this.Repository.FetchAll.Where(StartsDuringEndsDuringSelector(modelId, startDate, endDate)).Count();
            int startsAndEndsEqual = this.Repository.FetchAll.Where(StartsAndEndsEqualSelector(modelId, startDate, endDate)).Count();

            // Count the cases - there should only be 1 or none otherwise something is badly wrong with the repo.
            int caseCount = startsBeforeEndsAfter + startsBeforeEndsDuring + startsDuringEndsAfter + startsDuringEndsDuring + startsAndEndsEqual; // + startsAndEndsBefore + startsAndEndsAfter;

            if (caseCount == 1)
            {
                if (startsBeforeEndsAfter == 1)
                {
                    result = ExistingType.StartsBeforeEndsAfter;
                }
                else if (startsBeforeEndsDuring == 1)
                {
                    result = ExistingType.StartsBeforeEndsDuring;
                }
                else if (startsDuringEndsAfter == 1)
                {
                    result = ExistingType.StartsDuringEndsAfter;
                }
                else if (startsDuringEndsDuring == 1)
                {
                    result = ExistingType.StartsDuringEndsDuring;
                }
                else if (startsAndEndsEqual == 1)
                {
                    result = ExistingType.StartsAndEndsEqual;
                }
                else
                {
                    // A match was not found?!
                    throw new InvalidOperationException("A matching datetime case could not found. There is a problem with the PointInTimeService.");
                }
            }
            else if (caseCount > 1)
            {
                throw new InvalidOperationException("There can only be one matching case for time point values. There is a problem with the PointInTimeService.");
            }

            return result;
        }

        /// <summary>
        /// Create a new record with a new modelId.
        /// </summary>
        /// <param name="newModel"></param>
        /// <returns></returns>
        private THistoricModel CreateNewModelId(THistoricModel newModel)
        {
            IHistoricDbModel<TKey> model = null;

            using (Transposition transposition = new Transposition())
            {
                model = (IHistoricDbModel<TKey>)transposition.Transpose(newModel, Activator.CreateInstance<THistoricModel>());
            }

            // Are there any records and create a key.
            model.Action = EFActionType.Create;
            model.Deleted = false;

            model.StartDate = model.StartDate == null ? DateTimeExtensions.DefaultLowDate() : model.StartDate.Value;
            model.EndDate = model.EndDate == null ? DateTimeExtensions.DefaultHighDate() : model.EndDate.Value;

            if (model.StartDate.HasValue)
            {
                if (model.StartDate.Value.Kind != DateTimeKind.Utc)
                {
                    model.StartDate = model.StartDate.HasValue ? new DateTime?(new DateTime(model.StartDate.Value.Ticks, DateTimeKind.Utc)) : null;
                }
            }

            if (model.EndDate.HasValue)
            {
                if (model.EndDate.Value.Kind != DateTimeKind.Utc)
                {
                    model.EndDate = model.EndDate.HasValue ? new DateTime?(new DateTime(model.EndDate.Value.Ticks, DateTimeKind.Utc)) : null;
                }
            }

            if (model.ModelId.Equals(default(TKey)))
            {
                TKey maxOfModelId = default(TKey);

                try
                {
                    maxOfModelId = this.FetchRaw().Max(m => ((IHistoricDbModel<TKey>)m).ModelId);
                }
                catch (Exception)
                {
                    // usually caused by no records.
                    maxOfModelId = default(TKey);
                }

                model.ModelId = (TKey)maxOfModelId.Increment(typeof(TKey));
            }

            // create a new record.
            this.Repository.Create((THistoricModel)model);

            return (THistoricModel)model;
        }
    }
}
