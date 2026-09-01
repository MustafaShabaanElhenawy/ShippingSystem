using System;
using System.Collections.Generic;
using TrainingShippingSystem.DAL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.BLL
{
    public class ContainerBLL
    {
        private readonly ContainerDAL containerDAL =
            new ContainerDAL();


        // GET ALL
        public List<Container> GetContainers()
        {
            return containerDAL.GetContainers();
        }


        // GET BY ID
        public Container GetContainerByID(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "Invalid container ID.");

            return containerDAL.GetContainerByID(id);
        }


        // INSERT
        public int InsertContainer(Container container)
        {
            ValidateContainer(container);

            return containerDAL.InsertContainer(container);
        }


        // UPDATE
        public bool UpdateContainer(Container container)
        {
            if (container.ID <= 0)
                throw new ArgumentException(
                    "Invalid container ID.");

            ValidateContainer(container);

            return containerDAL.UpdateContainer(container);
        }


        // DELETE
        public bool DeleteContainer(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "Invalid container ID.");

            return containerDAL.DeleteContainer(id);
        }


        // SEARCH
        public List<Container> SearchContainers(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return containerDAL.GetContainers();

            return containerDAL.SearchContainers(search.Trim());
        }


        // VALIDATION
        private void ValidateContainer(Container container)
        {
            if (container == null)
                throw new ArgumentException(
                    "Container data is required.");

            if (string.IsNullOrWhiteSpace(
                container.ContainerNumber))
            {
                throw new ArgumentException(
                    "Container Number is required.");
            }

            if (string.IsNullOrWhiteSpace(
                container.ContainerType))
            {
                throw new ArgumentException(
                    "Container Type is required.");
            }

            if (container.BillID <= 0)
                throw new ArgumentException(
                    "Bill is required.");
        }
    }
}