namespace Husa.Quicklister.Abor.Api.Mappings
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Response;

    public class ValidationErrorMappingProfile : Profile
    {
        public ValidationErrorMappingProfile()
        {
            this.CreateMap<ValidationResult, ValidationErrorResponse>()
                .ForMember(validationResponse => validationResponse.FieldName, validationResult => validationResult.MapFrom(x => x.MemberNames.First()));
        }
    }
}
