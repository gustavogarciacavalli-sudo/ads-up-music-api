using FluentValidation;
using BeatFlowApi;

namespace BeatFlowApi.Validators;

public class TrackValidator : AbstractValidator<Track>
{
    public TrackValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage("O título da música não pode ser vazio.")
            .MaximumLength(200).WithMessage("O título deve ter no máximo 200 caracteres.");

        RuleFor(t => t.Bpm)
            .GreaterThan(0).WithMessage("O BPM deve ser maior que 0.");

        RuleFor(t => t.Genre)
            .NotEmpty().WithMessage("O gênero da música não pode ser vazio.")
            .MaximumLength(100).WithMessage("O gênero deve ter no máximo 100 caracteres.");

        RuleFor(t => t.ArtistId)
            .GreaterThan(0).WithMessage("O ArtistId deve ser um valor válido (> 0).");
    }
}
