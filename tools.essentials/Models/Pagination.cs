using System;

/// <summary>
/// Models to use with a common ViewModel in your views. These models help automate some tasks and are required by some of the tag helper classes.
/// </summary>
namespace tools.Models
{
    /// <summary>
    /// A class to hold information about the current page of a list spanning many pages.
    /// Mostly taken from Pro ASP.Net Core MVC Adam Freeman.
    /// Required by the Pagination TagHelper.
    /// </summary>
    public abstract class Pagination : IPagination
    {
        public int ItemCount { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string PageAction { get; set; }

        public int PageCount()
        {
            if (this.ItemsPerPage <=0)
            {
                throw new IndexOutOfRangeException(@"The quantity of items per page must be greater than zero.");
            }

            int maxPages = this.ItemCount / this.ItemsPerPage;
            
            // When the remainder after division is not a whole number, add one to the page count.
            if ((this.ItemCount % this.ItemsPerPage) > 0)
            {
                maxPages += 1;
            }

            return maxPages;
        }
    }    
}
