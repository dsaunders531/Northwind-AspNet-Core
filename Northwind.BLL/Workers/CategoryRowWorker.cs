using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using tools.EF;
using tools.Exceptions;
using tools.Utility;
using tools.WorkerPattern;

namespace Northwind.BLL.Workers
{
    // Note: this is the basis for GenericWorkerBase and will need migrating to this abstract class
    // when it has been proven stable.

    /// <summary>
    /// Manage the category database items
    /// </summary>
    public class CategoryWorker : Worker
    {
        private IRepository<Category, int> CategoryRepository { get; set; }

        public CategoryWorker(IRepository<Category, int> categories)
        {
            CategoryRepository = categories;
        }

        /// <summary>
        /// Fetches all the categories
        /// </summary>
        /// <returns></returns>
        public List<CategoryRowApiO> FetchAll()
        {
            List<Category> categories = CategoryRepository.FetchAll.OrderBy(c => c.CategoryName).ToList();
            List<CategoryRowApiO> result = new List<CategoryRowApiO>();

            using (Transposition transposition = new Transposition())
            {
                foreach (Category item in categories)
                {
                    result.Add(transposition.Transpose<CategoryRowApiO>(item, new CategoryRowApiO()));
                }
            }

            return result;
        }

        /// <summary>
        /// Fetches 1 category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public CategoryRowApiO Fetch(int categoryId)
        {
            Category category = CategoryRepository.Fetch(categoryId);

            if (category == null)
            {
                throw new RecordNotFoundException("Could not find a item with id '{0}'", categoryId.ToString());
            }
            else
            {
                CategoryRowApiO result = new CategoryRowApiO();

                using (Transposition tranposition = new Transposition())
                {
                    result = tranposition.Transpose<CategoryRowApiO>(category, result);
                }

                return result;
            }
        }

        /// <summary>
        /// Fetch an item using a other fields apart from its key.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public CategoryRowApiO Fetch(CategoryRowApiO category)
        {
            Category categoryDb = CategoryRepository.FetchAll.Where(f => f.CategoryName.Substring(0, category.CategoryName.Length) == category.CategoryName
                                                                            && f.Description.Substring(0, category.Description.Length) == category.Description).FirstOrDefault();

            if (categoryDb == null)
            {
                throw new RecordNotFoundException("Could not find a matching item.");
            }
            else
            {
                CategoryRowApiO result = new CategoryRowApiO();

                using (Transposition transposition = new Transposition())
                {
                    result = transposition.Transpose(categoryDb, result);
                }

                return result;
            }
        }

        /// <summary>
        /// Adds a new record
        /// </summary>
        /// <param name="apiModel"></param>
        /// <returns>The key for the new record.</returns>
        public CategoryRowApiO Create(CategoryRowApiO apiModel)
        {
            CategoryRowApiO result = default(CategoryRowApiO);

            if (apiModel == null)
            {
                throw new ArgumentNullException("The input parameter cannot be null");
            }
            else
            {
                if (ModelState.IsValid == false)
                {
                    string modelType = apiModel.GetType().ToString();
                    ModelState.AddModelError(string.Empty, string.Format("Validation Failed, the {0} contains invalid data.", modelType));
                    throw new ModelStateException(string.Format("The {0} is not valid", modelType), ModelState);
                }
                else
                {
                    // check the item does not already exist
                    Category dbModel = CategoryRepository.FetchAll.Where(cat => cat.CategoryName == apiModel.CategoryName).FirstOrDefault();

                    if (dbModel != null)
                    {
                        // the item already exists
                        throw new RecordFoundException(string.Format("A record for '{0}' already exists!", apiModel.CategoryId.ToString()));
                    }
                    else
                    {
                        dbModel = new Category();

                        // map the item and add it
                        using (Transposition transposition = new Transposition())
                        {
                            dbModel = transposition.Transpose<Category>(apiModel, dbModel);

                            CategoryRepository.Create(dbModel);

                            result = transposition.Transpose<CategoryRowApiO>(dbModel, apiModel);
                        }
                    }
                }
            }

            return result;
        }

        public CategoryRowApiO Update(CategoryRowApiO apiModel)
        {
            CategoryRowApiO result = default(CategoryRowApiO);

            if (apiModel == null)
            {
                throw new ArgumentNullException("The input parameter cannot be null");
            }
            else
            {
                if (ModelState.IsValid == false)
                {
                    string modelType = apiModel.GetType().ToString();
                    ModelState.AddModelError(string.Empty, string.Format("Validation Failed, the {0} contains invalid data.", modelType));
                    throw new ModelStateException(string.Format("The {0} is not valid", modelType), ModelState);
                }
                else
                {
                    // find the item
                    Category dbModel = CategoryRepository.Fetch(apiModel.CategoryId);

                    if (dbModel == null)
                    {
                        // item was not found!
                        throw new RecordNotFoundException("Could not find a item with id '{0}'", apiModel.CategoryId.ToString());
                    }
                    else
                    {
                        // map the apiO to the db model.
                        using (Transposition transposition = new Transposition())
                        {
                            dbModel = transposition.Transpose<Category>(apiModel, dbModel);

                            // update the item
                            CategoryRepository.Update(dbModel);

                            result = transposition.Transpose<CategoryRowApiO>(dbModel, apiModel);
                        }
                    }
                }
            }

            return result;
        }

        public void Delete(int categoryId)
        {
            Category category = CategoryRepository.Fetch(categoryId);

            if (category == null)
            {
                // record not found
                throw new RecordNotFoundException("Could not find a item with id '{0}'", categoryId.ToString());
            }
            else
            {
                CategoryRepository.Delete(category);
            }
        }

        public void Commit()
        {
            CategoryRepository.Save();
        }
    }
}
