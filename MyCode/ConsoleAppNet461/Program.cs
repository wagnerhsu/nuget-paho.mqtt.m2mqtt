using CommonLib;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using System;
using System.Threading;
using Wagner.Framework.WxString.Extensions;

namespace ConsoleAppNet461
{
    class Program
    {
        static ILogger Logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            TestPublish();
            Console.WriteLine("Press ENTER to exit");
            Console.Read();
        }

        private static void TestPublish()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();
            var mqttSetting = configuration.GetSection("MqttSetting").Get<Dgw.Netstandard.Common.Models.MqttConfigurationModel>();
            var json = JsonConvert.SerializeObject(mqttSetting);
            Logger.Debug(json);
            var payLoad = json.AsByteArray();
            MqttPublishService mqttPublishService = new MqttPublishService(mqttSetting.Server.Ip, mqttSetting.Server.Port, mqttSetting.Client.Id);
            for (int i = 0; i < 1000;i++)
            {
                Logger.Debug($"Publish {i}");
                
                mqttPublishService.Publish(mqttSetting.Client.Topic, payLoad);
                Thread.Sleep(configuration.GetValue<int>("Interval"));
            }
            Logger.Debug("Done!");
        }
    }
}
