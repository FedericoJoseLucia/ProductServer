using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductServer.API.Models;
using ProductServer.Application.Commands.CreateProduct;
using ProductServer.Application.Commands.UpdateProduct;

namespace ProductServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISender mediator;

        public ProductController(ISender mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(nameof(Create))]
        [Consumes(System.Net.Mime.MediaTypeNames.Application.Json)]
        [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RequestResult>> Create([FromBody] CreateProductRequest message, CancellationToken cancellationToken)
        {
            var command = message.Adapt<CreateProductCommand>();

            var result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

            var response = result.Adapt<RequestResult>();

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut(nameof(Update))]
        [Consumes(System.Net.Mime.MediaTypeNames.Application.Json)]
        [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RequestResult>> Update([FromBody] UpdateProductRequest message, CancellationToken cancellationToken)
        {
            var command = message.Adapt<UpdateProductCommand>();

            var result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

            var response = result.Adapt<RequestResult>();

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
