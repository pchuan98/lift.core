namespace Lift.Core.Constant;

/// <summary>
/// 
/// </summary>
public static class ValueBox
{
    /// <summary>
    /// 
    /// </summary>
    public static class DoubleBox
    {
        /// <summary>
        /// 
        /// </summary>
        public const double Threshold = 1E-40;

        /// <summary>
        /// 
        /// </summary>
        public static double Zero = 0;

        public static bool Equal(double a, double b)
            => Math.Abs(a - b) < Threshold;
    }

}
