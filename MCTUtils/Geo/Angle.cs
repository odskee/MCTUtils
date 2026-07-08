namespace MCTUtils.Geo;

/// <summary>
/// Angle conversion utilities.
/// </summary>
public static class Angle
{
    /// <summary>
    /// Converts radians to degrees.
    /// </summary>
    public static double ConvertRadiansToDegrees(double radians)
    {
        return radians * 180 / Math.PI;
    }

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    public static double ConvertDegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }
}
