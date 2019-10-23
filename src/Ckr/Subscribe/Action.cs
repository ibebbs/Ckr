using Confluent.Kafka;
using Confluent.Kafka.Reactive;
using Confluent.Kafka.Reactive.Consumer;
using FluentColorConsole;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Event = Confluent.Kafka.Reactive.Consumer.Event;

namespace Ckr.Subscribe
{
    public static class Action
    {
        private static (TextFirstConsole, string) EventText(IEvent @event)
        {
            switch (@event)
            {
                case Event.LogMessageReceived logMessageReceived: return (ColorConsole.WithCyanText, $"Log: '{logMessageReceived.LogMessage.Message}'");
                case Event.EndOfPartition endOfPartition: return (ColorConsole.WithGrayText, $"End of partition at '{endOfPartition.Topic}/{endOfPartition.Partition.Value}@{endOfPartition.Offset.Value}'");
                case Event.MessageReceived<string, string> message: return (ColorConsole.WithWhiteText, $"Message from '{message.ConsumeResult.Topic}/{message.ConsumeResult.Partition.Value}@{message.ConsumeResult.Offset.Value}': '{message.ConsumeResult.Message.Value}'");
                case Event.PartitionsAssigned paritionsAssigned: return (ColorConsole.WithMagentaText, "Assigned: " + string.Join(", ", paritionsAssigned.Partitions.Select(partition => $"'{partition.Topic}/{partition.Partition.Value}'")));
                case Event.PartitionsRevoked paritionsRevoked: return (ColorConsole.WithDarkMagentaText, "Revoked: " + string.Join(", ", paritionsRevoked.Partitions.Select(partition => $"'{partition.Topic}/{partition.Partition.Value}'")));
                case Event.StatisticsReceived statistics: return (ColorConsole.WithYellowText, $"Statistics '{statistics.Statistics}'");
                case Event.OffsetsCommitted offsetsCommitted: return (ColorConsole.WithGreenText,  "Committed:" + string.Join(", ", offsetsCommitted.CommittedOffsets.Offsets.Select(offset => $"'{offset.Topic}/{offset.Partition.Value}@{offset.Offset.Value}'")));
                default: return (ColorConsole.WithRedText, $"Unknown Event of type: '{@event.GetType().Name}'");
            }
        }

        private static ConsumerBuilder<string, string> AddDeserializers(ConsumerBuilder<string, string> consumerBuilder)
        {
            return consumerBuilder
                .SetKeyDeserializer(Serialization.StringDeserializer.Instance)
                .SetValueDeserializer(Serialization.StringDeserializer.Instance);
        }

        public static async Task<int> Runner(Options options)
        {
            ConsumerConfig config = new ConsumerConfig
            {
                GroupId = options.Group,
                BootstrapServers = $"{options.Broker}:{options.Port}",
                EnableAutoCommit = options.AutoCommit,
                CancellationDelayMaxMs = 100,
                AutoOffsetReset = options.OffsetReset,
                EnablePartitionEof = true
            };

            var consumer = config.ToReactiveConsumer<string, string>(AddDeserializers);

            using (var subscription = consumer.Select(EventText).Subscribe(tuple => tuple.Item1.WriteLine(tuple.Item2)))
            {
                using (var connection = consumer.Connect())
                {
                    consumer.Subscribe(options.Topic);

                    Console.WriteLine($"Subscribed to topic {options.Topic} from broker at {options.Broker}:{options.Port}");
                    Console.WriteLine("Hit <Enter> to quit.");

                    await Task.Run(() => Console.ReadLine());
                }
            }

            return 0;
        }
    }
}
