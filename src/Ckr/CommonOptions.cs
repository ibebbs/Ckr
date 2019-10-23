using CommandLine;

namespace Ckr
{
    public class CommonOptions
    {
        [Option('b', "broker", Required = true, HelpText = "Address of the broker.")]
        public string Broker { get; set; }

        [Option('p', "port", Required = false, Default = 9092, HelpText = "Port on which to connect to the broker.")]
        public int Port { get; set; }

        [Option('g', "group", Required = false, HelpText = "Group id to use when connecting to the broker.")]
        public string Group { get; set; }

        [Option("User", Required = false, Default = null, HelpText = "The username to use to authenticate with the broker")]
        public string Username { get; set; }

        [Option("Password", Required = false, Default = null, HelpText = "The password to use to authenticate with the broker")]
        public string Password { get; set; }
    }
}
