using System;

namespace LegacyApp;

public class UserValidator : IUserValidator
{
    public bool ValidateUser(string firstName, string lastName, string email, DateTime dateOfBirth)
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