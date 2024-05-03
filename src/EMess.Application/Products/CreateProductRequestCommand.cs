using EMess.Application.Common.Validation;
using EMess.Application.Interfaces.Persistence;
using EMess.Domain.Products;
using FluentValidation;
using MediatR;

namespace EMess.Application.Products
{
    public class CreateProductRequestCommand : IRequest<bool>
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
    }

    public class CreateProductRequestCommandValidator : AbstractValidator<CreateProductRequestCommand>
    {
        public CreateProductRequestCommandValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty();
        }
    }

    public class CreateProductRequestCommandHandler : IRequestHandler<CreateProductRequestCommand, bool>
    {
        private readonly IRepository<Product> _repository;

        public CreateProductRequestCommandHandler(IRepository<Product> repository) => _repository = repository;

        public async Task<bool> Handle(CreateProductRequestCommand request, CancellationToken cancellationToken)
        {
            var product = new Product()
            {
                Title = request.Title,
                Description = !string.IsNullOrEmpty(request.Description) ? request.Description : string.Empty
            };

            await _repository.InsertAsync(product);
            return await _repository.SaveAsync();
        }
    }
}
