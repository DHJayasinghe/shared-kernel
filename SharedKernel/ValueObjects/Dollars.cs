using System;
using System.Collections.Generic;

namespace SharedKernel.ValueObjects;

public sealed class Dollars : ValueObject
{
    public decimal Value { get; }

    public bool IsZero => Value == 0;

    private Dollars(decimal value) => Value = value;

    public static Result<Dollars> Create(decimal dollarAmount)
    {
        if (dollarAmount < 0)
            return Result.Fail<Dollars>("Dollar amount cannot be negative");

        if (dollarAmount % 0.01m > 0)
            return Result.Fail<Dollars>("Dollar amount cannot contain part of a penny");

        return Result.Ok(new Dollars(dollarAmount));
    }

    public static Dollars Zero() => (Dollars)0.00m;

    public static Dollars Of(decimal dollarAmount) => Create(dollarAmount).Value;

    public static Dollars operator *(Dollars dollars, decimal multiplier) => new(dollars.Value * multiplier);

    public static Dollars operator +(Dollars dollars1, Dollars dollars2) => new(dollars1.Value + dollars2.Value);

    public static Dollars operator -(Dollars dollars1, Dollars dollars2) => new(dollars1.Value - dollars2.Value);

    public long ToCents() => (long)Math.Round(this * 100);

    public bool Exceeds(Dollars dollars) => this > dollars;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Dollars(decimal dollars) => Create(dollars).Value;
    public static implicit operator decimal(Dollars dollars) => dollars.Value;
}