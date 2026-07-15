using MCTUtils.Internal.Geo;
using System.Numerics;

namespace MCTUtils.Geo;

public enum FuelWeight
{
    /// <summary>
    /// Jet A-1 fuel type.
    /// </summary>
    JetFuel = 0,


    /// <summary>
    /// Diesel fuel type.
    /// </summary>
    Diesel = 1,


    /// <summary>
    /// Gasoline fuel type.
    /// </summary>
    Gasoline = 2,


    /// <summary>
    /// Methanol fuel type.
    /// </summary>
    Methanol = 3
}



/// <summary>
/// Angle conversion utilities.
/// </summary>
public static class AnglesAndMeasurements
{
    /// <summary>
    /// Converts radians to degrees.
    /// </summary>
    /// <param name="radians"></param>
    /// <returns>conversion to degrees</returns>
    public static double ConvertRadiansToDegrees(double radians)
    {
        return radians * 180 / Math.PI;
    }


    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    /// <param name="degrees"></param>
    /// <returns>conversion to radians</returns>
    public static double ConvertDegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }


    /// <summary>
    /// Converts meters to feet.
    /// </summary>
    /// <param name="meters"></param>
    /// <returns>feet</returns>
    public static int ConvertToFeet(double meters)
    {
        if (meters == 0)
        {
            return 0;
        }
        double _con = meters * 3.280839895;
        return Convert.ToInt32(Math.Round(_con));
    }

    /// <summary>
    /// Converts feet to meters.
    /// </summary>
    /// <param name="feet"></param>
    /// <returns>meters</returns>
    public static double ConvertToMeter(int feet)
    {
        if (feet == 0)
            return 0;

        return Math.Round(feet * 0.3048, 2);
    }


    /// <summary>
    /// Converts feet to meters.
    /// </summary>
    /// <param name="feet"></param>
    /// <returns>meters</returns>
    public static double ConvertToMeter(double feet)
    {
        if (feet == 0)
            return 0;

        return Math.Round(feet * 0.3048, 2);
    }


    /// <summary>
    /// Converts tons to litres based on the fuel type.
    /// </summary>
    /// <param name="tons"></param>
    /// <param name="fuelType"></param>
    /// <returns>litres</returns>
    public static double TonsToLitres(int tons, FuelWeight fuelType)
    {
        return fuelType switch
        {
            FuelWeight.JetFuel => tons * 1000 / 0.840,
            FuelWeight.Diesel => tons * 1000 / 0.845,
            FuelWeight.Gasoline => tons * 1000 / 0.775,
            FuelWeight.Methanol => tons * 1000 / 0.792,
            _ => 0,
        };
    }

    /// <summary>
    /// Converts nautical miles to meters.
    /// </summary>
    /// <param name="nauticalMile"></param>
    /// <returns>meters</returns>
    public static double ConvertNmToMeter(double nauticalMile)
    {
        return Math.Round(nauticalMile * 1852);
    }


    /// <summary>
    /// Converts meters to kilometers.
    /// </summary>
    /// <param name="meter"></param>
    /// <returns>kilometers</returns>
    public static double ConvertMeterToKm(double meter)
    {
        return Math.Round(meter / 1000, 3);
    }


    /// <summary>
    /// Converts meters to nautical miles.
    /// </summary>
    /// <param name="meter"></param>
    /// <returns>nautical miles</returns>
    public static double ConvertMeterToNm(double meter)
    {
        return Math.Round(meter / 1852);
    }


    /// <summary>
    /// Calculates the barycentric coordinates of a point p with respect to a triangle defined by points a, b, and c.
    /// </summary>
    /// <param name="p"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns>The barycentric coordinates of the point.</returns>
    public static BarryPoint GetBarycentric(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        Vector2 v0 = Vector2.Subtract(b, a);
        Vector2 v1 = Vector2.Subtract(c, a);
        Vector2 v2 = Vector2.Subtract(p, a);
        float d00 = Vector2.Dot(v0, v0);
        float d01 = Vector2.Dot(v0, v1);
        float d11 = Vector2.Dot(v1, v1);
        float d20 = Vector2.Dot(v2, v0);
        float d21 = Vector2.Dot(v2, v1);
        float denom = d00 * d11 - d01 * d01;

        float v = (d11 * d20 - d01 * d21) / denom;
        float w = (d00 * d21 - d01 * d20) / denom;
        float u = 1.0f - v - w;

        BarryPoint ret = new() { wB = w, wC = v, wA = u };
        return ret;
    }


    /// <summary>
    /// Calculates the simple distance between two geographical points specified by their latitude and longitude using the Haversine formula.
    /// </summary>
    /// <param name="latStart"></param>
    /// <param name="latEnd"></param>
    /// <param name="lonStart"></param>
    /// <param name="lonEnd"></param>
    /// <returns>The distance between the two points in meters.</returns>
    public static double GetSimpleDistance(double latStart, double latEnd, double lonStart, double lonEnd)
    {
        double Lat_1 = ConvertDegreesToRadians(latStart);
        double Lon_1 = ConvertDegreesToRadians(lonStart);
        double Lat_2 = ConvertDegreesToRadians(latEnd);
        double Lon_2 = ConvertDegreesToRadians(lonEnd);

        const double r = 6378100; // meters

        var sdlat = Math.Sin((Lat_2 - Lat_1) / 2);
        var sdlon = Math.Sin((Lon_2 - Lon_1) / 2);
        var q = sdlat * sdlat + Math.Cos(Lat_1) * Math.Cos(Lat_2) * sdlon * sdlon;
        var d = 2 * r * Math.Asin(Math.Sqrt(q));

        return d;
    }

    /// <summary>
    /// Calculates the simple distance between two geographical points specified by their latitude and longitude using the Haversine formula.
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns>The distance between the two points in meters.</returns>
    public static double GetSimpleDistance(double[] point1, double[] point2)
    {
        return GetSimpleDistance(point1[0], point2[0], point1[1], point2[1]);
    }
}
