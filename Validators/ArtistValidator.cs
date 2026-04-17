using FluentValidation;
using BeatFlowApi;

namespace BeatFlowApi.Validators;

public class ArtistValidator : AbstractValidator<Artist>
{
    public ArtistValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty().WithMessage("O nome do artista não pode ser vazio.")
            .MaximumLength(150).WithMessage("O nome do artista deve ter no máximo 150 caracteres.");

        RuleFor(a => a.Genre)
            .NotEmpty().WithMessage("O gênero do artista não pode ser vazio.")
            .MaximumLength(100).WithMessage("O gênero deve ter no máximo 100 caracteres.");
    }
}
