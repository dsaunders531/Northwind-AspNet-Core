using mezzanine.Models;
using Microsoft.AspNetCore.Mvc;

namespace mezzanine.WorkerPattern
{
    public abstract class Worker : Controller
    {
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
