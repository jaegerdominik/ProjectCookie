using NUnit.Framework;
using ProjectCookie._src.dal;
using Tests;

namespace ProjectCookie._src.tests.Repository;

public class TestUserRepository : BaseUnitTest
{
    /*
     * Specific Tests
     */
    
    [Test]
    public async Task TestLogin()
    {
        User user = new User("Pa$$word");
        user.Email = "login@repo.com";
        user.Firstname = "First";
        user.Lastname = "Last";
        user.Active = true;

        UnitOfWork.Users.InsertAsync(user).Wait();
        var result = UnitOfWork.Users.Login(user.Email, user.Password).Result;
        
        Assert.That(result.ID, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo("login@repo.com"));
    }
    
    /*
     * Base Class Tests
     */
    
    [Test]
    public Task TestFilterBy()
    {
        User user = new User("Pa$$word");
        user.Email = "FilterBy";
        user.Active = true;

        UnitOfWork.Users.InsertAsync(user).Wait();
        UnitOfWork.Users.InsertAsync(user).Wait();
        UnitOfWork.Users.InsertAsync(user).Wait();
        
        Assert.That(UnitOfWork.Users.FilterBy(doc => doc.Email == user.Email).Count(), Is.EqualTo(3));
        return Task.CompletedTask;
    }

    [Test]
    public Task TestFilterByConvert()
    {
        User user = new User("Pa$$word");
        user.Email = "ConvertBy";
        user.Active = true;

        UnitOfWork.Users.InsertAsync(user).Wait();
        var result = UnitOfWork.Users.FilterBy(doc => doc.Email == "ConvertBy", doc => doc);
        
        Assert.That(result.Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }

    [Test]
    public Task TestFindOneAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "FindOne";
        user.Active = true;

        UnitOfWork.Users.InsertAsync(user).Wait();
        var result = UnitOfWork.Users.FindAsync(doc => doc.Email == "FindOne").Result;
        
        Assert.That(result.Email, Is.EqualTo(user.Email));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestFindByIdAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "FindOneByID";
        user.Active = true;

        UnitOfWork.Users.InsertAsync(user).Wait();

        var getItem = UnitOfWork.Users.FindAsync(doc => doc.Email == "FindOneByID").Result;
        var result = UnitOfWork.Users.FindByIdAsync(getItem.ID).Result;
        
        Assert.That(result.Email, Is.EqualTo(user.Email));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestInsertOneAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "InsertOne";
        user.Active = true;

        UnitOfWork.Users.InsertAsync(user).Wait();
        var result = UnitOfWork.Users.FilterBy(doc => doc.Email == "InsertOne");
        
        Assert.That(result.Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestUpdateOneAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "UpdateOne";
        user.Active = true;

        UnitOfWork.Users.InsertAsync(user).Wait();

        user.Active = false;
        UnitOfWork.Users.UpdateAsync(user);
        var result = UnitOfWork.Users.FindAsync(doc => doc.Email == "UpdateOne").Result;
        
        Assert.That(result.Active, Is.EqualTo(false));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteOneAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "DeleteOne";
        user.Active = true;

        UnitOfWork.Users.InsertAsync(user).Wait();
        UnitOfWork.Users.DeleteAsync(doc => doc.Email == "DeleteOne");
        var result = UnitOfWork.Users.FilterBy(doc => doc.Email == user.Email);
        
        Assert.That(result.Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteByIdAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "DeleteOneByID";
        user.Active = true;

        UnitOfWork.Users.InsertAsync(user).Wait();

        var getItem = UnitOfWork.Users.FindAsync(doc => doc.Email == "DeleteOneByID").Result;
        var result = UnitOfWork.Users.DeleteByIdAsync(getItem.ID);
        
        Assert.That(UnitOfWork.Users.FilterBy(doc => doc.Email == user.Email).Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
}