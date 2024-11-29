using AutoMapper;
using NoteManager.DtoModels;
using NoteManager.EntityModels;
using NoteManager.Models;

namespace NoteManager.Mapping
{
    /// <summary>
    /// AutoMapper profile for mapping between different models in the NoteManager application.
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperProfile"/> class.
        /// Sets up mappings between various model types.
        /// </summary>
        public MapperProfile() { 
            CreateMap<Note, NoteDto>();
            CreateMap<NoteEM, NoteDto>();
            CreateMap<NoteDto, NoteEM>();
            CreateMap<CategoryDto, CategoryEM>();
            CreateMap<CategoryEM, CategoryDto>();
        }
    }
}

