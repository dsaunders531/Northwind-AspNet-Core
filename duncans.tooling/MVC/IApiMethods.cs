// <copyright file="IApiMethods.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace duncans.MVC
{
    // There is an interface for each type of api action so you have more flexability about the ones you want to implement.

    /// <summary>
    /// Interface for implementing Get on a generic api controller.
    /// </summary>
    /// <typeparam name="TApiRowModel"></typeparam>
    public interface IApiGetable<TApiRowModel>
    {
        ActionResult<List<TApiRowModel>> Get();
    }

    /// <summary>
    /// Interface for implementing Post on a generic api controller.
    /// </summary>
    /// <typeparam name="TApiRowModel"></typeparam>
    /// <typeparam name="TDbModelKey"></typeparam>
    public interface IApiPostable<TApiRowModel, TDbModelKey>
    {
        ActionResult<TApiRowModel> Post(TDbModelKey key);
    }

    /// <summary>
    /// Interface for implementing Put on a generic api controller.
    /// </summary>
    /// <typeparam name="TApiRowModel"></typeparam>
    public interface IApiPutable<TApiRowModel>
    {
        ActionResult<TApiRowModel> Put(TApiRowModel apiRowModel);
    }

    /// <summary>
    /// Interface for implementing Patch on a generic api controller.
    /// </summary>
    /// <typeparam name="TApiRowModel"></typeparam>
    public interface IApiPatchable<TApiRowModel>
    {
        ActionResult<TApiRowModel> Patch(TApiRowModel apiRowModel);
    }

    /// <summary>
    /// Interface for implementing Delete on a generic api controller.
    /// </summary>
    /// <typeparam name="TDbModelKey"></typeparam>
    public interface IApiDeleteable<TDbModelKey>
    {
        ActionResult Delete(TDbModelKey key);
    }
}
