using MCTUtils.Internal.DCS;
using MCTUtils.Internal.Geo;
using Proj4Net.Core;
using System.Text.Json;

namespace MCTUtils.DCS
{
    public class DCSEnvironment
    {
        private string dCSProjectionString = "+proj=tmerc +lat_0=0 +lon_0={c_value} +k_0={s_value} +x_0={e_value} +y_0={n_value} +units=m +ellps=WGS84";

        private readonly TheatreTranslation? theatreTranslation;
        
        private readonly CoordinateReferenceSystemFactory crsFactory = new();
        private readonly CoordinateTransformFactory ctFactory = new();
        private readonly CoordinateReferenceSystem dcsCrs;
        private readonly CoordinateReferenceSystem wgs84Crs;
        private readonly ICoordinateTransform toGeoTransform;
        private readonly ICoordinateTransform toDCSTransform;



        /// <summary>
        /// Initializes a new instance of the <see cref="DCSEnvironment"/> class with the specified theatre translation parameters.
        /// </summary>
        /// <param name="translation"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public DCSEnvironment(TheatreTranslation translation)
        {
            if (translation == null)
            {
                throw new InvalidOperationException("Theatre translation parameters have not been set. Please call SetTranslationParameters() before converting coordinates.");
            }

            theatreTranslation = translation;

            dcsCrs = crsFactory.CreateFromParameters(
                "DCS",
                DCSProjectionString(
                    translation.Central_meridian,
                    translation.Scale_factor,
                    translation.False_easting,
                    translation.False_northing));

            wgs84Crs = crsFactory.CreateFromName("EPSG:4326");

            toGeoTransform = ctFactory.CreateTransform(dcsCrs, wgs84Crs);
            toDCSTransform = ctFactory.CreateTransform(wgs84Crs, dcsCrs);
        }


        /// <summary>
        /// Converts a decimal degrees coordinate to a DCS Vec2 coordinate using the provided DCSTerrain translation parameters.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns>Vec2 object containing the DCS coordinates</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Vec2 DecimalDegreesToDCSVec2(Coordinate coordinate)
        {
            if (toDCSTransform == null)
            {
                throw new InvalidOperationException("Theatre translation parameters have not been set. Please call SetTranslationParameters() before converting coordinates.");
            }

            Coordinate targetCoord = new Coordinate();
            toDCSTransform.Transform(coordinate, targetCoord);
            return new() { X = targetCoord.Y, Y = targetCoord.X };
        }



        /// <summary>
        /// Converts a DCS Vec2 coordinate to decimal degrees using the provided DCSTerrain translation parameters.
        /// </summary>
        /// <param name="DCSVec2"></param>
        /// <returns>BasicCoordinate object containing the decimal degrees</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public BasicCoordinate DCSVec2ToDecimalDegrees(Vec2 DCSVec2)
        {
            if (toGeoTransform == null)
            {
                throw new InvalidOperationException("Theatre translation parameters have not been set. Please call SetTranslationParameters() before converting coordinates.");
            }

            Coordinate sourceCoord = new Coordinate(DCSVec2.Y, DCSVec2.X);
            Coordinate targetCoord = new Coordinate();
            toGeoTransform.Transform(sourceCoord, targetCoord);
            return new() { Latitidue = targetCoord.Y, Longtitude = targetCoord.X };
        }

        /// <summary>
        /// Converts a DCS Vec3 coordinate to decimal degrees using the provided DCSTerrain translation parameters.
        /// </summary>
        /// <param name="DCSVec3"></param>
        /// <returns>BasicCoordinate object containing the decimal degrees</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public BasicCoordinate DCSVec3ToDecimalDegrees(Vec3 DCSVec3)
        {
            if (toGeoTransform == null)
            {
                throw new InvalidOperationException("Theatre translation parameters have not been set. Please call SetTranslationParameters() before converting coordinates.");
            }

            return DCSVec2ToDecimalDegrees(new(DCSVec3));
        }


        /// <summary>
        /// Generates a Proj4 string for the DCS projection based on the provided parameters.
        /// </summary>
        /// <param name="CentralMeridan"></param>
        /// <param name="ScaleFactor"></param>
        /// <param name="FalseEasting"></param>
        /// <param name="FalseNorthing"></param>
        /// <returns>Proj4 string for the DCS projection</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public string DCSProjectionString(int CentralMeridan, double ScaleFactor, double FalseEasting, double FalseNorthing)
        {
            if (theatreTranslation == null)
            {
                throw new InvalidOperationException("Theatre translation parameters have not been set. Please call SetTranslationParameters() before converting coordinates.");
            }

            return dCSProjectionString
                .Replace("{c_value}", CentralMeridan.ToString())
                .Replace("{s_value}", ScaleFactor.ToString())
                .Replace("{e_value}", FalseEasting.ToString())
                .Replace("{n_value}", FalseNorthing.ToString());
        }
    }
}
