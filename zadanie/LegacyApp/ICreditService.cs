using System;

namespace LegacyApp;

public interface ICreditService : IDisposable
{
    int GetCreditLimit(string lastName, DateTime dateOfBirth);
    
}