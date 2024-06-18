namespace Microsoft.Azure.SpaceFx.HostServices.Sensor.IntegrationTests.Tests;

[Collection(nameof(TestSharedContext))]
public class SensorDataTests : IClassFixture<TestSharedContext> {
    readonly TestSharedContext _context;

    public SensorDataTests(TestSharedContext context) {
        _context = context;
    }

    [Fact]
    public async Task SensorDataReceived() {
        const string testName = nameof(SensorDataReceived);
        DateTime maxTimeToWaitForMsg;

        // Preset the requestId so we don't cross wires with any other tests
        string requestId = Guid.NewGuid().ToString();

        Console.WriteLine($"[{testName}] - START");

        // Set this to null to make it easier for asynchronously tracking
        Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.TaskingResponse? response = null;
        Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.SensorData? dataResponse = null;

        // Register a callback event to catch the response
        MessageHandler<MessageFormats.HostServices.Sensor.TaskingResponse>.MessageReceivedEvent += (object? _, MessageFormats.HostServices.Sensor.TaskingResponse _response) => {
            if (_response.ResponseHeader.TrackingId == requestId) response = _response;
        };

        MessageHandler<MessageFormats.HostServices.Sensor.SensorData>.MessageReceivedEvent += (object? _, MessageFormats.HostServices.Sensor.SensorData _dataResponse) => {
            if (_dataResponse.SensorID == TestSharedContext.SENSOR_ID) dataResponse = _dataResponse;
        };


        MessageFormats.HostServices.Sensor.TaskingRequest request = new() {
            RequestHeader = new() {
                TrackingId = requestId
            },
            SensorID = TestSharedContext.SENSOR_ID
        };

        Console.WriteLine($"[{testName}] - Sending tasking request");
        await TestSharedContext.SPACEFX_CLIENT.DirectToApp(appId: TestSharedContext.TARGET_SVC_APP_ID, message: request);

        // Reset our message deadline so we don't end early (incase the above code took a little bit to run)
        maxTimeToWaitForMsg = DateTime.Now.Add(TestSharedContext.MAX_TIMESPAN_TO_WAIT_FOR_MSG);
        Console.WriteLine($"[{testName}] - Setting response message deadline for {maxTimeToWaitForMsg:yyyy-MM-dd'T'HH:mm:ss'Z'} ({TestSharedContext.MAX_TIMESPAN_TO_WAIT_FOR_MSG.TotalSeconds} seconds)");

        // Wait here for the message to get sent back
        while (response == null && DateTime.Now <= maxTimeToWaitForMsg) {
            System.Threading.Thread.Sleep(200);
        }

        if (response == null) {
            //  We timed out waiting for the service to send us our message.  Throw an error
            Console.WriteLine($"[{testName}] - Timed out waiting for {nameof(response)}.  :(");
            throw new TimeoutException($"Failed to hear {TestSharedContext.TARGET_SVC_APP_ID} message after {TestSharedContext.MAX_TIMESPAN_TO_WAIT_FOR_MSG.TotalSeconds} seconds.  Please check that {TestSharedContext.TARGET_SVC_APP_ID} is deployed");
        }

        Console.WriteLine($"[{testName}] - Got a response!  Result status: {response.ResponseHeader.Status}");

        Assert.Equal(Microsoft.Azure.SpaceFx.MessageFormats.Common.StatusCodes.Successful, response.ResponseHeader.Status);
        Assert.Equal(TestSharedContext.SENSOR_ID, response.SensorID);
        Console.WriteLine($"[{testName}] - END");

        // Reset our message deadline so we don't end early (incase the above code took a little bit to run)
        maxTimeToWaitForMsg = DateTime.Now.Add(TestSharedContext.MAX_TIMESPAN_TO_WAIT_FOR_MSG);
        Console.WriteLine($"[{testName}] - Setting Sensor Data message deadline for {maxTimeToWaitForMsg:yyyy-MM-dd'T'HH:mm:ss'Z'} ({TestSharedContext.MAX_TIMESPAN_TO_WAIT_FOR_MSG.TotalSeconds} seconds)");

        // Wait here for the message to get sent back
        while (dataResponse == null && DateTime.Now <= maxTimeToWaitForMsg) {
            System.Threading.Thread.Sleep(200);
        }

        if (dataResponse == null) {
            //  We timed out waiting for the service to send us our message.  Throw an error
            Console.WriteLine($"[{testName}] - Timed out waiting for {nameof(dataResponse)}.  :(");
            throw new TimeoutException($"Failed to hear {TestSharedContext.TARGET_SVC_APP_ID} message after {TestSharedContext.MAX_TIMESPAN_TO_WAIT_FOR_MSG.TotalSeconds} seconds.  Please check that {TestSharedContext.TARGET_SVC_APP_ID} is deployed");
        }

        Assert.Equal(Microsoft.Azure.SpaceFx.MessageFormats.Common.StatusCodes.Successful, dataResponse.ResponseHeader.Status);

        Console.WriteLine($"[{testName}] - END");
    }

