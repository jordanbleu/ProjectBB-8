namespace Assets.Source.Extensions
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Returns true if this is within '<paramref name="units"/>' of '<paramref name="ofValue"/>' 
        /// (plus or minus)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="units"></param>
        /// <param name="ofValue"></param>
        /// <returns></returns>
        public static bool IsWithin(this float value, float units, float ofValue)
        {
            var min = ofValue - units;
            var max = ofValue + units;
            return (value >= min) && (value <= max);
        }

    }
}
