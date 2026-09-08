using TrainingShippingSystem.DAL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.BLL
{
    public class DashboardBLL
    {
        private readonly DashboardDAL dashboardDAL;

        public DashboardBLL()
        {
            dashboardDAL = new DashboardDAL();
        }

        public DashboardStatistics GetDashboardStatistics()
        {
            return dashboardDAL.GetDashboardStatistics();
        }
    }
}