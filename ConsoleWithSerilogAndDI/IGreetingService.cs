using System.Threading.Tasks;

namespace ConsoleWithSerilogAndDI
{
    public interface IGreetingService
    {
        Task RunAsync();
    }
}