using Amazon.SecurityToken.SAML;
using dal;
using dal.Repositories;
using dal.Repositories.interfaces;

namespace tests.Repository;

public class TestUserRepository : BaseTest
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

        UnitOfWork.User.InsertOneAsync(user).Wait();
        var result = UnitOfWork.User.Login(user.Email, user.HashedPassword).Result;
        
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

        UnitOfWork.User.InsertOneAsync(user).Wait();
        UnitOfWork.User.InsertOneAsync(user).Wait();
        UnitOfWork.User.InsertOneAsync(user).Wait();
        
        Assert.That(UnitOfWork.User.FilterBy(doc => doc.Email == user.Email).Count(), Is.EqualTo(3));
        return Task.CompletedTask;
    }

    [Test]
    public Task TestFilterByConvert()
    {
        User user = new User("Pa$$word");
        user.Email = "ConvertBy";
        user.Active = true;

        UnitOfWork.User.InsertOneAsync(user).Wait();
        var result = UnitOfWork.User.FilterBy(doc => doc.Email == "ConvertBy", doc => doc);
        
        Assert.That(result.Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }

    [Test]
    public Task TestFindOneAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "FindOne";
        user.Active = true;

        UnitOfWork.User.InsertOneAsync(user).Wait();
        var result = UnitOfWork.User.FindOneAsync(doc => doc.Email == "FindOne").Result;
        
        Assert.That(result.Email, Is.EqualTo(user.Email));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestFindByIdAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "FindOneByID";
        user.Active = true;

        UnitOfWork.User.InsertOneAsync(user).Wait();

        var getItem = UnitOfWork.User.FindOneAsync(doc => doc.Email == "FindOneByID").Result;
        var result = UnitOfWork.User.FindByIdAsync(getItem.ID).Result;
        
        Assert.That(result.Email, Is.EqualTo(user.Email));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestInsertOneAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "InsertOne";
        user.Active = true;

        UnitOfWork.User.InsertOneAsync(user).Wait();
        var result = UnitOfWork.User.FilterBy(doc => doc.Email == "InsertOne");
        
        Assert.That(result.Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestUpdateOneAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "UpdateOne";
        user.Active = true;

        UnitOfWork.User.InsertOneAsync(user).Wait();

        user.Active = false;
        UnitOfWork.User.UpdateOneAsync(user);
        var result = UnitOfWork.User.FindOneAsync(doc => doc.Email == "UpdateOne").Result;
        
        Assert.That(result.Active, Is.EqualTo(false));
        return Task.CompletedTask;
    }
    
    // TODO:
    [Test]
    public Task TestInsertOrUpdateOneAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "Update";
        user.Active = true;
        
        User user2 = new User("Pa$$word");
        user2.Email = "Insert";
        user2.Active = true;

        UnitOfWork.User.InsertOneAsync(user).Wait();

        user.Email = "UpdateOrInsert";
        UnitOfWork.User.InsertOrUpdateOneAsync(user).Wait();
        UnitOfWork.User.InsertOrUpdateOneAsync(user2).Wait();
        
        Assert.That(UnitOfWork.User.FilterBy(doc => doc.Email == "Update").Count(), Is.EqualTo(0));
        Assert.That(UnitOfWork.User.FilterBy(doc => doc.Email == "Insert").Count(), Is.EqualTo(1));
        Assert.That(UnitOfWork.User.FilterBy(doc => doc.Email == "UpdateOrInsert").Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteOneAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "DeleteOne";
        user.Active = true;

        UnitOfWork.User.InsertOneAsync(user).Wait();
        UnitOfWork.User.DeleteOneAsync(doc => doc.Email == "DeleteOne");
        var result = UnitOfWork.User.FilterBy(doc => doc.Email == user.Email);
        
        Assert.That(result.Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteByIdAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "DeleteOneByID";
        user.Active = true;

        UnitOfWork.User.InsertOneAsync(user).Wait();

        var getItem = UnitOfWork.User.FindOneAsync(doc => doc.Email == "DeleteOneByID").Result;
        var result = UnitOfWork.User.DeleteByIdAsync(getItem.ID);
        
        Assert.That(UnitOfWork.User.FilterBy(doc => doc.Email == user.Email).Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteManyAsync()
    {
        User user = new User("Pa$$word");
        user.Email = "DeleteMany";
        user.Active = true;

        UnitOfWork.User.InsertOneAsync(user).Wait();
        UnitOfWork.User.InsertOneAsync(user).Wait();
        UnitOfWork.User.InsertOneAsync(user).Wait();
        
        Assert.That(UnitOfWork.User.FilterBy(doc => doc.Email == user.Email).Count(), Is.EqualTo(3));

        UnitOfWork.User.DeleteManyAsync(doc => doc.Email == "DeleteMany");
        var result = UnitOfWork.User.FilterBy(doc => doc.Email == user.Email);
        
        Assert.That(result.Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
}