using mezzanine.EF;
using mezzanine.Utility;
using mezzanine.WorkerPattern;
using mezzanine.TagHelpers;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.BLL.Workers
{
    /// <summary>
    /// The retail inventory worker.
    /// Exposes products and categories for viewing.
    /// </summary>
    public class RetailInventoryWorker : Worker
    {
        private IRepository<ProductDbModel, int> ProductRepository { get; set; }

        private IRepository<CategoryDbModel, int> CategoryRepository { get; set; }

        public RetailInventoryWorker(IRepository<ProductDbModel, int> products, IRepository<CategoryDbModel, int> categories)
        {
            this.ProductRepository = products;
            this.CategoryRepository = categories;
        }

        public List<CategoryRowApiModel> GetAllCategories()
        {
            List<CategoryDbModel> categories = this.CategoryRepository.FetchAll.OrderBy(c => c.CategoryName).ToList();
            List <CategoryRowApiModel> result = new List<CategoryRowApiModel>();

            using (Transposition transposition = new Transposition())
            {
                foreach (CategoryDbModel item in categories)
                {
                    result.Add(transposition.Transpose<CategoryRowApiModel>(item, new CategoryRowApiModel()));
                }
            }
            
            return result;
        }

        public List<ProductApiModel> GetAllProducts()
        {
            List<ProductDbModel> products = this.ProductRepository.FetchAll.OrderBy(o => o.ProductName).ToList();
            List<ProductApiModel> result = new List<ProductApiModel>();

            using (Transposition transposition = new Transposition())
            {
                foreach (ProductDbModel item in products)
                {
                    result.Add(transposition.Transpose<ProductApiModel>(item, new ProductApiModel()));
                }
            }

            return result;
        }

        public PaginationModel CategoriesPages(string pageAction, int itemsPerPage)
        {
            return base.CreatePaginationModel(pageAction, itemsPerPage, this.CategoryRepository.FetchAll.Count());
        }

        public List<CategoryRowApiModel> GetCategoriesPaged(int itemsPerPage, int page)
        {
            page -= 1;
            List<CategoryDbModel> pagedCategories = this.CategoryRepository.FetchAll.OrderBy(c => c.CategoryName).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
            List<CategoryRowApiModel> result = new List<CategoryRowApiModel>();

            using (Transposition transposition = new Transposition())
            {
                foreach (CategoryDbModel item in pagedCategories)
                {
                    result.Add(transposition.Transpose<CategoryRowApiModel>(item, new CategoryRowApiModel()));
                }
            }

            return result;
        }

        public PaginationModel ProductsPages(string pageAction, int itemsPerPage)
        {
            return base.CreatePaginationModel(pageAction, itemsPerPage, this.ProductRepository.FetchAll.Count());
        }

        public List<ProductApiModel> GetProductsPaged(int itemsPerPage, int page)
        {
            page -= 1;
            List<ProductDbModel> pagedProducts = this.ProductRepository.FetchAll.OrderBy(p => p.ProductName).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
            List<ProductApiModel> result = new List<ProductApiModel>();

            using (Transposition transposition = new Transposition())
            {
                foreach (ProductDbModel item in pagedProducts)
                {
                    result.Add(transposition.Transpose<ProductApiModel>(item, new ProductApiModel()));
                }
            }

            return result;
        }

        public CategoryRowApiModel GetCategory(int categoryId)
        {
            CategoryDbModel category = this.CategoryRepository.Fetch(categoryId);
            CategoryRowApiModel result = new CategoryRowApiModel();

            using (Transposition tranposition = new Transposition())
            {
                result = tranposition.Transpose<CategoryRowApiModel>(category, result);
            }

            return result;
        }

        public ProductApiModel GetProduct(int productId)
        {
            ProductDbModel product = this.ProductRepository.Fetch(productId);
            ProductApiModel result = new ProductApiModel();

            using (Transposition transposition = new Transposition())
            {
                result = transposition.Transpose<ProductApiModel>(product, result);
            }

            return result;
        }

        public List<ProductRowApiModel> GetCategoryProducts(int categoryId)
        {
            CategoryDbModel category = this.CategoryRepository.Fetch(categoryId);
            List<ProductRowApiModel> result = new List<ProductRowApiModel>();

            if (category != null)
            {
                using (Transposition transposition = new Transposition())
                {
                    foreach (ProductDbModel item in category.Products)
                    {
                        result.Add(transposition.Transpose<ProductRowApiModel>(item, new ProductRowApiModel()));
                    }
                }               
            }

            return result;
        }

        public PaginationModel CategoryProductsPages(int categoryId, string pageAction, int itemsPerPage)
        {
            return base.CreatePaginationModel(pageAction, itemsPerPage, this.GetCategoryProducts(categoryId).Count());
        }

        public List<ProductApiModel> GetCategoryProductsPaged(int categoryId, int itemsPerPage, int page)
        {
            page -= 1;
            List<ProductDbModel> pagedProducts = (from ProductDbModel p in this.ProductRepository.FetchAll
                                          where p.CategoryId == categoryId
                                          orderby p.ProductName
                                          select p)
                                          .Skip(itemsPerPage * page).Take(itemsPerPage)
                                          .ToList();

            List<ProductApiModel> result = new List<ProductApiModel>();

            using (Transposition transposition = new Transposition())            
            {
                foreach (ProductDbModel item in pagedProducts)
                {
                    result.Add(transposition.Transpose<ProductApiModel>(item, new ProductApiModel()));
                }
            }

            return result;
        }
    }
}
