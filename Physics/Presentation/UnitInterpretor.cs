﻿using System.Text;
using System.Text.RegularExpressions;

namespace Physics.Presentation;

internal class UnitInterpretor
{
    private readonly List<KnownUnit> _baseUnits;
    private readonly IUnitDialect _dialect;
    private readonly Dictionary<string, Unit> _parseCache = new();
    private readonly Dictionary<string, KnownUnit> _prefixedUnits = new();
    private readonly IUnitSystem _system;
    private readonly string _unitRegex;

    public UnitInterpretor(IUnitSystem system, IUnitDialect dialect)
    {
        _system = system;
        _dialect = dialect;
        _unitRegex = BuildUnitRegex();
        _baseUnits = [.._system.BaseUnits];

        foreach (var unit in system)
        {
            if (unit.Symbol != unit.BaseSymbol)
            {
                _prefixedUnits.Add(unit.BaseSymbol, unit);
            }
        }
    }

    public string ToString(Unit unit)
    {
        var nominator = new List<string>();
        var denominator = new List<string>();

        for (var i = 0; i < unit.Dimension.Count(); i++)
        {
            var exponent = unit.Dimension[i];

            if (exponent > 0)
            {
                nominator.Add(BuildFactor(_baseUnits[i].Symbol, exponent));
            }

            if (exponent < 0)
            {
                denominator.Add(BuildFactor(_baseUnits[i].Symbol, exponent));
            }
        }

        if (!nominator.Any() && !denominator.Any())
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        if (!unit.IsCoherent)
        {
            builder.Append(unit.Factor).Append(" ");
        }

        builder.Append(!nominator.Any() ? "1" : string.Join(_dialect.Multiplication.First(), nominator));

        if (denominator.Any())
        {
            builder.Append(" {0} ".FormatWith(_dialect.Division.First()));
            builder.Append(string.Join(_dialect.Multiplication.First(), denominator));
        }

        return builder.ToString();
    }

    private string BuildFactor(string symbol, int exponent)
    {
        var absolute = Math.Abs(exponent);

        if (absolute == 1)
        {
            return symbol;
        }

        return "{0}{1}{2}".FormatWith(symbol, _dialect.Exponentiation.First(), absolute);
    }

    public Unit Parse(string unitExpression)
    {
        if (_parseCache.TryGetValue(unitExpression, out var unit))
        {
            return unit;
        }

        lock (_parseCache)
        {
            if (_parseCache.TryGetValue(unitExpression, out unit))
            {
                return unit;
            }

            unit = ParseCore(unitExpression);
            _parseCache.Add(unitExpression, unit);
        }

        return unit;
    }

    private Unit ParseCore(string unitExpression)
    {
        var fraction = unitExpression.Split(
            (string[])_dialect.Division,
            StringSplitOptions.RemoveEmptyEntries);

        if (fraction.Length == 0)
        {
            return _system.NoUnit;
        }

        if (fraction.Length == 1)
        {
            return ParseUnitProduct(fraction[0]);
        }

        if (fraction.Length > 2)
        {
            throw new FormatException(Messages.UnitExpressionInvalid.FormatWith(unitExpression));
        }

        var nominator = ParseUnitProduct(fraction[0]);
        var denominator = ParseUnitProduct(fraction[1]);

        return nominator/denominator;
    }

    private Unit ParseUnitProduct(string unitExpression)
    {
        var factors = unitExpression.Split(
            (string[])_dialect.Multiplication,
            StringSplitOptions.RemoveEmptyEntries);
        var product = _system.NoUnit;

        return factors.Aggregate(product, (current, factor) => current*ParseUnit(factor));
    }

    private Unit ParseUnit(string unitExpression)
    {
        var matches = Regex.Matches(unitExpression, _unitRegex);

        if (matches.Count == 0)
        {
            throw new FormatException(Messages.UnitExpressionInvalid.FormatWith(unitExpression));
        }

        if (matches.Count > 1)
        {
            throw new FormatException(Messages.UnitExpressionAmbiguous.FormatWith(unitExpression));
        }

        var prefixSymbol = matches[0].Groups["prefix"].Value;
        var unitSymbol = matches[0].Groups["unit"].Value;
        var exponent = matches[0].Groups["exponent"].Value;

        var knownUnit = _system[unitSymbol];
        Unit parsedUnit;

        if (knownUnit == null)
        {
            knownUnit = _prefixedUnits[unitSymbol];
            parsedUnit = knownUnit/knownUnit.InherentFactor;
        }
        else
        {
            parsedUnit = knownUnit;
        }


        if (!string.IsNullOrEmpty(prefixSymbol))
        {
            parsedUnit *= UnitPrefix.Prefixes[prefixSymbol];
        }

        if (!string.IsNullOrEmpty(exponent))
        {
            parsedUnit ^= int.Parse(exponent);
        }

        return parsedUnit;
    }

    private string BuildUnitRegex()
    {
        var prefixes = string.Join("|", UnitPrefix.Prefixes.Keys);
        var units = string.Join("|", _system.Select(u => Regex.Escape(u.BaseSymbol)));
        var exponentiationOperators = string.Join("|", _dialect.Exponentiation.Select(Regex.Escape));

        var regex = @"^(?<prefix>({0}))?(?<unit>{1})({2}(?<exponent>-?\d+))?$".FormatWith(prefixes, units,
            exponentiationOperators);

        return regex;
    }
}