using mezzanine.EF;
using Northwind.DAL.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// This is a singleton repository for storing api login details in memory.
    /// </summary>
    public class ApiLoginRepository : IRepository<ApiSessionModel, string>
    {
        /// <summary>
        /// These are sessions which have been saved.
        /// </summary>
        private List<ApiSessionModel> SavedApiSessions { get; set; }

        /// <summary>
        /// These are sessions which have been added etc but have not been committed.
        /// </summary>
        private List<ApiSessionModel> UnsavedApiSessions { get; set; }

        public ApiLoginRepository()
        {
            this.SavedApiSessions = new List<ApiSessionModel>();
            this.UnsavedApiSessions = new List<ApiSessionModel>();
        }

        public IQueryable<ApiSessionModel> FetchAll
        {
            get
            {
                return this.SavedApiSessions.AsQueryable();
            }
        }

        public void Create(ApiSessionModel item)
        {
            this.UnsavedApiSessions.Add(item);
        }

        public void Delete(ApiSessionModel item)
        {
            if (this.UnsavedApiSessions.Contains(item))
            {
                this.UnsavedApiSessions.Remove(item);
            }

            if (this.SavedApiSessions.Contains(item))
            {
                this.SavedApiSessions.Remove(item);
            }
        }

        public ApiSessionModel Fetch(string token)
        {
            return (from ApiSessionModel l in this.FetchAll
                    where l.Token == token
                    select l).FirstOrDefault();
        }
        
        public ApiSessionModel FetchByLogin(ApiLoginModel model)
        {
            return (from ApiSessionModel l in this.FetchAll
                    where l.Email == model.Email && l.Password == model.Password
                    select l).FirstOrDefault();
        }

        /// <summary>
        /// Remove any logins which have expired
        /// </summary>
        public void ClearExpiredLogins(int timeoutHours)
        {
            this.Commit(); // move unsaved to saved.

            IQueryable<ApiSessionModel> expiredModels = from ApiSessionModel l in this.FetchAll
                                                where l.SessionStarted < DateTime.Now.AddHours(timeoutHours * -1)
                                                select l;

            if (expiredModels != null && expiredModels.Count() > 0)
            {
                foreach (ApiSessionModel item in expiredModels)
                {
                    this.Delete(item);
                }
                this.Commit();
            }
        }

        /// <summary>
        /// Update is not supported by the ApiLoginRepository
        /// </summary>
        /// <param name="item"></param>
        public void Update(ApiSessionModel item)
        {
            this.Delete(item);
            this.UnsavedApiSessions.Add(item);
        }

        /// <summary>
        /// Save the sessions
        /// </summary>
        public void Commit()
        {
            // Note all we are doing is moving the unsaved sessions to saved sessions.
            this.SavedApiSessions.AddRange(this.UnsavedApiSessions);
            this.UnsavedApiSessions.Clear();
        }

        public void Dispose()
        {
            this.SavedApiSessions.Clear();
            this.UnsavedApiSessions.Clear();
            this.SavedApiSessions = null;
            this.UnsavedApiSessions = null;
        }
    }
}
