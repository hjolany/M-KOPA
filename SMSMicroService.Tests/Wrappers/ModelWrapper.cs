using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SMSMicroService.Tests.Wrappers.Interfaces;

namespace SMSMicroService.Tests.Wrappers
{
    public class ModelWrapper : IModelWrapper
    {
        private readonly IModel _model;

        public ModelWrapper()
        {

        }
        public ModelWrapper(IModel model)
        {
            _model = model;
        }
        public void Dispose()
        {
            Dispose();
        }

        public void Abort()
        {
            _model.Abort();
        }

        public void Abort(ushort replyCode, string replyText)
        {
            _model.Abort(replyCode, replyText);
        }

        public void BasicAck(ulong deliveryTag, bool multiple)
        {
            _model.BasicAck(deliveryTag, multiple);
        }

        public void BasicCancel(string consumerTag)
        {
            _model.BasicCancel(consumerTag);
        }

        public void BasicCancelNoWait(string consumerTag)
        {
            _model.BasicCancelNoWait(consumerTag);
        }

        public BasicGetResult BasicGet(string queue, bool autoAck)
        {
            return _model.BasicGet(queue, autoAck);
        }

        public void BasicNack(ulong deliveryTag, bool multiple, bool requeue)
        {
            _model.BasicNack(deliveryTag, multiple, requeue);
        }

        public void BasicPublish(string exchange, string routingKey, bool mandatory, IBasicProperties basicProperties, ReadOnlyMemory<byte> body)
        {
            _model.BasicPublish(exchange, routingKey, mandatory, basicProperties, body);
        }

        public void BasicQos(uint prefetchSize, ushort prefetchCount, bool global)
        {
            _model.BasicQos(prefetchSize, prefetchCount, global);
        }

        public void BasicRecover(bool requeue)
        {
            throw new NotImplementedException();
        }

        public void BasicRecoverAsync(bool requeue)
        {
            throw new NotImplementedException();
        }

        public void BasicReject(ulong deliveryTag, bool requeue)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Close(ushort replyCode, string replyText)
        {
            throw new NotImplementedException();
        }

        public void ConfirmSelect()
        {
            throw new NotImplementedException();
        }

        public IBasicPublishBatch CreateBasicPublishBatch()
        {
            throw new NotImplementedException();
        }

        public IBasicProperties CreateBasicProperties()
        {
            throw new NotImplementedException();
        }

