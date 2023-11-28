namespace Lift.Core.Constant;

/// <summary>
/// 常量
/// </summary>
public static class ValueBox
{
    /// <summary>
    /// 
    /// </summary>
    public static class DoubleBox
    {
        /// <summary>
        /// 临界点，用于浮点类型的比较
        /// </summary>
        public const double Threshold = 1E-40;

        /// <summary>
        /// 零
        /// </summary>
        public static double Zero = 0;

        public static bool Equal(double a, double b)
            => Math.Abs(a - b) < Threshold;
    }

}
