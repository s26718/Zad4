namespace LegacyApp.Tests;

public class FakeCreditService: ICreditService
{
    private readonly Dictionary<string, int> _database =
        new Dictionary<string, int>()
        {
            {"Kowalski", 200},
            {"Malewski", 100},
            {"Smith", 50},
            {"Doe", 3000},
            {"Kwiatkowski", 1000}
        };
    
    public int GetCreditLimit(string lastName, DateTime dateOfBirth)
    {
        int randomWaitingTime = new Random().Next(3000);
        Thread.Sleep(randomWaitingTime);

        if (_database.ContainsKey(lastName))
            return _database[lastName];

        throw new ArgumentException($"Client {lastName} does not exist");
    }

    public void Dispose()
    {
        
    }
}