namespace PayloadApp.DebugClient;

public class MessageSender : BackgroundService {
    private readonly ILogger<MessageSender> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Microsoft.Azure.SpaceFx.Core.Client _client;
    private readonly string _appId;
    private readonly string _hostSvcAppId;

    public MessageSender(ILogger<MessageSender> logger, IServiceProvider serviceProvider) {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _client = _serviceProvider.GetService<Microsoft.Azure.SpaceFx.Core.Client>() ?? throw new NullReferenceException($"{nameof(Microsoft.Azure.SpaceFx.Core.Client)} is null");
        _appId = _client.GetAppID().Result;
        _hostSvcAppId = _appId.Replace("-client", "");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {

        using (var scope = _serviceProvider.CreateScope()) {
            System.Threading.Thread.Sleep(3000);

            Boolean SVC_ONLINE = _client.ServicesOnline().Any(pulse => pulse.AppId.Equals(_hostSvcAppId, StringComparison.CurrentCultureIgnoreCase));

            // await PluginConfigurationRequest();
            await QuerySensorsAvailable();
            await SensorTaskingPreCheck();
            await SensorTasking();
        }

        _logger.LogInformation("MessageSender sending repeatSensorData: {time}", DateTimeOffset.Now);
        while (!stoppingToken.IsCancellationRequested) {
            using (var scope = _serviceProvider.CreateScope()) {
                Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.SensorData sensorData = new() {
                    SensorID = "Testing"
                };

                await _client.DirectToApp(_hostSvcAppId, sensorData);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    private void ListHeardServices() {
        System.Threading.Thread.Sleep(3000);
        _logger.LogInformation("Apps Online:");
        _client.ServicesOnline().ForEach((pulse) => Console.WriteLine($"...{pulse.AppId}..."));
    }


    private async Task QuerySensorsAvailable() {
        _logger.LogInformation("Querying Available Sensors:");
        Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.SensorsAvailableRequest request = new() {
            RequestHeader = new() {
                TrackingId = Guid.NewGuid().ToString()
            }
        };

        await _client.DirectToApp(appId: _hostSvcAppId, message: request);
    }

    private async Task SensorTaskingPreCheck() {
        Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.TaskingPreCheckRequest request = new() {
            RequestHeader = new() {
                TrackingId = Guid.NewGuid().ToString()
            },
            SensorID = "DemoTemperatureSensor"
        };

        await _client.DirectToApp(appId: _hostSvcAppId, message: request);
    }

    private async Task SensorTasking() {

        Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.TaskingRequest request = new() {
            RequestHeader = new() {
                TrackingId = Guid.NewGuid().ToString()
            },
            SensorID = "DemoTemperatureSensor"
        };


        await _client.DirectToApp(appId: _hostSvcAppId, message: request);
    }

    private async Task PluginConfigurationRequest() {
        PluginConfigurationRequest request = new() {
            RequestHeader = new() {
                TrackingId = Guid.NewGuid().ToString()
            }
        };

        _logger.LogInformation("Sending Plugin Configuration Request");

        VerifyTopicHasSensorId(request, "Testing");

        await _client.DirectToApp(appId: _hostSvcAppId, message: request);
    }

    private void VerifyTopicHasSensorId(IMessage message, string topic) {
        string sensorId = string.Empty;
        _logger.LogInformation(message.Descriptor.FullName);
    }

}
