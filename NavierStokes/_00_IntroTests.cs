using Shouldly;
using TorchSharp;

namespace NavierStokes;

[TestFixture]
public class _00IntroTests
{
    public _00IntroTests()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
    }

    [Test]
    public void ShouldCreateLinearSpace()
    {
        var dtype = torch.ScalarType.Float32;
        var result = torch.linspace(0, 5, 10, dtype: dtype);
        var data = result.data<float>().ToArray();
        data.ShouldBe(new float[]
        {
            0f,
            0.5555556f,
            1.1111112f,
            1.6666667f,
            2.2222223f,
            2.7777777f,
            3.3333333f,
            3.8888888f,
            4.4444447f,
            5f
        }, 1e-7f);
    }

    [Test]
    public void ShouldCreateSlice()
    {
        // arrange
        var dtype = torch.ScalarType.Float32;
        var linspace = torch.linspace(0, 5, 10, dtype: dtype);

        // act
        var result = linspace[..3].data<float>().ToArray();

        // assert
        result.ShouldBe(new float[]
        {
            0f,
            0.5555556f,
            1.1111112f
        }, 1e-7f);
    }

    [Test]
    public void ShouldSetVariable()
    {
        // arrange
        var dtype = torch.ScalarType.Float32;
        var linspace = torch.linspace(0, 5, 10, dtype: dtype);

        // act
        linspace[3] = 42;

        // act
        var data = linspace.data<float>().ToArray();
        data.ShouldBe(new float[]
        {
            0f,
            0.5555556f,
            1.1111112f,
            42f, // updated value
            2.2222223f,
            2.7777777f,
            3.3333333f,
            3.8888888f,
            4.4444447f,
            5f
        }, 1e-7f);
    }
}