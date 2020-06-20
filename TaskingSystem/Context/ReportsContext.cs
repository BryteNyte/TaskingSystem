using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TaskingSystem.Models;
using TaskingSystem.ViewModels;

namespace TaskingSystem.Context
{
    public class ReportsContext
    {
        ReportsViewModel rpVM = new ReportsViewModel();
        
        public async Task<List<TasksModel>> GetReports(string date,int cmpID)
        {
           
            int month = 0;
            if (string.IsNullOrWhiteSpace(date))
            {
                month = 0;
            }
            else
            {
                month = Convert.ToInt32(date);
            }


            List<TasksModel> taskModel = new List<TasksModel>();
            taskModel = await rpVM.GetReports(cmpID, month);

            return taskModel;
        }
    }
}