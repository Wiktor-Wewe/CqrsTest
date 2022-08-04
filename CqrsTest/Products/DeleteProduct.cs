using CqrsTest.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CqrsTest.Products
{
    [ApiController]
    public class DeleteProduct : Controller
    {
        private readonly IMediator _mediator;

        public DeleteProduct(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("api/products/{id}")]
        public async Task<IActionResult> Get(Guid id) => await _mediator.Send(new DeleteProductCommand(id)).Process();

        public class DeleteProductCommand : IRequest<Result>
        {
            public DeleteProductCommand(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; set; }
        }

        public class GetProductQueryHandler : IRequestHandler<DeleteProductCommand, Result>
        {
            private readonly IProductRepository _repository;

            public GetProductQueryHandler(IProductRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.Delete(request.Id, cancellationToken);

                if (result)
                {
                    return Result.Ok();
                }

                return Result.NotFound(request.Id);
            }
        }
    }
}