    [Fact]
    /// <summary>
    /// Verify we can get Recurring Sensor Data
    /// </summary>
    public async Task SensorDataRecurring() {
        const string testName = nameof(SensorDataReceived);
        DateTime maxTimeToWaitForMsg;

        // Preset the requestId so we don't cross wires with any other tests
        string requestId = Guid.NewGuid().ToString();

        Console.WriteLine($"[{testName}] - START");

        // Set this to null to make it easier for asynchronously tracking
        Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.SensorData? dataResponse = null;

        MessageHandler<MessageFormats.HostServices.Sensor.SensorData>.MessageReceivedEvent += (object? _, MessageFormats.HostServices.Sensor.SensorData _dataResponse) => {
            if (string.Equals(_dataResponse.SensorID, TestSharedContext.SENSOR_TEMPERATURE_ID, StringComparison.OrdinalIgnoreCase)) dataResponse = _dataResponse;
        };

        MessageFormats.HostServices.Sensor.TaskingRequest request = new() {
            RequestHeader = new() {
                TrackingId = requestId,
            },
            SensorID = TestSharedContext.SENSOR_TEMPERATURE_ID
        };

        Console.WriteLine($"[{testName}] - Sending tasking request for '{TestSharedContext.SENSOR_TEMPERATURE_ID}'");
        await TestSharedContext.SPACEFX_CLIENT.DirectToApp(appId: TestSharedContext.TARGET_SVC_APP_ID, message: request);


        // Reset our message deadline so we don't end early (incase the above code took a little bit to run)
        maxTimeToWaitForMsg = DateTime.Now.Add(TestSharedContext.MAX_TIMESPAN_TO_WAIT_FOR_MSG);
        Console.WriteLine($"[{testName}] - Setting Sensor Data message deadline for {maxTimeToWaitForMsg:yyyy-MM-dd'T'HH:mm:ss'Z'} ({TestSharedContext.MAX_TIMESPAN_TO_WAIT_FOR_MSG.TotalSeconds} seconds)");

        // Wait here for the message to get sent back
        while (dataResponse == null && DateTime.Now <= maxTimeToWaitForMsg) {
            System.Threading.Thread.Sleep(200);
        }

        if (dataResponse == null) {
            //  We timed out waiting for the service to send us our message.  Throw an error
            Console.WriteLine($"[{testName}] - Timed out waiting for {nameof(dataResponse)}.  :(");
            throw new TimeoutException($"Failed to hear {TestSharedContext.TARGET_SVC_APP_ID} message after {TestSharedContext.MAX_TIMESPAN_TO_WAIT_FOR_MSG.TotalSeconds} seconds.  Please check that {TestSharedContext.TARGET_SVC_APP_ID} is deployed");
        }

        Assert.Equal(MessageFormats.Common.StatusCodes.Successful, dataResponse.ResponseHeader.Status);

        Console.WriteLine($"[{testName}] - END");
    }
}