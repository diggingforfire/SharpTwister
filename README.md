# What's this?
A C# implementation of the [Mersenne Twister](https://en.wikipedia.org/wiki/Mersenne_Twister) PRNG which was originally written in C by Takuji Nishimura and Makoto Matsumoto. The C# version was made to be used in a similar fashion to the Random class in the .NET framework.

# Does it produce the same numbers as the original?
There are integration tests that compile and pinvoke the original C version and compare the output to the C# version.

# How do I use it?
```C#
// using a time-dependent default seed value
var mersenneTwister = new MersenneTwister();

// .. or using your own seed
mersenneTwister = new MersenneTwister(someRandomSeed);

// you can also (re)seed after creating the object
mersenneTwister.Seed(someRandomSeed);

// 32-bit unsigned integer that is greater than or equal to 0 and less than uint.MaxValue
uint nextInt = mersenneTwister.Next();

// non-negative random integer that is less than the specified maximum
uint boundedNextInt = mersenneTwister.Next(10000);

// floating-point number that is greater than or equal to 0.0, and less than 1.0
double nextDouble = mersenneTwister.NextDouble();

```

# Caveats

* Mersenne Twister is not cryptographically secure. Use ```RNGCryptoServiceProvider``` or derive a class from ```System.Security.Cryptography.RandomNumberGenerator``` instead.
* Avoid instantiating multiple instances when using a time dependent seed to prevent generating identical numeric sequences.
* This implementation is not thread safe. Use a sychronization object if you call any methods from multiple threads.
