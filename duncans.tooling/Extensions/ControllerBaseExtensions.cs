// <copyright file="ControllerBaseExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace duncans
{
    public static class ControllerBaseExtensions
    {
        /// <summary>
        /// Creates an internal server error result (status code 500).
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static StatusCodeResult InternalServerError(this Controller controller)
        {
            return new StatusCodeResult(500);
        }

        /// <summary>
        /// Creates a formbidden server error result (status code 403).
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static StatusCodeResult Forbidden(this Controller controller)
        {
            return new StatusCodeResult(403);
        }

        /// <summary>
        /// Create pagination details from list information.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="pageAction"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public static PaginationModel CreatePaginationModel(this Controller controller, string pageAction, int itemsPerPage, int itemCount)
        {
            PaginationModel result = new PaginationModel()
            {
                CurrentPage = 0,
                ItemCount = itemCount,
                PageAction = pageAction,
                ItemsPerPage = itemsPerPage
            };

            return result;
        }

        /// <summary>
        /// Adds error details to model state when a dbUpdateConcurrencyException happens.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="e"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> UpdateExceptionToModelState<T>(this Controller controller, DbUpdateConcurrencyException e)
        {
            return GetExceptionDetails<T>(e.Entries);
        }

        /// <summary>
        /// Adds error details to model state when a dbUpdateException happens.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="e"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> UpdateExceptionToModelState<T>(this Controller controller, DbUpdateException e)
        {
            return GetExceptionDetails<T>(e.Entries);
        }

        private static List<KeyValuePair<string, string>> GetExceptionDetails<T>(IReadOnlyList<EntityEntry> entries)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (EntityEntry entity in entries)
            {
                PropertyValues dbProperties = entity.GetDatabaseValues();

                if (dbProperties == null)
                {
                    result.Add(new KeyValuePair<string, string>(string.Empty, "The record has been deleted."));
                }
                else
                {
                    result.Add(new KeyValuePair<string, string>(string.Empty, "The underlying database record has been changed during the time you have been editing the record."));

                    T dbModel = default(T);
                    T clientModel = default(T);

                    try
                    {
                        dbModel = (T)dbProperties.ToObject();
                    }
                    catch (Exception)
                    {
                        dbModel = default(T);
                    }

                    try
                    {
                        clientModel = (T)entity.Entity;
                    }
                    catch (Exception)
                    {
                        clientModel = default(T);
                    }

                    if (dbModel != null && clientModel != null)
                    {
                        if (!(dbModel.Equals(default(T)) && clientModel.Equals(default(T))))
                        {
                            foreach (PropertyInfo propInfo in dbModel.GetType().GetProperties())
                            {
                                try
                                {
                                    if (clientModel.GetPropertyValue(propInfo.Name) != dbModel.GetPropertyValue(propInfo.Name))
                                    {
                                        result.Add(new KeyValuePair<string, string>(propInfo.Name, string.Format("Current Value: {0}", dbModel.GetPropertyValue(propInfo.Name).ToString())));
                                    }
                                }
                                catch (Exception)
                                {
                                    // do nothing.
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
