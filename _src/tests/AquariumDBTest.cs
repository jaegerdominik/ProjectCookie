using dal;

namespace tests;

public class AquariumDBTest : BaseTest
{
    [Test]
    public async Task InsertTest()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "Test";

        await UnitOfWork.Aquarium.InsertOneAsync(aquarium);
    }
}