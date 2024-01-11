using Data.Records;
using Data.Types;

namespace Api.Tests.Api.Inventory.SearchThreads._Helper;

public static class SearchThreadsHelper
{
    public static List<IDatabaseRecord> SetupThreads(UserRecord user, List<IDatabaseRecord> records)
    {
        var thread1 = new ThreadRecord
        {
            Id = 6319,
            Reference = Guid.Parse("1a3eda3e-2533-4c47-8ca5-94fbe471fa48"),
            Brand = "TestBrand1",
            Code = "TestCode1",
            Colour = "TestColour1"
        };

        var thread2 = new ThreadRecord
        {
            Id = 4531,
            Reference = Guid.Parse("4d03c4c2-4858-4ad0-91a4-91a67c54376b"),
            Brand = "TestBrand2",
            Code = "TestCode2",
            Colour = "TestColour2"
        };

        var thread3 = new ThreadRecord
        {
            Id = 4846,
            Reference = Guid.Parse("9e83c942-660a-470f-b6b2-2efcdd41bc51"),
            Brand = "TestBrand1",
            Code = "TestCode3",
            Colour = "TestColour3"
        };

        var thread4 = new ThreadRecord
        {
            Id = 8737,
            Reference = Guid.Parse("e12260d3-6f2e-47b2-9dff-bb9c4af2ed82"),
            Brand = "TestBrand2",
            Code = "TestCode4",
            Colour = "TestColour4"
        };

        var userThread1 = new UserThreadRecord
        {
            User = user,
            Thread = thread1,
            Count = 623
        };

        var userThread2 = new UserThreadRecord
        {
            User = user,
            Thread = thread2,
            Count = 7224
        };

        return new List<IDatabaseRecord>
        {
            thread1,
            thread2,
            thread3,
            thread4,
            userThread1,
            userThread2
        }.Concat(records).ToList();
    }
}