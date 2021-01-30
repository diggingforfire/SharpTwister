using System.Runtime.InteropServices;

namespace SharpTwister.Tests.mt19937
{
    /// <summary>
    /// Wrapper class for the original C implementation of the Mersenne Twister.
    /// </summary>
    internal class Mt19937Ar
    {
        private const string LibraryPath = "mt19937ar.so";

        [DllImport(LibraryPath, EntryPoint = "init_genrand")]
        private static extern void init_genrand(uint seed);

        [DllImport(LibraryPath, EntryPoint = "genrand_int32")]
        private static extern uint genrand_int32();

        [DllImport(LibraryPath, EntryPoint = "genrand_real1")]
        private static extern double genrand_real1();

        public void Seed(uint seed)
        {
            init_genrand(seed);
        }

        public uint Next()
        {
            return genrand_int32();
        }

        public double NextDouble()
        {
            return genrand_real1();
        }
    }
}

