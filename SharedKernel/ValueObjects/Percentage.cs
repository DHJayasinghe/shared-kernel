using System.Collections.Generic;

namespace SharedKernel.ValueObjects;

public sealed class Percentage : ValueObject
{
    public decimal Value { get; }

    public bool IsZero => Value == 0;

    private Percentage(decimal value) => Value = value;

    public static Result<Percentage> Create(decimal percentage)
    {
        if (percentage < 0)
            return Result.Fail<Percentage>("Percentage cannot be negative");

        if (percentage % 0.01m > 0)
            return Result.Fail<Percentage>("Percentage cannot exceeds 2 decimal places");

        if (percentage > 100)
            return Result.Fail<Percentage>("Percentage cannot exceeds 100 percent");

        return Result.Ok(new Percentage(percentage));
    }

    public static Percentage Zero() => (Percentage)0m;
    public static Percentage Hundred() => (Percentage)100m;

    public static Percentage Of(decimal percentage) => Create(percentage).Value;

    public Dollars From(Dollars dollarAmount) => dollarAmount * (Value / 100);

    public static Percentage operator *(Percentage percentage, decimal multiplier) => new(percentage.Value * multiplier);

    public static Percentage operator +(Percentage percentage1, Percentage percentage2) => new(percentage1.Value + percentage2.Value);

    public static Percentage operator -(Percentage percentage1, Percentage percentage2) => new(percentage1.Value - percentage2.Value);

    public bool Exceeds(Percentage percentage) => this > percentage;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Percentage(decimal percentage) => Create(percentage).Value;
    public static implicit operator decimal(Percentage percentage) => percentage.Value;
}