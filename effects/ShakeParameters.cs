using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.effects
{
    /// <summary>
    /// Parameters for screen shaking.
    /// </summary>
    public class ShakeParameters
    {
        private float _shakeValue = 0; // this value will be what gets adjusted during Screen Shakes

        private float _shakeStrength = 30;
        private float _shakeDecay = 5;

        public float ShakeStrength { get => _shakeStrength; }
        public float ShakeDecay { get => _shakeDecay; }

        /// <summary>
        /// The value we will be adjusting for our screen shake.
        /// </summary>
        public float ShakeValue { get => _shakeValue; set => _shakeValue = value; }

    }
}
