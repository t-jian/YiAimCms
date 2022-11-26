﻿using AutoMapper;
using YiAim.Cms.Blogs;

namespace YiAim.Cms;

public class CmsApplicationAutoMapperProfile : Profile
{
    public CmsApplicationAutoMapperProfile()
    {
        CreateMap<Blog, BaseBlogDto>();
        CreateMap<Blog, BlogDetailDto>();
        CreateMap<Blog, PageBlogDto>();
        CreateMap<CreateBlogInput, Blog>().ReverseMap();


        CreateMap<Category, BaseCategoryDto>();
        CreateMap<Category, CategoryDto>();

        CreateMap<CreateCategoryInput, Category>().ReverseMap();
        CreateMap<EditCategoryInput, Category>();



        CreateMap<CreateAnthologyInput, Anthology>().ReverseMap();
        CreateMap<UpdateAnthologyInput, Anthology>().ReverseMap();

        CreateMap<Anthology, PageAnthologyDto>();
        CreateMap<Anthology, AnthologyDto>();
        CreateMap<Anthology, PageAnthologyClientDto>();
    }
}
