using NLog;
using System;
using System.Net;
using uPLibrary.Networking.M2Mqtt;

namespace CommonLib
{
    public class MqttPublishService
    {
        private static ILogger Logger = LogManager.GetCurrentClassLogger();
        private string _ip;
        private int _port;
        private string _clientId;
        private MqttClient _client;

        public MqttPublishService(string ip, int port, string clientId)
        {
            _ip = ip;
            _port = port;
            _clientId = clientId;
        }

        public void Publish(string topic, byte[] data)
        {
            if (_client == null)
            {
                _client = new MqttClient(_ip, _port, false, null, null, MqttSslProtocols.None);
                Logger.Info($"Connect MQTT server:{_ip}-{_port}");
                try
                {
                    _client.Connect(_clientId);
                }
                catch(Exception ex)
                {
                    Logger.Error(ex, "Error to connect");
                }
            }
            try
            {
                Logger.Debug($"Publish...{topic}");
                if (_client.IsConnected)
                    _client.Publish(topic, data);
                else throw new Exception("Offline");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error to publish");
                try
                {
                    _client.Disconnect();
                }
                catch { }
                _client = null;
            }
        }
    }
}