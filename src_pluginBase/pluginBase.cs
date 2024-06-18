namespace Microsoft.Azure.SpaceFx.HostServices.Sensor.Plugins;
public abstract class PluginBase : Microsoft.Azure.SpaceFx.Core.IPluginBase, IPluginBase {
    public abstract ILogger Logger { get; set; }
    public abstract Task BackgroundTask();
    public abstract void ConfigureLogging(ILoggerFactory loggerFactory);
    public abstract Task<MessageFormats.Common.PluginHealthCheckResponse> PluginHealthCheckResponse();

    // Sensor Service Stuff
    public abstract Task<MessageFormats.HostServices.Sensor.SensorsAvailableRequest?> SensorsAvailableRequest(MessageFormats.HostServices.Sensor.SensorsAvailableRequest? input_request);
    public abstract Task<(MessageFormats.HostServices.Sensor.SensorsAvailableRequest?, MessageFormats.HostServices.Sensor.SensorsAvailableResponse?)> SensorsAvailableResponse(MessageFormats.HostServices.Sensor.SensorsAvailableRequest? input_request, MessageFormats.HostServices.Sensor.SensorsAvailableResponse? input_response);
    public abstract Task<MessageFormats.HostServices.Sensor.TaskingPreCheckRequest?> TaskingPreCheckRequest(MessageFormats.HostServices.Sensor.TaskingPreCheckRequest? input_request);
    public abstract Task<(MessageFormats.HostServices.Sensor.TaskingPreCheckRequest?, MessageFormats.HostServices.Sensor.TaskingPreCheckResponse?)> TaskingPreCheckResponse(MessageFormats.HostServices.Sensor.TaskingPreCheckRequest? input_request, MessageFormats.HostServices.Sensor.TaskingPreCheckResponse? input_response);
    public abstract Task<MessageFormats.HostServices.Sensor.TaskingRequest?> TaskingRequest(MessageFormats.HostServices.Sensor.TaskingRequest? input_request);
    public abstract Task<(MessageFormats.HostServices.Sensor.TaskingRequest?, MessageFormats.HostServices.Sensor.TaskingResponse?)> TaskingResponse(MessageFormats.HostServices.Sensor.TaskingRequest? input_request, MessageFormats.HostServices.Sensor.TaskingResponse? input_response);
    public abstract Task<MessageFormats.HostServices.Sensor.SensorData?> SensorData(MessageFormats.HostServices.Sensor.SensorData? input_request);

}

public interface IPluginBase {
    Task<MessageFormats.HostServices.Sensor.SensorsAvailableRequest?> SensorsAvailableRequest(MessageFormats.HostServices.Sensor.SensorsAvailableRequest? input_request);
    Task<(MessageFormats.HostServices.Sensor.SensorsAvailableRequest?, MessageFormats.HostServices.Sensor.SensorsAvailableResponse?)> SensorsAvailableResponse(MessageFormats.HostServices.Sensor.SensorsAvailableRequest? input_request, MessageFormats.HostServices.Sensor.SensorsAvailableResponse? input_response);
    Task<MessageFormats.HostServices.Sensor.TaskingPreCheckRequest?> TaskingPreCheckRequest(MessageFormats.HostServices.Sensor.TaskingPreCheckRequest? input_request);
    Task<(MessageFormats.HostServices.Sensor.TaskingPreCheckRequest?, MessageFormats.HostServices.Sensor.TaskingPreCheckResponse?)> TaskingPreCheckResponse(MessageFormats.HostServices.Sensor.TaskingPreCheckRequest? input_request, MessageFormats.HostServices.Sensor.TaskingPreCheckResponse? input_response);
    Task<MessageFormats.HostServices.Sensor.TaskingRequest?> TaskingRequest(MessageFormats.HostServices.Sensor.TaskingRequest? input_request);
    Task<(MessageFormats.HostServices.Sensor.TaskingRequest?, MessageFormats.HostServices.Sensor.TaskingResponse?)> TaskingResponse(MessageFormats.HostServices.Sensor.TaskingRequest? input_request, MessageFormats.HostServices.Sensor.TaskingResponse? input_response);
    Task<MessageFormats.HostServices.Sensor.SensorData?> SensorData(MessageFormats.HostServices.Sensor.SensorData? input_request);
}
