namespace Microsoft.Azure.SpaceFx.HostServices.Sensor;

public partial class MessageHandler<T> : Microsoft.Azure.SpaceFx.Core.IMessageHandler<T> where T : notnull {
    private readonly ILogger<MessageHandler<T>> _logger;
    public static EventHandler<T>? MessageReceivedEvent;
    private readonly Utils.PluginDelegates _pluginDelegates;
    private readonly Microsoft.Azure.SpaceFx.Core.Services.PluginLoader _pluginLoader;
    private readonly IServiceProvider _serviceProvider;
    private readonly Core.Client _client;
    private readonly Models.APP_CONFIG _appConfig;

    public MessageHandler(ILogger<MessageHandler<T>> logger, Utils.PluginDelegates pluginDelegates, Microsoft.Azure.SpaceFx.Core.Services.PluginLoader pluginLoader, IServiceProvider serviceProvider, Core.Client client) {
        _logger = logger;
        _pluginDelegates = pluginDelegates;
        _pluginLoader = pluginLoader;
        _serviceProvider = serviceProvider;
        _client = client;

        _appConfig = new Models.APP_CONFIG();
    }

    public void MessageReceived(T message, MessageFormats.Common.DirectToApp fullMessage) => Task.Run(() => {
        using (var scope = _serviceProvider.CreateScope()) {

            if (message == null || EqualityComparer<T>.Default.Equals(message, default)) {
                _logger.LogInformation("Received empty message '{messageType}' from '{appId}'.  Discarding message.", typeof(T).Name, fullMessage.SourceAppId);
                return;
            }

            switch (typeof(T).Name) {
                case string messageType when messageType.Equals(typeof(MessageFormats.HostServices.Sensor.SensorData).Name, StringComparison.CurrentCultureIgnoreCase):
                    SensorDataHandler(message: message as MessageFormats.HostServices.Sensor.SensorData, fullMessage: fullMessage);
                    break;
                case string messageType when messageType.Equals(typeof(MessageFormats.HostServices.Sensor.SensorsAvailableRequest).Name, StringComparison.CurrentCultureIgnoreCase):
                    SensorsAvailableRequestHandler(message: message as MessageFormats.HostServices.Sensor.SensorsAvailableRequest, fullMessage: fullMessage);
                    break;
                case string messageType when messageType.Equals(typeof(MessageFormats.HostServices.Sensor.SensorsAvailableResponse).Name, StringComparison.CurrentCultureIgnoreCase):
                    if (message == null) return;

                    _logger.LogInformation("Processing message type '{messageType}' from '{sourceApp}' (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.GetType().Name, fullMessage.SourceAppId, (message as MessageFormats.HostServices.Sensor.SensorsAvailableResponse)?.ResponseHeader.TrackingId, (message as MessageFormats.HostServices.Sensor.SensorsAvailableResponse)?.ResponseHeader.CorrelationId, (message as MessageFormats.HostServices.Sensor.SensorsAvailableResponse)?.ResponseHeader.Status);

                    if (MessageReceivedEvent == null) break;

                    foreach (Delegate handler in MessageReceivedEvent.GetInvocationList()) {
                        Task.Factory.StartNew(
                            () => handler.DynamicInvoke(fullMessage.ResponseHeader.AppId, message));
                    }

                    break;
                case string messageType when messageType.Equals(typeof(MessageFormats.HostServices.Sensor.TaskingPreCheckRequest).Name, StringComparison.CurrentCultureIgnoreCase):
                    TaskingPreCheckRequestHandler(message: message as MessageFormats.HostServices.Sensor.TaskingPreCheckRequest, fullMessage: fullMessage);
                    break;
                case string messageType when messageType.Equals(typeof(MessageFormats.HostServices.Sensor.TaskingPreCheckResponse).Name, StringComparison.CurrentCultureIgnoreCase):
                    _logger.LogInformation("Processing message type '{messageType}' from '{sourceApp}' (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.GetType().Name, fullMessage.SourceAppId, (message as MessageFormats.HostServices.Sensor.TaskingPreCheckResponse)?.ResponseHeader.TrackingId, (message as MessageFormats.HostServices.Sensor.TaskingPreCheckResponse)?.ResponseHeader.CorrelationId, (message as MessageFormats.HostServices.Sensor.TaskingPreCheckResponse)?.ResponseHeader.Status);

                    if (MessageReceivedEvent == null) break;

                    foreach (Delegate handler in MessageReceivedEvent.GetInvocationList()) {
                        Task.Factory.StartNew(
                            () => handler.DynamicInvoke(fullMessage.ResponseHeader.AppId, message));
                    }
                    break;
                case string messageType when messageType.Equals(typeof(MessageFormats.HostServices.Sensor.TaskingRequest).Name, StringComparison.CurrentCultureIgnoreCase):
                    TaskingRequestHandler(message: message as MessageFormats.HostServices.Sensor.TaskingRequest, fullMessage: fullMessage);
                    break;
                case string messageType when messageType.Equals(typeof(MessageFormats.HostServices.Sensor.TaskingResponse).Name, StringComparison.CurrentCultureIgnoreCase):
                    _logger.LogInformation("Processing message type '{messageType}' from '{sourceApp}' (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.GetType().Name, fullMessage.SourceAppId, (message as MessageFormats.HostServices.Sensor.TaskingResponse)?.ResponseHeader.TrackingId, (message as MessageFormats.HostServices.Sensor.TaskingResponse)?.ResponseHeader.CorrelationId, (message as MessageFormats.HostServices.Sensor.TaskingResponse)?.ResponseHeader.Status);

                    if (MessageReceivedEvent == null) break;

                    foreach (Delegate handler in MessageReceivedEvent.GetInvocationList()) {
                        Task.Factory.StartNew(
                            () => handler.DynamicInvoke(fullMessage.ResponseHeader.AppId, message));
                    }
                    break;
            }
        }
    });
}
