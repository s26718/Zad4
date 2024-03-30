using System;

namespace LegacyApp
{
    public class UserService
    {
        private IClientRepository _clientRepository;
        private ICreditService _creditService;

        
        public UserService(IClientRepository clientRepository, ICreditService creditService)
        {
            _clientRepository = clientRepository;
            _creditService = creditService;
        }

        public UserService()
        {
            _clientRepository = new ClientRepository();
            _creditService = new UserCreditService();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            if (!IsEmailCorrect(email))
            {
                return false;
            }
            
            if ( GetUserAge(dateOfBirth)< 21)
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
        
        
        private bool IsEmailCorrect(string email)
        {
            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int GetUserAge(DateTime dateOfBirth)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            return age;
        }
    }
    
}
