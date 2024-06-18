namespace Microsoft.Azure.SpaceFx.HostServices.Sensor;
public partial class Utils {
    public class PluginDelegates {
        private readonly ILogger<PluginDelegates> _logger;
        private readonly List<Core.Models.PLUG_IN> _plugins;
        public PluginDelegates(ILogger<PluginDelegates> logger, IServiceProvider serviceProvider) {
            _logger = logger;
            _plugins = serviceProvider.GetService<List<Core.Models.PLUG_IN>>() ?? new List<Core.Models.PLUG_IN>();
        }

        internal MessageFormats.HostServices.Sensor.SensorsAvailableRequest? SensorsAvailableRequest((MessageFormats.HostServices.Sensor.SensorsAvailableRequest? input_request, Plugins.PluginBase plugin) input) {
            const string methodName = nameof(input.plugin.SensorsAvailableRequest);

            if (input.input_request is null || input.input_request is default(MessageFormats.HostServices.Sensor.SensorsAvailableRequest)) {
                _logger.LogDebug("Plugin {pluginName} / {pluginMethod}: Received empty input.  Returning empty results", input.plugin.ToString(), methodName);
                return input.input_request;
            }
            _logger.LogDebug("Plugin {pluginMethod}: START", methodName);

            try {
                Task<MessageFormats.HostServices.Sensor.SensorsAvailableRequest?> pluginTask = input.plugin.SensorsAvailableRequest(input_request: input.input_request);
                pluginTask.Wait();
                input.input_request = pluginTask.Result;
            } catch (Exception ex) {
                _logger.LogError("Plugin {pluginName} / {pluginMethod}: Error: {errorMessage}", input.plugin.ToString(), methodName, ex.Message);
            }

            _logger.LogDebug("Plugin {pluginName} / {pluginMethod}: END", input.plugin.ToString(), methodName);
            return input.input_request;
        }

        internal (MessageFormats.HostServices.Sensor.SensorsAvailableRequest? output_request, MessageFormats.HostServices.Sensor.SensorsAvailableResponse? output_response) SensorsAvailableResponse((MessageFormats.HostServices.Sensor.SensorsAvailableRequest? input_request, MessageFormats.HostServices.Sensor.SensorsAvailableResponse? input_response, Plugins.PluginBase plugin) input) {
            const string methodName = nameof(input.plugin.SensorsAvailableResponse);
            if (input.input_request is null || input.input_request is default(MessageFormats.HostServices.Sensor.SensorsAvailableRequest)) {
                _logger.LogDebug("Plugin {Plugin_Name} / {methodName}: Received empty input.  Returning empty results", input.plugin.ToString(), methodName);
                return (input.input_request, input.input_response);
            }
            _logger.LogDebug("Plugin {Plugin_Name} / {methodName}: START", input.plugin.ToString(), methodName);

            try {
                Task<(MessageFormats.HostServices.Sensor.SensorsAvailableRequest? output_request, MessageFormats.HostServices.Sensor.SensorsAvailableResponse? output_response)> pluginTask = input.plugin.SensorsAvailableResponse(input_request: input.input_request, input_response: input.input_response);
                pluginTask.Wait();

                input.input_request = pluginTask.Result.output_request;
                input.input_response = pluginTask.Result.output_response;
            } catch (Exception ex) {
                _logger.LogError("Error in plugin '{Plugin_Name}:{methodName}'.  Error: {errMsg}", input.plugin.ToString(), methodName, ex.Message);
            }

            _logger.LogDebug("Plugin {Plugin_Name} / {methodName}: END", input.plugin.ToString(), methodName);
            return (input.input_request, input.input_response);
        }

        internal MessageFormats.HostServices.Sensor.TaskingPreCheckRequest? TaskingPreCheckRequest((MessageFormats.HostServices.Sensor.TaskingPreCheckRequest? input_request, Plugins.PluginBase plugin) input) {
            const string methodName = nameof(input.plugin.TaskingPreCheckRequest);

            if (input.input_request is null || input.input_request is default(MessageFormats.HostServices.Sensor.TaskingPreCheckRequest)) {
                _logger.LogDebug("Plugin {pluginName} / {pluginMethod}: Received empty input.  Returning empty results", input.plugin.ToString(), methodName);
                return input.input_request;
            }
            _logger.LogDebug("Plugin {pluginMethod}: START", methodName);

            try {
                Task<MessageFormats.HostServices.Sensor.TaskingPreCheckRequest?> pluginTask = input.plugin.TaskingPreCheckRequest(input_request: input.input_request);
                pluginTask.Wait();
                input.input_request = pluginTask.Result;
            } catch (Exception ex) {
                _logger.LogError("Plugin {pluginName} / {pluginMethod}: Error: {errorMessage}", input.plugin.ToString(), methodName, ex.Message);
            }

            _logger.LogDebug("Plugin {pluginName} / {pluginMethod}: END", input.plugin.ToString(), methodName);
            return input.input_request;
        }

