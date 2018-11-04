using Microsoft.AspNetCore.Mvc;
using mezzanine.TagHelpers;

namespace mezzanine.WorkerPattern
{
    public abstract class Worker : Controller
    {
        /// <summary>
        /// Create pagination details from list information.
        /// </summary>
        /// <param name="pageAction"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public PaginationModel CreatePaginationModel(string pageAction, int itemsPerPage, int itemCount)
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
    }
}
