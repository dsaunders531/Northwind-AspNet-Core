using mezzanine.EF;
using mezzanine.Utility;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class ProductRowWorker : IGenericWorker<ProductDbModel, ProductRowApiModel, int>
    {
        private IHistoryService<ProductDbModel, int, ProductHistoryDbModel> HistoryService { get; set; }

        public ProductRowWorker(IHistoryService<ProductDbModel, int, ProductHistoryDbModel> historyService)
        {
            this.HistoryService = historyService;
        }

        public void Commit()
        {
            this.HistoryService.Commit();
        }

        public ProductRowApiModel Create(ProductRowApiModel apiModel)
        {
            using (Transposition transposition = new Transposition())
            {
                ProductDbModel model = new ProductDbModel();

                model = transposition.Transpose(apiModel, model);

                this.HistoryService.Create(model);

                apiModel = transposition.Transpose(model, apiModel);
            }

            return apiModel;
        }

        public void Delete(int key)
        {
            ProductDbModel model = this.HistoryService.Fetch(key);
            this.HistoryService.Delete(model);
        }

        public ProductRowApiModel Fetch(int key)
        {
            ProductDbModel model = this.HistoryService.Fetch(key);
            ProductRowApiModel result = new ProductRowApiModel();

            using (Transposition transposition = new Transposition())
            {
                result = transposition.Transpose(model, result);
            }

            return result;
        }

        public ProductRowApiModel Fetch(ProductRowApiModel apiModel, Func<ProductDbModel, bool> fetchWithoutKey)
        {
            return this.Fetch(apiModel.RowId);
        }

        public List<ProductRowApiModel> FetchAll()
        {
            List<ProductRowApiModel> result = new List<ProductRowApiModel>();

            using (Transposition transposition = new Transposition())
            {
                foreach (ProductDbModel item in this.HistoryService.FetchAll)
                {
                    result.Add(transposition.Transpose(item, new ProductRowApiModel()));
                }
            }

            return result;
        }

        public ProductRowApiModel Update(ProductRowApiModel apiModel)
        {
            ProductDbModel model = this.HistoryService.Fetch(apiModel.RowId);

            using (Transposition transposition = new Transposition())
            {
                model = transposition.Transpose(apiModel, model);

                this.HistoryService.Update(model);

                apiModel = transposition.Transpose(model, apiModel);
            }

            return apiModel;
        }
    }
}
