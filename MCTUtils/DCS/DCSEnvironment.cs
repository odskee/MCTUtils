using MCTUtils.Internal.DCS;
using MCTUtils.Internal.Geo;
using Proj4Net.Core;
using System.Text.Json;

namespace MCTUtils.DCS
{
    public class DCSEnvironment
    {
        private readonly string dCSProjectionString = "`+proj=tmerc +lat_0=0 +lon_0=${c_value} +k_0=${s_value} +x_0=${e_value} +y_0=${n_value} +towgs84=0,0,0,0,0,0,0 +units=m +vunits=m +ellps=WGS84 +no_defs +axis=neu`";

        private TheatreTranslation? theatreTranslation;


        /// <summary>
        /// Sets the translation parameters for the DCS environment using a TheatreTranslation object.
        /// </summary>
        /// <param name="Translation"></param>
        public void SetTranslationParameters(TheatreTranslation Translation)
        {
            theatreTranslation = Translation;
        }


        /// <summary>
        /// Converts a DCS Vec2 coordinate to decimal degrees using the provided DCSTerrain translation parameters.
        /// </summary>
        /// <param name="DCSVec2"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public BasicCoordinate DCSVec2ToDecimalDegrees(Vec2 DCSVec2)
        {
            if (theatreTranslation == null)
            {
                throw new InvalidOperationException("Theatre translation parameters have not been set. Please call SetTranslationParameters() before converting coordinates.");
            }

            BasicCoordinate resp = new();
            CoordinateReferenceSystemFactory crsFactory = new();
            CoordinateTransformFactory ctFactory = new();
            var sourceCRS = crsFactory.CreateFromParameters("DCS", DCSProjectionString(theatreTranslation.Central_meridian, theatreTranslation.Scale_factor, theatreTranslation.False_easting, theatreTranslation.False_northing));
            var targetCRS = crsFactory.CreateFromName("EPSG:3857");
            var transform = ctFactory.CreateTransform(sourceCRS, targetCRS);
            var sourceCoord = new Coordinate(DCSVec2.X, DCSVec2.Y);
            var targetCoord = new Coordinate();
            transform.Transform(sourceCoord, targetCoord);
            resp.Latitidue = targetCoord.Y;
            resp.Longtitude = targetCoord.X;
            return resp;
        }

        /// <summary>
        /// Converts a DCS Vec3 coordinate to decimal degrees using the provided DCSTerrain translation parameters.
        /// </summary>
        /// <param name="DCSVec3"></param>
        /// <returns>BasicCoordinate object containing the decimal degrees</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public BasicCoordinate DCSVec3ToDecimalDegrees(Vec3 DCSVec3)
        {
            if (theatreTranslation == null)
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
