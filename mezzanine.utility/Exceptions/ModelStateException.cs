using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace mezzanine.Exceptions
{
    [Serializable]
    public class ModelStateException : Exception
    {
        public int ErrorCount { get; private set; }

        public ModelStateDictionary ModelState { get; private set; }


        public ModelStateException(string message, ModelStateDictionary modelState) : base(message)
        {
            Configure(modelState);
        }

        public ModelStateException(string message, ModelStateDictionary modelState, Exception innerException) : base(message, innerException)
        {
            Configure(modelState);
        }

        private void Configure(ModelStateDictionary modelState)
        {
            ErrorCount = modelState.ErrorCount;
            ModelState = modelState;
        }
    }
}