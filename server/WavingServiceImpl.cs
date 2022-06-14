using Deadline;
using Grpc.Core;
using System.Threading.Tasks;
using static Deadline.WavingService;

namespace server
{
    public class WavingServiceImpl : WavingServiceBase
    {
        public override async Task<WavingResponse> wage_with_deadline(WavingRequest request, ServerCallContext context)
        {
            await Task.Delay(300);

            return new WavingResponse { Result = $"Hello {request.Name}" };
        }
    }

}