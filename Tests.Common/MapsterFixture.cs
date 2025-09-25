using Mapster;

namespace Tests.Common
{
    public class MapsterFixture
    {
        public MapsterFixture()
        {
            // Configure Mapster once for all tests
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Application.AssemblyMarker).Assembly);
        }
    }

    [CollectionDefinition("Mapster")]
    public class MapsterCollection : ICollectionFixture<MapsterFixture>
    {
        // This class has no code, it's just a marker for xUnit
    }
}
