using System;
using FluentAssertions;
using SharpTwister.Tests.mt19937;
using Xunit;

namespace SharpTwister.Tests
{
    public class MersenneTwisterTests
    {
        private readonly MersenneTwister _mersenneTwister = new MersenneTwister();
        private readonly Mt19937Ar _originalMersenneTwister = new Mt19937Ar();
        private const int SampleSize = 1_000_000;

        [Theory]
        [InlineData(0)]
        [InlineData(81)]
        [InlineData(543)]
        [InlineData(9008)]
        [InlineData(66578)]
        [InlineData(133429)]
        [InlineData(4366742)]
        [InlineData(77653426)]
        [InlineData(2068543256)]
        [InlineData(uint.MaxValue)]
        public void Next_Should_Produce_Results_Identical_To_Original_C_Version(uint seed)
        {
            SeedAndRun(seed, () => _mersenneTwister.Next().Should().Be(_originalMersenneTwister.Next()));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(35)]
        [InlineData(980)]
        [InlineData(3655)]
        [InlineData(23568)]
        [InlineData(434462)]
        [InlineData(1232356)]
        [InlineData(78745674)]
        [InlineData(656542199)]
        [InlineData(uint.MaxValue)]
        public void Next_Double_Should_Produce_Results_Identical_To_Original_C_Version(uint seed)
        {
            SeedAndRun(seed, () => _mersenneTwister.NextDouble().Should().Be(_originalMersenneTwister.NextDouble()));
        }

        [Theory]
        [InlineData(3, 2, 1)]
        [InlineData(88, 58, 37)]
        [InlineData(323, 121, 117)]
        [InlineData(9780, 5321, 609)]
        [InlineData(64453, 32352, 21384)]
        [InlineData(285566, 937283, 605497)]
        [InlineData(4725681, 7210345, 4665968)]
        [InlineData(79091327, 46898923, 487498)]
        [InlineData(137545789, 230806462, 180310089)]
        [InlineData(uint.MaxValue, uint.MaxValue, 419326370)]
        public void Next_With_Max_Value_Should_Honor_Upper_Bound(uint seed, uint maxValue, uint expectedBoundedNext)
        {
            _mersenneTwister.Seed(seed);
            _mersenneTwister.Next(maxValue).Should().Be(expectedBoundedNext);
        }

        private void SeedAndRun(uint seed, Action action)
        {
            _mersenneTwister.Seed(seed);
            _originalMersenneTwister.Seed(seed);

            for (int i = 0; i < SampleSize; i++)
            {
                action();
            }
        }
    }
}
