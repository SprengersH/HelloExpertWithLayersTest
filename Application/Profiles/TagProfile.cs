using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using Core.Entities;

namespace Application.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>();
            CreateMap<TagForCreationDto, Tag>();
            CreateMap<TagForUpdateDto, Tag>();
            CreateMap<Tag, TagForUpdateDto>();
        }
    }
}
