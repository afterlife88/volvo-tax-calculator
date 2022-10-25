namespace Volvo.TaxCalculator.Domain;

public record TaxRules(IReadOnlyCollection<HourRule> HourRules);

public record HourRule(TimeOnly From, TimeOnly To, int Tax);