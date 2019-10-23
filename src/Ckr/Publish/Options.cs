using CommandLine;

namespace Ckr.Publish
{
    [Verb("publish", HelpText = "Publish a message to a broker on a specific topic")]
    public class Options : CommonOptions
    {

        [Option('t', "topic", Required = true, HelpText = "Topic on which to publish a message.")]
        public string Topic { get; set; }

        [Option('m', "message", HelpText = "The message to publish to the broker.", SetName = "Source")]
        public string Message { get; set; }
    }
}
