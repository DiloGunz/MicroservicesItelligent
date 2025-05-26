using ArticlesService.Application.Models.Commands.ArticleCommands;
using ArticlesService.Application.Models.Dtos.ArticlesDtos;
using ArticlesService.Domain;
using ArticlesService.SharedKernel.Collection;
using AutoMapper;

namespace ArticlesService.Application.Shared.Articles;

public class ArticleMap : Profile
{
	public ArticleMap()
	{
		CreateMap<CreateArticleCommand, Article>()
			.ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title.ToUpper().Trim()))
			.ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Content.ToUpper().Trim()))
			.ForMember(dst => dst.Summary, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Summary) ? null : src.Summary.ToUpper().Trim()));

        CreateMap<EditArticleCommand, Article>()
            .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title.ToUpper().Trim()))
            .ForMember(dst => dst.Content, opt => opt.MapFrom(src => src.Content.ToUpper().Trim()))
            .ForMember(dst => dst.Summary, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Summary) ? null : src.Summary.ToUpper().Trim()));

        CreateMap<Article, ArticleListDto>();
		CreateMap<Article, ArticleDetailsDto>();

		CreateMap<DataCollection<Article>, DataCollection<ArticleListDto>>();
        CreateMap<DataCollection<Article>, DataCollection<ArticleDetailsDto>>();
    }
}