using NUnit.Framework;
using ProjectCookie.DAL.Entities;

namespace ProjectCookie.Tests;

[TestFixture]
[Order(1)]
public class CreateDataTest : BaseUnitTest
{
    [Test (ExpectedResult = true)]
    [Order(1)]
    public async Task<bool> CreateTestScoresAndUsers()
    {
        try
        {
            User existingTestUser = await UnitOfWork.Users.FindByIdAsync(-999);
            if (existingTestUser != null) return true;
            
            foreach (User user in _GetUsers())   
                await UnitOfWork.Users.InsertAsync(user);
            
            foreach (Score stats in _GetScores())
                await UnitOfWork.Scores.InsertAsync(stats);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Scores and User creation failed: {e}");
            return false;
        }
    }
    

    #region Data

    private static User[] _GetUsers()
    {
        User testUser1 = new()
        {
            ID = -999,
            Username = "Admin",
        };

        User testUser2 = new()
        {
            ID = -998,
            Username = "Carlos",
        };

        return [testUser1, testUser2];
    }
    
    private static Score[] _GetScores()
    {
        Score testScore1 = new()
        {
            ID = -999,
            Points = 20,
            Timestamp = "00:10,81",
            FK_User = -999,
        };

        Score testScore2 = new()
        {
            ID = -998,
            Points = 20,
            Timestamp = "00:20,23",
            FK_User = -999,
        };

        Score testScore3 = new()
        {
            ID = -997,
            Points = 50,
            Timestamp = "00:39,47",
            FK_User = -998,
        };

        return [testScore1, testScore2, testScore3];
    }
 
    #endregion
}