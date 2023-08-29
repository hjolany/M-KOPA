using Newtonsoft.Json;
using RabbitMQ.Client;
using SMSMicroService.Gateway.Interface;
using System.Text;
using Microsoft.AspNetCore.Connections;
using SMSMicroService.Infrastructures;
using SMSMicroService.Infrastructures.Extensions;
using IConnectionFactory = RabbitMQ.Client.IConnectionFactory;

namespace SMSMicroService.Gateway
{
    public class RabbitMqEventBusGateway<T> : IEventBusGateway<T>
    {
        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public RabbitMqEventBusGateway(string exchangeName, string routingKey,
            IConnection connection)
        {
            _exchangeName = exchangeName;
            _routingKey = routingKey;
            _connection = connection;
            _channel = connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Topic);
        }
        public async Task Publish(T data)
        {
            try
            {
                if (!_connection.IsOpen)
                    throw new ConnectionAbortedException();

                var message = JsonConvert.SerializeObject(data);
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(exchange: _exchangeName, routingKey: _routingKey, basicProperties: null, body: body);
            }
            catch (Exception ex)
            {
                throw new CriticalException(ex.GetFullMessage());
            }
        }
    }
}
