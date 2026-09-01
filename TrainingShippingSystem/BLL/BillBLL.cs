using System;
using System.Collections.Generic;
using TrainingShippingSystem.DAL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.BLL
{
    public class BillBLL
    {
        private readonly BillDAL billDAL = new BillDAL();


        // GET ALL
        public List<Bill> GetBills()
        {
            return billDAL.GetBills();
        }


        // GET BY ID
        public Bill GetBillByID(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid bill ID.");

            return billDAL.GetBillByID(id);
        }


        // INSERT
        public int InsertBill(Bill bill)
        {
            ValidateBill(bill);

            return billDAL.InsertBill(bill);
        }


        // UPDATE
        public bool UpdateBill(Bill bill)
        {
            if (bill.ID <= 0)
                throw new ArgumentException("Invalid bill ID.");

            ValidateBill(bill);

            return billDAL.UpdateBill(bill);
        }


        // DELETE
        public bool DeleteBill(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid bill ID.");

            return billDAL.DeleteBill(id);
        }


        // SEARCH
        public List<Bill> SearchBills(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return billDAL.GetBills();

            return billDAL.SearchBills(search.Trim());
        }


        // VALIDATION
        private void ValidateBill(Bill bill)
        {
            if (bill == null)
                throw new ArgumentException("Bill data is required.");

            if (string.IsNullOrWhiteSpace(bill.BillNumber))
                throw new ArgumentException("Bill Number is required.");

            if (bill.ClientID <= 0)
                throw new ArgumentException("Client is required.");

            if (bill.VoyageID <= 0)
                throw new ArgumentException("Voyage is required.");

            if (bill.GrossWeight < 0)
                throw new ArgumentException("Gross Weight cannot be negative.");

            if (bill.NetWeight < 0)
                throw new ArgumentException("Net Weight cannot be negative.");

            if (bill.NetWeight > bill.GrossWeight)
                throw new ArgumentException(
                    "Net Weight cannot be greater than Gross Weight.");
        }
    }
}