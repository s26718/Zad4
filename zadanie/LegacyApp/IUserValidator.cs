using System;

namespace LegacyApp;

public interface IUserValidator
{
     bool ValidateUser(string firstName, string lastName, string email, DateTime dateOfBirth);
}