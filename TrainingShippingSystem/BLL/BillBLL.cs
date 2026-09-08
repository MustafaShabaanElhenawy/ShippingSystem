using System;
using System.Collections.Generic;
using TrainingShippingSystem.DAL;
using TrainingShippingSystem.Models;
namespace TrainingShippingSystem.BLL
{
    public class BillsBLL
    {
        private readonly BillsDAL billsDAL;
        public BillsBLL()
        {
            billsDAL = new BillsDAL();
        }
        public List<Bill> GetBills()
        {
            return billsDAL.GetBills();
        }
        public Bill GetBillByID(int id)
        {
            return billsDAL.GetBillByID(id);
        }
        public int InsertBill(Bill bill)
        {
            ValidateBill(bill);
            return billsDAL.InsertBill(bill);
        }
        public int UpdateBill(Bill bill)
        {
            ValidateBill(bill);
            return billsDAL.UpdateBill(bill);
        }
        public int DeleteBill(int id)
        {
            return billsDAL.DeleteBill(id);
        }
        public List<Bill> AdvancedSearchBills(
            string billNumber,
            int? clientID,
            int? voyageID)
        {
            return billsDAL.AdvancedSearchBills(
                billNumber,
                clientID,
                voyageID
            );
        }
        // =========================================
        // FILTER + SEARCH + PAGINATION + SORTING
        // =========================================
        public FilterBillsResult FilterBills(
            string billNumber,
            int? clientID,
            int? voyageID,
            int pageNumber,
            int pageSize,
            string sortBy,
            string sortDirection)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            if (pageSize < 1)
            {
                pageSize = 10;
            }
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                sortBy = "ID";
            }
            if (string.IsNullOrWhiteSpace(sortDirection))
            {
                sortDirection = "DESC";
            }
            return billsDAL.FilterBills(
                billNumber,
                clientID,
                voyageID,
                pageNumber,
                pageSize,
                sortBy,
                sortDirection
            );
        }
        private void ValidateBill(Bill bill)
        {
            if (bill == null)
            {
                throw new ArgumentException(
                    "Bill data is required."
                );
            }
            if (string.IsNullOrWhiteSpace(
                bill.BillNumber))
            {
                throw new ArgumentException(
                    "Bill Number is required."
                );
            }
            if (bill.ClientID <= 0)
            {
                throw new ArgumentException(
                    "Client is required."
                );
            }
            if (bill.VoyageID <= 0)
            {
                throw new ArgumentException(
                    "Voyage is required."
                );
            }
            if (bill.GrossWeight <= 0)
            {
                throw new ArgumentException(
                    "Gross Weight is required."
                );
            }
            if (bill.NetWeight <= 0)
            {
                throw new ArgumentException(
                    "Net Weight is required."
                );
            }
            if (bill.NetWeight > bill.GrossWeight)
            {
                throw new ArgumentException(
                    "Net Weight cannot be greater than Gross Weight."
                );
            }
        }
    }
}