using dal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using services.Authentication;
using services.Response;
using services.ServiceImpl;

namespace tests.ServiceTests;

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
        
        UserService service = new UserService(UnitOfWork, UnitOfWork.User, AquariumLogger, new Authenticator(UnitOfWork));
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        ItemResponseModel<User> model = await service.Create(user);
        
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Data, Is.Not.Null);
        Assert.That(model.HasError, Is.False);

        var result = service.Login(user.Email, user.HashedPassword).Result;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Token, Is.Not.Null);
        Assert.That(result.Token, Has.Length.GreaterThanOrEqualTo(244));
    }
}