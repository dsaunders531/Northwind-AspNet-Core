using mezzanine.EF;
using mezzanine.Utility;
using mezzanine.WorkerPattern;
using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.BLL.Workers
{
    public class EmployeeRowWorker : IGenericWorker<EmployeeDbModel, EmployeeRowApiModel, int>
    {
        private IHistoryService<EmployeeDbModel, int, EmployeeHistoryDbModel> HistoryService { get; set; }

        public EmployeeRowWorker(IHistoryService<EmployeeDbModel, int, EmployeeHistoryDbModel> historyService)
        {
            this.HistoryService = historyService;
        }

        public void Commit()
        {
            this.HistoryService.Commit();
        }

        public EmployeeRowApiModel Create(EmployeeRowApiModel apiModel)
        {
            using (Transposition transposition = new Transposition())
            {
                EmployeeDbModel model = new EmployeeDbModel();

                model = transposition.Transpose(apiModel, model);

                this.HistoryService.Create(model);

                apiModel = transposition.Transpose(model, apiModel);
            }

            return apiModel;
        }

        public void Delete(int key)
        {
            EmployeeDbModel model = this.HistoryService.Fetch(key);
            this.HistoryService.Delete(model);
        }

        public EmployeeRowApiModel Fetch(int key)
        {
            EmployeeDbModel model = this.HistoryService.Fetch(key);
            EmployeeRowApiModel result = new EmployeeRowApiModel();

            using (Transposition transposition = new Transposition())
            {
                result = transposition.Transpose(model, result);
            }

            return result;
        }

        public EmployeeRowApiModel Fetch(EmployeeRowApiModel apiModel, Func<EmployeeDbModel, bool> fetchWithoutKey)
        {
            return this.Fetch(apiModel.RowId);
        }

        public List<EmployeeRowApiModel> FetchAll()
        {
            List<EmployeeRowApiModel> result = new List<EmployeeRowApiModel>();

            using (Transposition transposition = new Transposition())
            {
                foreach (EmployeeDbModel item in this.HistoryService.FetchAll)
                {
                    result.Add(transposition.Transpose(item, new EmployeeRowApiModel()));
                }
            }

            return result;
        }

        public EmployeeRowApiModel Update(EmployeeRowApiModel apiModel)
        {
            EmployeeDbModel model = this.HistoryService.Fetch(apiModel.RowId);

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
