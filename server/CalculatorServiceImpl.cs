using Calculator;
using Grpc.Core;
using System.Threading.Tasks;
using static Calculator.CalculatorService;

namespace server
{
    public class CalculatorServiceImpl : CalculatorServiceBase
    {
        public override Task<CalculatorResponse> Sum(CalculatorRequest request, ServerCallContext context)
        {
            var sum = request.Values.X + request.Values.Y;
            return Task.FromResult(new CalculatorResponse { Result = sum });
        }
    }
}
