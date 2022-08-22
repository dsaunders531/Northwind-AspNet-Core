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
            SavedApiSessions = new List<ApiSessionModel>();
            UnsavedApiSessions = new List<ApiSessionModel>();
        }

        public IQueryable<ApiSessionModel> FetchAll
        {
            get
            {
                return SavedApiSessions.AsQueryable();
            }
        }

        public void Create(ApiSessionModel item)
        {
            UnsavedApiSessions.Add(item);
        }

        public void Delete(ApiSessionModel item)
        {
            if (UnsavedApiSessions.Contains(item))
            {
                UnsavedApiSessions.Remove(item);
            }

            if (SavedApiSessions.Contains(item))
            {
                SavedApiSessions.Remove(item);
            }
        }

        public ApiSessionModel Fetch(string token)
        {
            return (from ApiSessionModel l in FetchAll
                    where l.Token == token
                    select l).FirstOrDefault();
        }

        public ApiSessionModel FetchByLogin(ApiLoginModel model)
        {
            return (from ApiSessionModel l in FetchAll
                    where l.Email == model.Email && l.Password == model.Password
                    select l).FirstOrDefault();
        }

        /// <summary>
        /// Remove any logins which have expired
        /// </summary>
        public void ClearExpiredLogins(int timeoutHours)
        {
            IQueryable<ApiSessionModel> expiredModels = from ApiSessionModel l in FetchAll
                                                        where l.SessionStarted < DateTime.Now.AddHours(timeoutHours * -1)
                                                        select l;

            foreach (ApiSessionModel item in expiredModels)
            {
                Delete(item);
            }

            Save();
        }

        /// <summary>
        /// Update is not supported by the ApiLoginRepository
        /// </summary>
        /// <param name="item"></param>
        public void Update(ApiSessionModel item)
        {
            Delete(item);
            UnsavedApiSessions.Add(item);
        }

        /// <summary>
        /// Save the sessions
        /// </summary>
        public void Save()
        {
            // Note all we are doing is moving the unsaved sessions to saved sessions.
            SavedApiSessions.AddRange(UnsavedApiSessions);
            UnsavedApiSessions.Clear();
        }
    }
}
