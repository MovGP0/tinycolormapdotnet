namespace Physics.Presentation;

internal class UnitDialect : IUnitDialect
{
    public Token Division => new("/", "\u00f7");
    public Token Multiplication => new(" ", "\u00B7", "\u00D7");
    public Token Exponentiation => new("^");
}