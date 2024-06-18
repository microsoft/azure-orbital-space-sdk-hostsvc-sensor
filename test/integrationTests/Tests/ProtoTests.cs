namespace Microsoft.Azure.SpaceFx.HostServices.Sensor.IntegrationTests.Tests;

[Collection(nameof(TestSharedContext))]
public class ProtoTests : IClassFixture<TestSharedContext> {
    readonly TestSharedContext _context;
    public ProtoTests(TestSharedContext context) {
        _context = context;
    }

    [Fact]
    public async Task SensorsAvailableRequest() {
        // Arrange
        List<string> expectedProperties = new() { "RequestHeader" };

        CheckProperties<MessageFormats.HostServices.Sensor.SensorsAvailableRequest>(expectedProperties);
    }

    [Fact]
    public async Task SensorsAvailableResponse() {
        // Arrange
        List<string> expectedProperties = new() { "ResponseHeader", "Sensors" };

        CheckProperties<MessageFormats.HostServices.Sensor.SensorsAvailableResponse>(expectedProperties);
    }

    [Fact]
    public async Task TaskingPreCheckRequest() {
        // Arrange
        List<string> expectedProperties = new() { "RequestHeader", "SensorID", "RequestTime", "ExpirationTime", "RequestData" };

        CheckProperties<MessageFormats.HostServices.Sensor.TaskingPreCheckRequest>(expectedProperties);
    }

    [Fact]
    public async Task TaskingPreCheckResponse() {
        // Arrange
        List<string> expectedProperties = new() { "ResponseHeader", "SensorID", "ResponseData" };

        CheckProperties<MessageFormats.HostServices.Sensor.TaskingPreCheckResponse>(expectedProperties);
    }

    [Fact]
    public async Task TaskingRequest() {
        // Arrange
        List<string> expectedProperties = new() { "RequestHeader", "SensorID", "RequestTime", "ExpirationTime", "RequestData" };

        CheckProperties<MessageFormats.HostServices.Sensor.TaskingRequest>(expectedProperties);
    }

    [Fact]
    public async Task TaskingResponse() {
        // Arrange
        List<string> expectedProperties = new() { "ResponseHeader", "SensorID", "ResponseData" };
        CheckProperties<MessageFormats.HostServices.Sensor.TaskingResponse>(expectedProperties);
    }


    private static void CheckProperties<T>(List<string> expectedProperties) where T : IMessage, new() {
        T testMessage = new T();
        List<string> actualProperties = testMessage.Descriptor.Fields.InFieldNumberOrder().Select(field => field.PropertyName).ToList();

        Console.WriteLine($"......checking properties for {typeof(T)}");

        Console.WriteLine($".........expected properties: ({expectedProperties.Count}): {string.Join(",", expectedProperties)}");
        Console.WriteLine($".........actual properties: ({actualProperties.Count}): {string.Join(",", actualProperties)}");

        Assert.Equal(0, expectedProperties.Count(_prop => !actualProperties.Contains(_prop)));  // Check if there's any properties missing in the message
        Assert.Equal(0, actualProperties.Count(_prop => !expectedProperties.Contains(_prop)));  // Check if there's any properties we aren't expecting
    }

    private static void CheckEnumerator<T>(List<string> expectedEnumValues) where T : System.Enum {
        // Loop through and try to set all the enum values
        foreach (string enumValue in expectedEnumValues) {
            // This will throw a hard exception if we pass an item that doesn't work
            object? parsedEnum = System.Enum.Parse(typeof(T), enumValue);
            Assert.NotNull(parsedEnum);
        }

        // Make sure we don't have any extra values we didn't test
        int currentEnumCount = System.Enum.GetNames(typeof(T)).Length;

        Assert.Equal(expectedEnumValues.Count, currentEnumCount);
    }
}
