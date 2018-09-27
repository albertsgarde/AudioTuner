using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Dsp;

namespace AudioTuner
{
    public static class ComplexExtensions
    {
        public static float LengthSquared(this Complex z)
        {
            return z.X * z.X + z.Y * z.Y;
        }

        public static float Length(this Complex z)
        {
            return (float)Math.Sqrt(z.LengthSquared());
        }
    }
}
