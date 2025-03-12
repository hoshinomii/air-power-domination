using UnityEngine;

namespace GameNet.KriptoFX.Realistic_Effects_Pack_3.Effects.Common
{
    public sealed class MinAttribute : PropertyAttribute
    {
        public readonly float min;

        public MinAttribute(float min)
        {
            this.min = min;
        }
    }
}
