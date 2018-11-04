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
        private IRepository<Product, int> ProductRepository { get; set; }

        private IRepository<Category, int> CategoryRepository { get; set; }

        public RetailInventoryWorker(IRepository<Product, int> products, IRepository<Category, int> categories)
        {
            this.ProductRepository = products;
            this.CategoryRepository = categories;
        }

        public List<CategoryRowApiO> GetAllCategories()
        {
            List<Category> categories = this.CategoryRepository.FetchAll.OrderBy(c => c.CategoryName).ToList();
            List <CategoryRowApiO> result = new List<CategoryRowApiO>();

            using (Transposition transposition = new Transposition())
            {
                foreach (Category item in categories)
                {
                    result.Add(transposition.Transpose<CategoryRowApiO>(item, new CategoryRowApiO()));
                }
            }
            
            return result;
        }

        public List<ProductApiO> GetAllProducts()
        {
            List<Product> products = this.ProductRepository.FetchAll.OrderBy(o => o.ProductName).ToList();
            List<ProductApiO> result = new List<ProductApiO>();

            using (Transposition transposition = new Transposition())
            {
                foreach (Product item in products)
                {
                    result.Add(transposition.Transpose<ProductApiO>(item, new ProductApiO()));
                }
            }

            return result;
        }

        public PaginationModel CategoriesPages(string pageAction, int itemsPerPage)
        {
            return base.CreatePaginationModel(pageAction, itemsPerPage, this.CategoryRepository.FetchAll.Count());
        }

        public List<CategoryRowApiO> GetCategoriesPaged(int itemsPerPage, int page)
        {
            page -= 1;
            List<Category> pagedCategories = this.CategoryRepository.FetchAll.OrderBy(c => c.CategoryName).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
            List<CategoryRowApiO> result = new List<CategoryRowApiO>();

            using (Transposition transposition = new Transposition())
            {
                foreach (Category item in pagedCategories)
                {
                    result.Add(transposition.Transpose<CategoryRowApiO>(item, new CategoryRowApiO()));
                }
            }

            return result;
        }

        public PaginationModel ProductsPages(string pageAction, int itemsPerPage)
        {
            return base.CreatePaginationModel(pageAction, itemsPerPage, this.ProductRepository.FetchAll.Count());
        }

        public List<ProductApiO> GetProductsPaged(int itemsPerPage, int page)
        {
            page -= 1;
            List<Product> pagedProducts = this.ProductRepository.FetchAll.OrderBy(p => p.ProductName).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
            List<ProductApiO> result = new List<ProductApiO>();

            using (Transposition transposition = new Transposition())
            {
                foreach (Product item in pagedProducts)
                {
                    result.Add(transposition.Transpose<ProductApiO>(item, new ProductApiO()));
                }
            }

            return result;
        }

        public CategoryRowApiO GetCategory(int categoryId)
        {
            Category category = this.CategoryRepository.Fetch(categoryId);
            CategoryRowApiO result = new CategoryRowApiO();

            using (Transposition tranposition = new Transposition())
            {
                result = tranposition.Transpose<CategoryRowApiO>(category, result);
            }

            return result;
        }

        public ProductApiO GetProduct(int productId)
        {
            Product product = this.ProductRepository.Fetch(productId);
            ProductApiO result = new ProductApiO();

            using (Transposition transposition = new Transposition())
            {
                result = transposition.Transpose<ProductApiO>(product, result);
            }

            return result;
        }

        public List<ProductRowApiO> GetCategoryProducts(int categoryId)
        {
            Category category = this.CategoryRepository.Fetch(categoryId);
            List<ProductRowApiO> result = new List<ProductRowApiO>();

            if (category != null)
            {
                using (Transposition transposition = new Transposition())
                {
                    foreach (Product item in category.Products)
                    {
                        result.Add(transposition.Transpose<ProductRowApiO>(item, new ProductRowApiO()));
                    }
                }               
            }

            return result;
        }

        public PaginationModel CategoryProductsPages(int categoryId, string pageAction, int itemsPerPage)
        {
            return base.CreatePaginationModel(pageAction, itemsPerPage, this.GetCategoryProducts(categoryId).Count());
        }

        public List<ProductApiO> GetCategoryProductsPaged(int categoryId, int itemsPerPage, int page)
        {
            page -= 1;
            List<Product> pagedProducts = (from Product p in this.ProductRepository.FetchAll
                                          where p.CategoryId == categoryId
                                          orderby p.ProductName
                                          select p)
                                          .Skip(itemsPerPage * page).Take(itemsPerPage)
                                          .ToList();

            List<ProductApiO> result = new List<ProductApiO>();

            using (Transposition transposition = new Transposition())            
            {
                foreach (Product item in pagedProducts)
                {
                    result.Add(transposition.Transpose<ProductApiO>(item, new ProductApiO()));
                }
            }

            return result;
        }
    }
}
