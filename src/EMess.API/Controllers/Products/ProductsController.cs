using Emess.Shared.Authorization;
using EMess.Application.Products;
using EMess.Infrastructure.Permissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Action = Emess.Shared.Authorization.Action;

namespace EMess.API.Controllers.Products
{
    public class ProductsController : BaseController
    {
        protected readonly IMediator _mediator;

        public ProductsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [HasPermission(Action.Create, Resource.Products)]
        public async Task<IActionResult> RegisterRoleAsync(CreateProductRequestCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
