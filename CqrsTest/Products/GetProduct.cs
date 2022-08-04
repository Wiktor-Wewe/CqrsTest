using AutoMapper;
using AutoMapper.QueryableExtensions;
using CqrsTest.Data;
using CqrsTest.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CqrsTest.Products.GetProducts;

namespace CqrsTest.Products
{
    [ApiController]
    public class GetProduct : Controller
    {
        private readonly IMediator _mediator;

        public GetProduct(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/api/products/{id}")]
        public async Task<IActionResult> Get(Guid id) => await _mediator.Send(new GetProductQuery(id)).Process();

        public class GetProductQuery : IRequest<Result<ProductDto>>
        {
            public GetProductQuery(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; set; }
        }

        public class ProductDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public Guid CategoryId { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Product, ProductDto>();
        }

        public class GetProductsQueryHandler : IRequestHandler<GetProductQuery, Result<ProductDto>>
        {
            private readonly DataContext _db;
            private readonly IMapper _mapper;


            public GetProductsQueryHandler(DataContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Result<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
            {
                var result = await _db.Products
                    .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                if(result == null)
                {
                    return Result.NotFound<ProductDto>(request.Id);
                }

                return Result.Ok(result);
            }
        }


    }
}
