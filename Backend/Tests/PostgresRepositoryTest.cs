using System.Diagnostics;
using NUnit.Framework;
using ProjectCookie.DAL.Entities;

namespace ProjectCookie.Tests;

[TestFixture, Order(3)]
public class PostgresRepositoryTest : BaseUnitTest
{
    [Test]
    public void FilterByTest()
    {
        IEnumerable<User> filteredUsers = UnitOfWork.Users.FilterBy(u => u.Username == "Admin");
        Assert.That(filteredUsers.Count(), Is.GreaterThan(0));
    }
    
    [Test]
    public async Task FindByIdTest()
    {
        User? userToFind = await UnitOfWork.Users.FindAsync(u => u.Username == "Admin");
        Debug.Assert(userToFind != null, nameof(userToFind) + " != null");
        Assert.That(userToFind, Is.Not.Null);
        
        User? userToFindById = await UnitOfWork.Users.FindByIdAsync(userToFind.ID);
        Debug.Assert(userToFindById != null, nameof(userToFindById) + " != null");
        Assert.That(userToFindById.Username, Is.EqualTo("Admin"));
    }
}