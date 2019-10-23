using CommandLine;
using System.Threading.Tasks;

namespace Ckr
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<Publish.Options, Subscribe.Options>(args)
                .MapResult(
                    (Publish.Options options) => Publish.Action.Runner(options),
                    (Subscribe.Options options) => Subscribe.Action.Runner(options),
                    errors => Task.FromResult(1)
                ); ;
        }
    }
}
