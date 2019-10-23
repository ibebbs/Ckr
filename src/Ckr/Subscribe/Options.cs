using CommandLine;
using Confluent.Kafka;

namespace Ckr.Subscribe
{
    [Verb("subscribe", HelpText = "Subscribe to messages from a broker on a specific topic")]
    public class Options : CommonOptions
    {
        [Option('t', "topic", Required = true, HelpText = "Topic on which to publish a message.")]
        public string Topic { get; set; }

        [Option('o', "offset", Required = false, HelpText = "The `auto.offset.reset` value to pass to the consumer", Default = AutoOffsetReset.Latest, MetaValue = "Earliest|Latest")]
        public AutoOffsetReset OffsetReset { get; set; } = AutoOffsetReset.Latest;

        [Option('a', "auto", Required = false, HelpText = "The `enable.auto.commit` value to pass to the consumer", Default = false, MetaValue = "true|false")]
        public bool AutoCommit { get; set; } = false;
    }
}
