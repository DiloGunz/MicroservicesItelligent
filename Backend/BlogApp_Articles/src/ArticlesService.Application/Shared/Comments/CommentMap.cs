using ArticlesService.Application.Models.Commands.CommentCommands;
using ArticlesService.Application.Models.Dtos.CommentDtos;
using ArticlesService.Domain;
using AutoMapper;

namespace ArticlesService.Application.Shared.Comments;

public class CommentMap : Profile
{
	public CommentMap()
	{
		CreateMap<CreateCommentCommand, Comment>()
			.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content.ToUpper().Trim()));

		CreateMap<Comment, CommentDto>()
			.ForMember(dest => dest.Article, opt => opt.MapFrom(src => src.Article.Title));
	}
}