        public void ExchangeBind(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void ExchangeBindNoWait(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDeclareNoWait(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDeclarePassive(string exchange)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDelete(string exchange, bool ifUnused)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDeleteNoWait(string exchange, bool ifUnused)
        {
            throw new NotImplementedException();
        }

        public void ExchangeUnbind(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void ExchangeUnbindNoWait(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void QueueBind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void QueueBindNoWait(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            return _model.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
        }

        public void QueueDeclareNoWait(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public QueueDeclareOk QueueDeclarePassive(string queue)
        {
            throw new NotImplementedException();
        }

        public uint MessageCount(string queue)
        {
            throw new NotImplementedException();
        }

        public uint ConsumerCount(string queue)
        {
            throw new NotImplementedException();
        }

        public uint QueueDelete(string queue, bool ifUnused, bool ifEmpty)
        {
            throw new NotImplementedException();
        }

        public void QueueDeleteNoWait(string queue, bool ifUnused, bool ifEmpty)
        {
            throw new NotImplementedException();
        }

        public uint QueuePurge(string queue)
        {
            throw new NotImplementedException();
        }

        public void QueueUnbind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void TxCommit()
        {
            throw new NotImplementedException();
        }

        public void TxRollback()
        {
            throw new NotImplementedException();
        }

        public void TxSelect()
        {
            throw new NotImplementedException();
        }

        public bool WaitForConfirms()
        {
            throw new NotImplementedException();
        }

        public bool WaitForConfirms(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public bool WaitForConfirms(TimeSpan timeout, out bool timedOut)
        {
            throw new NotImplementedException();
        }

        public void WaitForConfirmsOrDie()
        {
            throw new NotImplementedException();
        }

        public void WaitForConfirmsOrDie(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        /*public void BasicRecover(bool requeue)
        {
            _model.
        }

        public void BasicRecoverAsync(bool requeue)
        {
            _model.
        }

        public void BasicReject(ulong deliveryTag, bool requeue)
        {
            _model.
        }

        public void Close()
        {
            _model.
        }

        public void Close(ushort replyCode, string replyText)
        {
            _model.
        }

        public void ConfirmSelect()
        {
            _model.
        }

        public IBasicPublishBatch CreateBasicPublishBatch()
        {
            _model.
        }

        public IBasicProperties CreateBasicProperties()
        {
            _model.
        }

        public void ExchangeBind(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            _model.
        }

        public void ExchangeBindNoWait(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            _model.
        }

        public void ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            _model.
        }

        public void ExchangeDeclareNoWait(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            _model.
        }

        public void ExchangeDeclarePassive(string exchange)
        {
            _model.
        }

        public void ExchangeDelete(string exchange, bool ifUnused)
        {
            _model.
        }

        public void ExchangeDeleteNoWait(string exchange, bool ifUnused)
        {
            _model.
        }

        public void ExchangeUnbind(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            _model.
        }

        public void ExchangeUnbindNoWait(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            _model.
        }

        public void QueueBind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            _model.
        }

        public void QueueBindNoWait(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            _model.
        }

        public QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            _model.
        }

        public void QueueDeclareNoWait(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            _model.
        }

        public QueueDeclareOk QueueDeclarePassive(string queue)
        {
            _model.
        }

        public uint MessageCount(string queue)
        {
            _model.
        }

        public uint ConsumerCount(string queue)
        {
            _model.
        }

        public uint QueueDelete(string queue, bool ifUnused, bool ifEmpty)
        {
            _model.
        }

        public void QueueDeleteNoWait(string queue, bool ifUnused, bool ifEmpty)
        {
            _model.
        }

        public uint QueuePurge(string queue)
        {
            _model.
        }

        public void QueueUnbind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            _model.
        }

        public void TxCommit()
        {
            _model.
        }

        public void TxRollback()
        {
            _model.
        }

        public void TxSelect()
        {
            _model.
        }

        public bool WaitForConfirms()
        {
            _model.
        }

        public bool WaitForConfirms(TimeSpan timeout)
        {
            _model.
        }

        public bool WaitForConfirms(TimeSpan timeout, out bool timedOut)
        {
            _model.
        }

        public void WaitForConfirmsOrDie()
        {
            _model.
        }

        public void WaitForConfirmsOrDie(TimeSpan timeout)
        {
            _model.
        }*/

        public int ChannelNumber { get; }
        public ShutdownEventArgs CloseReason { get; }
        public IBasicConsumer DefaultConsumer { get; set; }
        public bool IsClosed { get; }
        public bool IsOpen { get; }
        public ulong NextPublishSeqNo { get; }
        public string CurrentQueue { get; }
        public TimeSpan ContinuationTimeout { get; set; }
        public event EventHandler<BasicAckEventArgs>? BasicAcks;
        public event EventHandler<BasicNackEventArgs>? BasicNacks;
        public event EventHandler<EventArgs>? BasicRecoverOk;
        public event EventHandler<BasicReturnEventArgs>? BasicReturn;
        public event EventHandler<CallbackExceptionEventArgs>? CallbackException;
        public event EventHandler<FlowControlEventArgs>? FlowControl;
        public event EventHandler<ShutdownEventArgs>? ModelShutdown;
        public void BasicConsume(string queue, bool autoAck, EventingBasicConsumer consumer)
        {
            _model.BasicConsume(queue: queue, consumer: consumer, autoAck: false);
        }

        public string BasicConsume(string queue, bool autoAck, string consumerTag, bool noLocal, bool exclusive, IDictionary<string, object> arguments, IBasicConsumer consumer)
        {
            return _model.BasicConsume(queue, autoAck, consumerTag, noLocal, exclusive, arguments, consumer);
        }

        public string BasicConsume(string queue, bool autoAck, string consumerTag, bool noLocal, bool exclusive, IDictionary<string, object> arguments, EventingBasicConsumer consumer)
        {
            return _model.BasicConsume(queue, autoAck, consumerTag, noLocal, exclusive, arguments, consumer);
        }

        public string BasicConsume(string queue, bool autoAck, IBasicConsumer consumer)
        {
            return _model.BasicConsume(queue, autoAck, consumer);
        }

        public void BasicPublish(string exchange, string routingKey, IBasicProperties basicProperties, byte[] body)
        {
            _model.BasicPublish(exchange, routingKey, basicProperties, body);
        }
    }
}
