using AutoMapper;
using AutoMapper.QueryableExtensions;
using CqrsTest.Categories;
using CqrsTest.Data;
using CqrsTest.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CqrsTest.Products
{
    [ApiController]
    public class GetProducts : Controller
    {
        private readonly IMediator _mediator;
        public GetProducts(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/api/products")]
        public async Task<IActionResult> Get([FromQuery] GetProductsQuery query)
        {
            return await _mediator.Send(query).Process();
        }

        public class GetProductsQuery : IRequest<Result<List<ProductDto>>>
        {

        }

        public class ProductDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public CategoryDto Category { get; set; }
        }

        public class CategoryDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Product, ProductDto>();
                CreateMap<Category, CategoryDto>();
            }
        }

        public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<ProductDto>>>
        {
            private readonly DataContext _db;
            private readonly IMapper _mapper;

            public GetProductsQueryHandler(DataContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Result<List<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
            {
                var result = await _db.Products
                    .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result.Ok(result);
            }
        }
    }
}
