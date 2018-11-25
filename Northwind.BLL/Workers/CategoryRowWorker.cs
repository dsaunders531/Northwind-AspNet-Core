using mezzanine.EF;
using mezzanine;
using mezzanine.Utility;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.BLL.Workers
{
    // Note: this is the basis for GenericWorkerBase and will need migrating to this abstract class
    // when it has been proven stable.

    /// <summary>
    /// Manage the category database items
    /// </summary>
    public class CategoryWorker : Worker
    {
        private IRepository<CategoryDbModel, int> CategoryRepository { get; set; }

        public CategoryWorker(IRepository<CategoryDbModel, int> categories)
        {
            this.CategoryRepository = categories;
        }

        /// <summary>
        /// Fetches all the categories
        /// </summary>
        /// <returns></returns>
        public List<CategoryRowApiModel> FetchAll()
        {
            List<CategoryDbModel> categories = this.CategoryRepository.FetchAll.OrderBy(c => c.CategoryName).ToList();
            List<CategoryRowApiModel> result = new List<CategoryRowApiModel>();

            using (Transposition transposition = new Transposition())
            {
                foreach (CategoryDbModel item in categories)
                {
                    result.Add(transposition.Transpose<CategoryRowApiModel>(item, new CategoryRowApiModel()));
                }
            }

            return result;
        }

        /// <summary>
        /// Fetches 1 category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public CategoryRowApiModel Fetch(int categoryId)
        {
            CategoryDbModel category = this.CategoryRepository.Fetch(categoryId);

            if (category == null)
            {
                throw new RecordNotFoundException("Could not find a item with id '{0}'", categoryId.ToString());
            }
            else
            {
                CategoryRowApiModel result = new CategoryRowApiModel();

                using (Transposition tranposition = new Transposition())
                {
                    result = tranposition.Transpose<CategoryRowApiModel>(category, result);
                }

                return result;
            }
        }

        /// <summary>
        /// Fetch an item using a other fields apart from its key.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public CategoryRowApiModel Fetch(CategoryRowApiModel category)
        {
            CategoryDbModel categoryDb = this.CategoryRepository.FetchAll.Where(f => f.CategoryName.Substring(0, category.CategoryName.Length) == category.CategoryName 
                                                                            && f.Description.Substring(0, category.Description.Length) == category.Description).FirstOrDefault();

            if (categoryDb == null)
            {
                throw new RecordNotFoundException("Could not find a matching item.");
            }
            else
            {
                CategoryRowApiModel result = new CategoryRowApiModel();

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
        public CategoryRowApiModel Create(CategoryRowApiModel apiModel)
        {
            CategoryRowApiModel result = default(CategoryRowApiModel);

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
                    CategoryDbModel dbModel = this.CategoryRepository.FetchAll.Where(cat => cat.CategoryName == apiModel.CategoryName).FirstOrDefault();

                    if (dbModel != null)
                    {
                        // the item already exists
                        throw new RecordFoundException(string.Format("A record for '{0}' already exists!", apiModel.CategoryId.ToString()));
                    }
                    else
                    {
                        dbModel = new CategoryDbModel();
                        
                        // map the item and add it
                        using (Transposition transposition = new Transposition())
                        {
                            dbModel = transposition.Transpose<CategoryDbModel>(apiModel, dbModel);

                            this.CategoryRepository.Create(dbModel);

                            result = transposition.Transpose<CategoryRowApiModel>(dbModel, apiModel);
                        }
                    }
                }
            }

            return result;
        }

        public CategoryRowApiModel Update(CategoryRowApiModel apiModel)
        {
            CategoryRowApiModel result = default(CategoryRowApiModel);

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
                    CategoryDbModel dbModel = this.CategoryRepository.Fetch(apiModel.CategoryId);

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
                            dbModel = transposition.Transpose<CategoryDbModel>(apiModel, dbModel);

                            // update the item
                            this.CategoryRepository.Update(dbModel);

                            result = transposition.Transpose<CategoryRowApiModel>(dbModel, apiModel);                        
                        }
                    }
                }
            }

            return result;
        }

        public void Delete(int categoryId)
        {
            CategoryDbModel category = this.CategoryRepository.Fetch(categoryId);

            if (category == null)
            {
                // record not found
                throw new RecordNotFoundException("Could not find a item with id '{0}'", categoryId.ToString());
            }
            else
            {
                this.CategoryRepository.Delete(category);
            }
        }

        public void Commit()
        {
            this.CategoryRepository.Commit();            
        }
    }
}
