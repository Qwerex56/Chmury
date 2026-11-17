using FluentValidation;

namespace Atlas.Map.Application.Findings.ReportFinding;

internal sealed class ReportFindingValidator : AbstractValidator<ReportFindingCommand>
{
    public ReportFindingValidator()
    {
        RuleFor(c => c.SpeciesId).NotEmpty();
        RuleFor(c => c.Latitude).InclusiveBetween(-90, 90);
        RuleFor(c => c.Longitude).InclusiveBetween(-180, 180);
        RuleFor(c => c.Note).MaximumLength(1000);
    }
}
