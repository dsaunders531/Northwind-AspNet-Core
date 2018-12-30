using mezzanine.EF;
using mezzanine.Utility;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;

namespace Northwind.BLL.Workers
{
    public class CustomerRowWorker : IGenericWorker<CustomerDbModel, CustomerRowApiModel, int>
    {
        private IHistoryService<CustomerDbModel, int, CustomerHistoryDbModel> HistoryService { get; set; }

        public CustomerRowWorker(IHistoryService<CustomerDbModel, int, CustomerHistoryDbModel> historyService)
        {
            this.HistoryService = historyService;
        }

        public void Commit()
        {
            this.HistoryService.Commit();
        }

        public CustomerRowApiModel Create(CustomerRowApiModel apiModel)
        {
            using (Transposition transposition = new Transposition())
            {
                CustomerDbModel model = new CustomerDbModel();

                model = transposition.Transpose(apiModel, model);

                this.HistoryService.Create(model);

                apiModel = transposition.Transpose(model, apiModel);
            }

            return apiModel;
        }

        public void Delete(int key)
        {
            CustomerDbModel model = this.HistoryService.Fetch(key);
            this.HistoryService.Delete(model);
        }

        public CustomerRowApiModel Fetch(int key)
        {
            CustomerDbModel model = this.HistoryService.Fetch(key);
            CustomerRowApiModel result = new CustomerRowApiModel();

            using (Transposition transposition = new Transposition())
            {
                result = transposition.Transpose(model, result);
            }

            return result;
        }

        public CustomerRowApiModel Fetch(CustomerRowApiModel apiModel, Func<CustomerDbModel, bool> fetchWithoutKey)
        {
            return this.Fetch(apiModel.RowId);
        }

        public List<CustomerRowApiModel> FetchAll()
        {
            List<CustomerRowApiModel> result = new List<CustomerRowApiModel>();

            using (Transposition transposition = new Transposition())
            {
                foreach (CustomerDbModel item in this.HistoryService.FetchAll)
                {
                    result.Add(transposition.Transpose(item, new CustomerRowApiModel()));
                }
            }

            return result;
        }

        public CustomerRowApiModel Update(CustomerRowApiModel apiModel)
        {
            CustomerDbModel model = this.HistoryService.Fetch(apiModel.RowId);

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
