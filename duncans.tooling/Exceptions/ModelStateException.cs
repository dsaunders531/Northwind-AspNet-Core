// <copyright file="ModelStateException.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Runtime.Serialization;

namespace duncans
{
    [Serializable]
    public class ModelStateException : Exception
    {
        public ModelStateException(string message, ModelStateDictionary modelState) : base(message)
        {
            this.Configure(modelState);
        }

        public ModelStateException(string message, ModelStateDictionary modelState, Exception innerException) : base(message, innerException)
        {
            this.Configure(modelState);
        }

        public int ErrorCount { get; private set; }

        public ModelStateDictionary ModelState { get; private set; }

        private void Configure(ModelStateDictionary modelState)
        {
            this.ErrorCount = modelState.ErrorCount;
            this.ModelState = modelState;
        }
    }
}