        internal (MessageFormats.HostServices.Sensor.TaskingPreCheckRequest? output_request, MessageFormats.HostServices.Sensor.TaskingPreCheckResponse? output_response) TaskingPreCheckResponse((MessageFormats.HostServices.Sensor.TaskingPreCheckRequest? input_request, MessageFormats.HostServices.Sensor.TaskingPreCheckResponse? input_response, Plugins.PluginBase plugin) input) {
            const string methodName = nameof(input.plugin.TaskingPreCheckResponse);
            if (input.input_request is null || input.input_request is default(MessageFormats.HostServices.Sensor.TaskingPreCheckRequest)) {
                _logger.LogDebug("Plugin {Plugin_Name} / {methodName}: Received empty input.  Returning empty results", input.plugin.ToString(), methodName);
                return (input.input_request, input.input_response);
            }
            _logger.LogDebug("Plugin {Plugin_Name} / {methodName}: START", input.plugin.ToString(), methodName);

            try {
                Task<(MessageFormats.HostServices.Sensor.TaskingPreCheckRequest? output_request, MessageFormats.HostServices.Sensor.TaskingPreCheckResponse? output_response)> pluginTask = input.plugin.TaskingPreCheckResponse(input_request: input.input_request, input_response: input.input_response);
                pluginTask.Wait();

                input.input_request = pluginTask.Result.output_request;
                input.input_response = pluginTask.Result.output_response;
            } catch (Exception ex) {
                _logger.LogError("Error in plugin '{Plugin_Name}:{methodName}'.  Error: {errMsg}", input.plugin.ToString(), methodName, ex.Message);
            }

            _logger.LogDebug("Plugin {Plugin_Name} / {methodName}: END", input.plugin.ToString(), methodName);
            return (input.input_request, input.input_response);
        }

        internal MessageFormats.HostServices.Sensor.TaskingRequest? TaskingRequest((MessageFormats.HostServices.Sensor.TaskingRequest? input_request, Plugins.PluginBase plugin) input) {
            const string methodName = nameof(input.plugin.TaskingRequest);

            if (input.input_request is null || input.input_request is default(MessageFormats.HostServices.Sensor.TaskingRequest)) {
                _logger.LogDebug("Plugin {pluginName} / {pluginMethod}: Received empty input.  Returning empty results", input.plugin.ToString(), methodName);
                return input.input_request;
            }
            _logger.LogDebug("Plugin {pluginMethod}: START", methodName);

            try {
                Task<MessageFormats.HostServices.Sensor.TaskingRequest?> pluginTask = input.plugin.TaskingRequest(input_request: input.input_request);
                pluginTask.Wait();
                input.input_request = pluginTask.Result;
            } catch (Exception ex) {
                _logger.LogError("Plugin {pluginName} / {pluginMethod}: Error: {errorMessage}", input.plugin.ToString(), methodName, ex.Message);
            }

            _logger.LogDebug("Plugin {pluginName} / {pluginMethod}: END", input.plugin.ToString(), methodName);
            return input.input_request;
        }

        internal (MessageFormats.HostServices.Sensor.TaskingRequest? output_request, MessageFormats.HostServices.Sensor.TaskingResponse? output_response) TaskingResponse((MessageFormats.HostServices.Sensor.TaskingRequest? input_request, MessageFormats.HostServices.Sensor.TaskingResponse? input_response, Plugins.PluginBase plugin) input) {
            const string methodName = nameof(input.plugin.TaskingResponse);
            if (input.input_request is null || input.input_request is default(MessageFormats.HostServices.Sensor.TaskingRequest)) {
                _logger.LogDebug("Plugin {Plugin_Name} / {methodName}: Received empty input.  Returning empty results", input.plugin.ToString(), methodName);
                return (input.input_request, input.input_response);
            }
            _logger.LogDebug("Plugin {Plugin_Name} / {methodName}: START", input.plugin.ToString(), methodName);

            try {
                Task<(MessageFormats.HostServices.Sensor.TaskingRequest? output_request, MessageFormats.HostServices.Sensor.TaskingResponse? output_response)> pluginTask = input.plugin.TaskingResponse(input_request: input.input_request, input_response: input.input_response);
                pluginTask.Wait();

                input.input_request = pluginTask.Result.output_request;
                input.input_response = pluginTask.Result.output_response;
            } catch (Exception ex) {
                _logger.LogError("Error in plugin '{Plugin_Name}:{methodName}'.  Error: {errMsg}", input.plugin.ToString(), methodName, ex.Message);
            }

            _logger.LogDebug("Plugin {Plugin_Name} / {methodName}: END", input.plugin.ToString(), methodName);
            return (input.input_request, input.input_response);
        }

        internal MessageFormats.HostServices.Sensor.SensorData? SensorData((MessageFormats.HostServices.Sensor.SensorData? input_request, Plugins.PluginBase plugin) input) {
            const string methodName = nameof(input.plugin.SensorData);

            if (input.input_request is null || input.input_request is default(MessageFormats.HostServices.Sensor.SensorData)) {
                _logger.LogDebug("Plugin {pluginName} / {pluginMethod}: Received empty input.  Returning empty results", input.plugin.ToString(), methodName);
                return input.input_request;
            }
            _logger.LogDebug("Plugin {pluginMethod}: START", methodName);

            try {
                Task<MessageFormats.HostServices.Sensor.SensorData?> pluginTask = input.plugin.SensorData(input_request: input.input_request);
                pluginTask.Wait();
                input.input_request = pluginTask.Result;
            } catch (Exception ex) {
                _logger.LogError("Plugin {pluginName} / {pluginMethod}: Error: {errorMessage}", input.plugin.ToString(), methodName, ex.Message);
            }

            _logger.LogDebug("Plugin {pluginName} / {pluginMethod}: END", input.plugin.ToString(), methodName);
            return input.input_request;
        }
    }
}