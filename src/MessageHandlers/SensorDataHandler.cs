using Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor;

namespace Microsoft.Azure.SpaceFx.HostServices.Sensor;

public partial class MessageHandler<T> {
    private void SensorDataHandler(MessageFormats.HostServices.Sensor.SensorData? message, MessageFormats.Common.DirectToApp fullMessage) {
        if (message == null) return;
        using (var scope = _serviceProvider.CreateScope()) {
            DateTime maxTimeToWait = DateTime.Now.Add(TimeSpan.FromMilliseconds(_appConfig.MESSAGE_RESPONSE_TIMEOUT_MS));

            _logger.LogInformation("Processing message type '{messageType}' from '{sourceApp}' (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.GetType().Name, fullMessage.SourceAppId, message.ResponseHeader.TrackingId, message.ResponseHeader.CorrelationId, message.ResponseHeader.Status);

            _logger.LogDebug("Passing message '{messageType}' to plugins (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.GetType().Name, message.ResponseHeader.TrackingId, message.ResponseHeader.CorrelationId, message.ResponseHeader.Status);

            MessageFormats.HostServices.Sensor.SensorData? pluginResult =
               _pluginLoader.CallPlugins<MessageFormats.HostServices.Sensor.SensorData?, Plugins.PluginBase>(
                   orig_request: message,
                   pluginDelegate: _pluginDelegates.SensorData);

            _logger.LogDebug("Plugins finished processing '{messageType}' (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.GetType().Name, message.ResponseHeader.TrackingId, message.ResponseHeader.CorrelationId, message.ResponseHeader.Status);

            // Update the request if our plugins changed it
            if (pluginResult == null) {
                _logger.LogInformation("Plugins nullified '{messageType}'.  Dropping Message (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.GetType().Name, message.ResponseHeader.TrackingId, message.ResponseHeader.CorrelationId, message.ResponseHeader.Status);
                return;
            }

            message = pluginResult;

            // Message is populated with the origin Tasking Tracking Id.  Pull it to calculate the destination app Id
            if (!string.IsNullOrWhiteSpace(message.TaskingTrackingId)) {
                _logger.LogDebug("Message '{messageType}' has TaskingTrackingId '{taskingTrackingId}' (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.GetType().Name, message.TaskingTrackingId, message.ResponseHeader.TrackingId, message.ResponseHeader.CorrelationId, message.ResponseHeader.Status);

                TaskingRequest? orig_request = _client.GetCacheItem<TaskingRequest>(cacheItemName: message.TaskingTrackingId).Result;

                if (orig_request == null) {
                    _logger.LogError("No TaskingTrackingId of '{taskingTrackingId}' found in cache.  Unable to route message.  Dropping message. (trackingId: '{trackingId}' / correlationId: '{correlationId}'/ status: '{status}')", message.TaskingTrackingId, message.ResponseHeader.TrackingId, message.ResponseHeader.CorrelationId, message.ResponseHeader.Status);
                    return;
                }

                message.DestinationAppId = orig_request.RequestHeader.AppId;
                _logger.LogDebug("Found app '{original_app_id}' for TaskingTrackingId '{taskingTrackingId}'.  (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.DestinationAppId, message.TaskingTrackingId, message.ResponseHeader.TrackingId, message.ResponseHeader.CorrelationId, message.ResponseHeader.Status);
            }


            // TODO: Add async service to check if services are online and if not, remove their permissions

            // No destination App ID, and no tracking ID specified.  Route the message to any subscribers
            if (string.IsNullOrWhiteSpace(message.DestinationAppId)) {
                _logger.LogTrace("Message '{messageType}' is a broadcast (message has no DestinationAppId) (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.GetType().Name, message.ResponseHeader.TrackingId, message.ResponseHeader.CorrelationId, message.ResponseHeader.Status);


                // Retrieve the items from cache
                ListValue? sensor_subscriptions = _client.GetCacheItem<ListValue>(cacheItemName: CACHE_KEYS.SENSOR_SUBSCRIPTIONS_PREFIX + message.SensorID).Result ?? new ListValue();

                // Loop through and broadcast out
                if (sensor_subscriptions.Values.Count > 0) {
                    foreach (Google.Protobuf.WellKnownTypes.Value sensor_subscription in sensor_subscriptions.Values) {
                        _logger.LogInformation("Sending message '{messageType}' to '{appId}'  (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.GetType().Name, sensor_subscription.StringValue, message.ResponseHeader.TrackingId, message.ResponseHeader.CorrelationId, message.ResponseHeader.Status);
                        _client.DirectToApp(appId: sensor_subscription.StringValue, message: message).Wait();
                    }
                }
            } else {
                    _logger.LogInformation("Sending message '{messageType}' to '{appId}'  (trackingId: '{trackingId}' / correlationId: '{correlationId}' / status: '{status}')", message.GetType().Name, message.DestinationAppId, message.ResponseHeader.TrackingId, message.ResponseHeader.CorrelationId, message.ResponseHeader.Status);
                    _client.DirectToApp(appId: message.DestinationAppId, message: message).Wait();
            }
        };
    }
}
