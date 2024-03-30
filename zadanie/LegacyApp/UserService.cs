using System;
using System.Xml;

namespace LegacyApp
{
    public class UserService
    {
        private IClientRepository _clientRepository;
        private ICreditService _creditService;
        private IUserValidator _userValidator;

        
        public UserService(IClientRepository clientRepository, ICreditService creditService, IUserValidator userValidator)
        {
            _clientRepository = clientRepository;
            _creditService = creditService;
            _userValidator = userValidator;
        }

        public UserService()
        {
            _clientRepository = new ClientRepository();
            _creditService = new UserCreditService();
            _userValidator = new UserValidator();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {

            if (!_userValidator.ValidateUser(firstName, lastName, email, dateOfBirth))
            {
                return false;
            }
            Client client = _clientRepository.GetById(clientId);

            User user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (ICreditService userCreditService = _creditService)
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit *= 2;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (ICreditService userCreditService = _creditService)
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
        
        
        
    }
    
}
