using System.Collections.Generic;
using TrainingShippingSystem.DAL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.BLL
{
    public class ShippingReportBLL
    {
        private readonly ShippingReportDAL reportDAL;

        public ShippingReportBLL()
        {
            reportDAL = new ShippingReportDAL();
        }

        public List<ShippingReport> GetShippingSummary()
        {
            return reportDAL.GetShippingSummary();
        }
    }
}