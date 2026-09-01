using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TrainingShippingSystem.DAL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.BLL
{
    public class VoyageBLL
    {
        private readonly VoyageDAL voyageDAL = new VoyageDAL();


        // GET ALL
        public List<Voyage> GetVoyages()
        {
            return voyageDAL.GetVoyages();
        }


        // GET BY ID
        public Voyage GetVoyageByID(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid voyage ID.");

            return voyageDAL.GetVoyageByID(id);
        }


        // INSERT
        public int InsertVoyage(Voyage voyage)
        {
            ValidateVoyage(voyage);

            return voyageDAL.InsertVoyage(voyage);
        }


        // UPDATE
        public bool UpdateVoyage(Voyage voyage)
        {
            if (voyage.ID <= 0)
                throw new ArgumentException("Invalid voyage ID.");

            ValidateVoyage(voyage);

            return voyageDAL.UpdateVoyage(voyage);
        }


        // DELETE
        public bool DeleteVoyage(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid voyage ID.");

            return voyageDAL.DeleteVoyage(id);
        }


        // SEARCH
        public List<Voyage> SearchVoyages(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return voyageDAL.GetVoyages();

            return voyageDAL.SearchVoyages(search.Trim());
        }


        // VALIDATION
        private void ValidateVoyage(Voyage voyage)
        {
            if (voyage == null)
                throw new ArgumentException("Voyage data is required.");

            if (string.IsNullOrWhiteSpace(voyage.VoyageNumber))
                throw new ArgumentException("Voyage Number is required.");

            if (string.IsNullOrWhiteSpace(voyage.VesselName))
                throw new ArgumentException("Vessel Name is required.");

            if (voyage.ETA == default(DateTime))
                throw new ArgumentException("ETA is required.");

            if (voyage.ETD == default(DateTime))
                throw new ArgumentException("ETD is required.");

            if (voyage.ETD < voyage.ETA)
                throw new ArgumentException("ETD cannot be earlier than ETA.");
        }
    }
}