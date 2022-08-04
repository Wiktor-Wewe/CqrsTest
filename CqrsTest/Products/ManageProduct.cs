using AutoMapper;
using CqrsTest.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json.Serialization;

namespace CqrsTest.Products
{
    [ApiController]
    public class ManageProduct : Controller
    {
        private readonly IMediator _mediator;
        public ManageProduct(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/products")]
        public async Task<IActionResult> Post(ManageProductCommand command)
        {
            return await _mediator.Send(command).Process();
        }

        [HttpPut("api/products/{id}")]
        public async Task<IActionResult> Post(Guid id, ManageProductCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command).Process();
        }

        public class ManageProductCommand : IRequest<Result<Guid>>
        {
            [JsonIgnore]
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public Guid CategoryId { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ManageProductCommand, Product>()
                    .ForMember(dest => dest.Category, opt => opt.MapFrom
                    (src => src.CategoryId));
            }
        }

        public class Validator : AbstractValidator<Product>
        {
            public Validator()
            {
                RuleFor(p => p.Name)
                    .NotEmpty();
                RuleFor(p => p.Category)
                    .NotEmpty();
            }
        }

        public class ManageProductCommandHandler : IRequestHandler<ManageProductCommand, Result<Guid>>
        {
            private readonly IProductRepository _repository;
            private readonly IMapper _mapper;
            private readonly IValidator<Product> _validator;

            public ManageProductCommandHandler(IProductRepository repository, IMapper mapper, IValidator<Product> validator)
            {
                _repository = repository;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<Result<Guid>> Handle(ManageProductCommand request, CancellationToken cancellationToken)
            {
                var isAdding = request.Id == Guid.Empty;
                Product product;
                if (isAdding)
                {
                    product = new Product();
                }
                else
                {
                    product = await _repository.GetById(request.Id, cancellationToken);

                    if(product == null)
                    {
                        return Result.NotFound<Guid>(request.Id);
                    }
                }

                product = _mapper.Map(request, product);

                var validationResult = await _validator.ValidateAsync(product, cancellationToken);

                if(validationResult.IsValid == false)
                {
                    return validationResult.ToResult<Guid>();
                }

                if (isAdding)
                {
                    await _repository.Create(product, cancellationToken);
                }
                else
                {
                    await _repository.Update(product, cancellationToken);
                }

                return Result.Ok(product.Id);
            }
        }
    }
}
