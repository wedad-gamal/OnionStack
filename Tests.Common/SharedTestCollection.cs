namespace Tests.Common
{
    [CollectionDefinition("Shared")]
    public class SharedTestCollection :
      ICollectionFixture<MapsterFixture>,
      ICollectionFixture<DatabaseFixture>
    {
        // No code needed, xUnit uses this class just for grouping
    }
}
