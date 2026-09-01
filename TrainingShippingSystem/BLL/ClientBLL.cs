using System;
using System.Collections.Generic;
using System.Net.Mail;
using TrainingShippingSystem.DAL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.BLL
{
    public class ClientBLL
    {
        private readonly ClientDAL clientDAL;

        public ClientBLL()
        {
            clientDAL = new ClientDAL();
        }


        // GET ALL
        public List<Client> GetAllClients()
        {
            return clientDAL.GetAll();
        }


        // GET BY ID
        public Client GetClientByID(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Client ID must be greater than zero.");
            }

            return clientDAL.GetByID(id);
        }


        // INSERT
        public int AddClient(Client client)
        {
            ValidateClient(client);

            return clientDAL.Insert(client);
        }


        // UPDATE
        public bool UpdateClient(Client client)
        {
            if (client.ID <= 0)
            {
                throw new ArgumentException(
                    "Client ID must be greater than zero.");
            }

            ValidateClient(client);

            return clientDAL.Update(client);
        }


        // DELETE
        public bool DeleteClient(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Client ID must be greater than zero.");
            }

            return clientDAL.Delete(id);
        }


        // SEARCH
        public List<Client> SearchClients(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return clientDAL.GetAll();
            }

            return clientDAL.Search(search.Trim());
        }


        // VALIDATION
        private void ValidateClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentException(
                    "Client data is required.");
            }

            if (string.IsNullOrWhiteSpace(client.Name))
            {
                throw new ArgumentException(
                    "Client name is required.");
            }

            if (string.IsNullOrWhiteSpace(client.Email))
            {
                throw new ArgumentException(
                    "Client email is required.");
            }

            if (!IsValidEmail(client.Email))
            {
                throw new ArgumentException(
                    "Please enter a valid email address.");
            }
        }


        // EMAIL VALIDATION
        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mailAddress =
                    new MailAddress(email);

                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}