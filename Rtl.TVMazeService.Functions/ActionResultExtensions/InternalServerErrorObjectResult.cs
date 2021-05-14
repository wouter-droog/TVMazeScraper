using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rtl.TVMazeService.Functions.ActionResultExtensions
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value) => StatusCode = StatusCodes.Status500InternalServerError;
    }
}
