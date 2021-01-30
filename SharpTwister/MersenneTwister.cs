using System;

namespace SharpTwister
{
    /* 
       Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
       All rights reserved.                          

       Redistribution and use in source and binary forms, with or without
       modification, are permitted provided that the following conditions
       are met: 

         1. Redistributions of source code must retain the above copyright
            notice, this list of conditions and the following disclaimer.

         2. Redistributions in binary form must reproduce the above copyright
            notice, this list of conditions and the following disclaimer in the
            documentation and/or other materials provided with the distribution.

         3. The names of its contributors may not be used to endorse or promote 
            products derived from this software without specific prior written 
            permission.

       THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
       "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
       LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
       A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
       CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
       EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
       PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
       PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
       LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
       NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
       SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

       Any feedback is very welcome.
       http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html
       email: m-mat @ math.sci.hiroshima-u.ac.jp (remove space)
    */
    
    /// <summary>
    /// Pseudo-number generator based on the Mersenne Twister algorithm.
    /// </summary>
    public class MersenneTwister
    {
        // Period parameters
        private const int  N                      = 624; 
        private const int  M                      = 397;
        private const uint MatrixA                = 0x9908b0df;
        private const uint UpperMask              = 0x80000000;
        private const int  LowerMask              = 0x7fffffff;
        private const uint SpectralTestMultiplier = 1812433253; // see Knuth TAOCP Vol2. 3rd Ed. P.106

        // Tempering bit masks
        private const uint B = 0x9d2c5680;
        private const uint C = 0xefc60000;

        // Private static variables
        private static readonly uint[] Mag01 = { 0, MatrixA };

        // Member variables
        private readonly uint[] _state = new uint[N];
        private int _index = N + 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpTwister.MersenneTwister"></see> class, using a time-dependent default seed value.
        /// </summary>
        public MersenneTwister() : this((uint)Environment.TickCount)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpTwister.MersenneTwister"></see> class, using the specified seed value.
        /// </summary>
        /// <param name="seed">The number used to calculate a starting value for the pseudo-random number sequence.</param>
        public MersenneTwister(uint seed)
        {
            Seed(seed);
        }

        /// <summary>
        /// Sets the starting value for the pseudo-random number sequence.
        /// </summary>
        /// <param name="seed">The number used to calculate a starting value for the pseudo-random number sequence.</param>
        public void Seed(uint seed)
        {
            _state[0] = seed;

            for (_index = 1; _index < N; _index++)
            {
                _state[_index] = (uint)(SpectralTestMultiplier * (_state[_index - 1] ^ (_state[_index -1] >> 30)) + _index);
            }
        }

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>A 32-bit unsigned integer that is greater than or equal to 0 and less than <see cref="F:System.UInt32.MaxValue"></see>.</returns>
        public uint Next()
        {
            if (_index >= N)
            {
                Twist();
            }

            uint next = _state[_index++];

            return Temper(next);
        }

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns></returns>
        public uint Next(uint maxValue)
        {
            return (uint)(Next() * (maxValue / 4294967296.0));
        }

        /// <summary>Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.</summary>
        /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
        public double NextDouble()
        {
            return Next() * (1.0 / uint.MaxValue);
        }

        private void Twist()
        {
            uint twister;
            int twistIndex;

            for (twistIndex = 0; twistIndex < N - 1; twistIndex++)
            {
                twister = (_state[twistIndex] & UpperMask) | (_state[twistIndex + 1] & LowerMask);
                _state[twistIndex] = _state[(twistIndex + M) % N] ^ (twister >> 1) ^ Mag01[twister & 1];
            }

            twister = (_state[N - 1] & UpperMask) | (_state[0] & LowerMask);
            _state[N - 1] = _state[M - 1] ^ (twister >> 1) ^ Mag01[twister & 1];

            _index = 0;
        }

        private static uint Temper(uint next)
        {
            next ^= (next >> 11);
            next ^= (next << 7) & B;
            next ^= (next << 15) & C;
            next ^= (next >> 18);

            return next;
        }
    }
}
