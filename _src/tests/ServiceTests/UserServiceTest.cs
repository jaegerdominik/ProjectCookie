using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using ProjectCookie._src.dal;
using ProjectCookie._src.services.ServiceImpl;
using Services.Response.Basis;

namespace ProjectCookie._src.tests.ServiceTests;

public class UserServiceTest : BaseServiceTest
{
    [Test]
    public async Task Login()
    {
        User user = new User("Pa$$word");
        user.Email = "login@service.com";
        user.Firstname = "First";
        user.Lastname = "Last";
        user.Active = true;
        
        UserService service = new UserService(CookieLogger, UnitOfWork, UnitOfWork.Users);
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        ItemResponseModel<User> model = await service.Create(user);
        
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Data, Is.Not.Null);
        Assert.That(model.HasError, Is.False);
    }
}