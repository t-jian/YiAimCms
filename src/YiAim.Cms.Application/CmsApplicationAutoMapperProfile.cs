using AutoMapper;
using YiAim.Cms.Blogs;

namespace YiAim.Cms;

public class CmsApplicationAutoMapperProfile : Profile
{
    public CmsApplicationAutoMapperProfile()
    {
        CreateMap<Blog, BaseBlogDto>();
        CreateMap<Blog, BlogDetailDto>();
        CreateMap<Blog, PageBlogDto>();
    }
}
