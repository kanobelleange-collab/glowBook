using System;
using Application.Features.Etablissements.DTOs;
using Domain.Entities;
using AutoMapper;


namespace Application.Features.Etablissements.Mapping
{
    public class EtablissementMappingProfile:Profile
    {
        public EtablissementMappingProfile()
        {
            CreateMap<Etablissement,EtablissementDto>().ReverseMap();
        }
    }


}