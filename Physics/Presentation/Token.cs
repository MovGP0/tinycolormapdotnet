namespace Physics.Presentation;

public class Token : ImmutableCollection<string>
{
    public Token(params string[] representations)
        : base(representations)
    {
    }

    public static explicit operator string[](Token token) => token.ToArray();
}