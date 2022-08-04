using AutoMapper;
using CqrsTest.Data;

namespace CqrsTest.Categories
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Guid, Category>()
                .ConvertUsing<GuidToCategoryConverter>();
        }
        
    }

    public class GuidToCategoryConverter : ITypeConverter<Guid, Category>
    {
        private readonly DataContext _db;
        public GuidToCategoryConverter(DataContext db)
        {
            _db = db;
        }

        public Category Convert(Guid source, Category destination, ResolutionContext context)
        {
            return _db.Categories.FirstOrDefault(c => c.Id == source);
        }
    }
}
