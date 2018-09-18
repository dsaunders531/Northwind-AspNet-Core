using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Runtime.Serialization;

namespace mezzanine.Exceptions
{
    [Serializable]
    public class ModelStateException : Exception
    {
        public int ErrorCount { get; private set; }

        public ModelStateDictionary ModelState { get; private set; }


        public ModelStateException(string message, ModelStateDictionary modelState) : base(message)
        {
            this.Configure(modelState);
        }

        public ModelStateException(string message, ModelStateDictionary modelState, Exception innerException) : base(message, innerException)
        {
            this.Configure(modelState);
        }

        private void Configure(ModelStateDictionary modelState)
        {
            this.ErrorCount = modelState.ErrorCount;
            this.ModelState = modelState;
        }
    }
}