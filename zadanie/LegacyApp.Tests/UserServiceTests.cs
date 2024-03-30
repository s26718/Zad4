using System.Threading.Channels;

namespace LegacyApp.Tests;

public class UserServiceTests
{
    [Fact]
    public void AddUser_Should_Return_False_When_First_Name_Is_Null()
    {
        
        //arrange - act - assert
        string firstName = null;
        string lastName = "czajka";
        string email = "janczajka@gmail.com";
        DateTime dateOfBirth = new DateTime(1999, 9, 30);
        int clientId = 1;
        UserService userService = new UserService();

        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        Assert.Equal(false,result);
    }
    [Fact]
    public void AddUser_Should_Return_False_When_Second_Name_Is_Null()
    {
        
        //arrange - act - assert
        string firstName = "jan";
        string lastName = null;
        string email = "janczajka@gmail.com";
        DateTime dateOfBirth = new DateTime(1999, 9, 30);
        int clientId = 1;
        UserService userService = new UserService();

        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        Assert.Equal(false,result);
    }
    [Fact]
    public void AddUserEmailDoesntHaveAtAndDot()
    {
        //arrange - act - assert
        string firstName = "jan";
        string lastName = "czajka";
        string email = "janczajkagmailcom";
        DateTime dateOfBirth = new DateTime(1999, 9, 30);
        int clientId = 1;
        UserService userService = new UserService();

        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        Assert.Equal(false,result);
    }
    
    [Fact]
    public void AddUserShouldReturnFalseWhenUserUnder21()
    {
        //arrange - act - assert
        string firstName = "jan";
        string lastName = "czajka";
        string email = "jan.czajka@gmailcom";
        DateTime dateOfBirth = new DateTime(2005, 9, 30);
        int clientId = 1;
        UserService userService = new UserService();

        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        Assert.Equal(false,result);
    }
    
    [Fact]
    public void AddUserShouldReturnFalseWhenNormalClientHasCreditLimitUnder500()
    {
        
        string firstName = "jan";
        string lastName = "Kowalski";
        string email = "jan.czajka@gmailcom";
        DateTime dateOfBirth = new DateTime(1999, 9, 30);
        int clientId = 1;
        
        ICreditService creditSerice = new FakeCreditService();
        IClientRepository clientRepository = new FakeClientRepository();
        IUserValidator userValidator = new UserValidator();
        UserService userService = new UserService(clientRepository,creditSerice,userValidator);
        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        Assert.Equal(false,result);
    }
    [Fact]
    public void AddUserShouldReturnTrueWhenNormalClientHasCreditLimitOver500()
    {
        
        string firstName = "jan";
        string lastName = "Doe";
        string email = "jan.czajka@gmailcom";
        DateTime dateOfBirth = new DateTime(1999, 9, 30);
        int clientId = 4;
        
        ICreditService creditSerice = new FakeCreditService();
        IClientRepository clientRepository = new FakeClientRepository();
        IUserValidator userValidator = new UserValidator();
        UserService userService = new UserService(clientRepository,creditSerice,userValidator);
        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        Assert.Equal(true,result);
    }
    [Fact]
    public void AddUserShouldReturnTrueWhenVeryImportantClient()
    {
        
        string firstName = "jan";
        string lastName = "Kowalski";
        string email = "jan.czajka@gmailcom";
        DateTime dateOfBirth = new DateTime(1999, 9, 30);
        int clientId = 2;
        
        ICreditService creditSerice = new FakeCreditService();
        IClientRepository clientRepository = new FakeClientRepository();
        IUserValidator userValidator = new UserValidator();
        UserService userService = new UserService(clientRepository,creditSerice,userValidator);
        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        Assert.Equal(true,result);
    }
    [Fact]
    public void AddUserShouldReturnTrueWhenImportantClient()
    {
        
        string firstName = "jan";
        string lastName = "Kowalski";
        string email = "jan.czajka@gmailcom";
        DateTime dateOfBirth = new DateTime(1999, 9, 30);
        int clientId = 3;
        
        ICreditService creditSerice = new FakeCreditService();
        IClientRepository clientRepository = new FakeClientRepository();
        IUserValidator userValidator = new UserValidator();
        UserService userService = new UserService(clientRepository,creditSerice,userValidator);
        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        Assert.Equal(true,result);
    }
    
    [Fact]
    public void AddUserShouldThrowExceptionWhenUserDoesNotExist()
    {
        
        string firstName = "jan";
        string lastName = "Kowalski";
        string email = "jan.czajka@gmailcom";
        DateTime dateOfBirth = new DateTime(1999, 9, 30);
        int clientId = 5;
        
        ICreditService creditSerice = new FakeCreditService();
        IClientRepository clientRepository = new FakeClientRepository();
        IUserValidator userValidator = new UserValidator();
        UserService userService = new UserService(clientRepository,creditSerice,userValidator);
        //bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        Assert.Throws<ArgumentException>(() =>
        {
            userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        });
    }
    [Fact]
    public void AddUserShouldThrowExceptionWhenClientCreditDoesNotExist()
    {
        
        string firstName = "jan";
        string lastName = "Doesnt exist";
        string email = "jan.czajka@gmailcom";
        DateTime dateOfBirth = new DateTime(1999, 9, 30);
        int clientId = 3;
        
        ICreditService creditSerice = new FakeCreditService();
        IClientRepository clientRepository = new FakeClientRepository();
        IUserValidator userValidator = new UserValidator();
        UserService userService = new UserService(clientRepository,creditSerice,userValidator);
        //bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        Assert.Throws<ArgumentException>(() =>
        {
            userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        });
    }

}