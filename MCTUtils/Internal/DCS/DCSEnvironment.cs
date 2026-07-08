using MCTUtils.Geo;
using MCTUtils.Internal.DCS;
using Proj4Net.Core;
using System.Text.Json;

namespace MCTUtils.Internal.DCS
{
    public class DCSEnvironment
    {
        private string dCSProjectionString = "`+proj=tmerc +lat_0=0 +lon_0=${c_value} +k_0=${s_value} +x_0=${e_value} +y_0=${n_value} +towgs84=0,0,0,0,0,0,0 +units=m +vunits=m +ellps=WGS84 +no_defs +axis=neu`";

        
        public BasicCoordinate DCSVec2ToDecimalDegrees(Vec2 DCSVec2, DCSTerrain Terrain)
        {
            BasicCoordinate resp = new();
            CoordinateReferenceSystemFactory crsFactory = new CoordinateReferenceSystemFactory();
            CoordinateTransformFactory ctFactory = new CoordinateTransformFactory();
            var sourceCRS = crsFactory.CreateFromParameters("DCS", DCSProjectionString(Terrain.Translation!.Central_meridian, Terrain.Translation!.Scale_factor, Terrain.Translation!.False_easting, Terrain.Translation!.False_northing));
            var targetCRS = crsFactory.CreateFromName("EPSG:3857");
            var transform = ctFactory.CreateTransform(sourceCRS, targetCRS);
            var sourceCoord = new Coordinate(DCSVec2.X, DCSVec2.Y);
            var targetCoord = new Coordinate();
            transform.Transform(sourceCoord, targetCoord);
            resp.Latitidue = targetCoord.Y;
            resp.Longtitude = targetCoord.X;
            return resp;
        }

        public BasicCoordinate DCSVec3ToDecimalDegrees(Vec3 DCSVec3, DCSTerrain Terrain)
        {
            return DCSVec2ToDecimalDegrees(new(DCSVec3), Terrain);
        }

        public async Task<List<DCSTerrain>> GetDCSTerrainsAsync(string DCSTerrainDataDestination)
        {
            List<DCSTerrain> resp = new();
            foreach (var dataFile in Directory.EnumerateFiles(DCSTerrainDataDestination))
            {
                DCSTerrain terrain = (await JsonSerializer.DeserializeAsync<DCSTerrain>(File.OpenRead(dataFile)))!;
                resp.Add(terrain);
            }

            return resp;
        }

        public List<DCSTerrain> GetDCSTerrains(string DCSTerrainDataDestination)
        {
            List<DCSTerrain> resp = new();
            foreach (var dataFile in Directory.EnumerateFiles(DCSTerrainDataDestination))
            {
                resp.Add(JsonSerializer.Deserialize<DCSTerrain>(File.ReadAllText(dataFile))!);
            }
            return resp;
        }

        public string DCSProjectionString(int CentralMeridan, double ScaleFactor, double FalseEasting, double FalseNorthing)
        {
            return dCSProjectionString
                .Replace("{c_value}", CentralMeridan.ToString())
                .Replace("{s_value}", ScaleFactor.ToString())
                .Replace("{e_value}", FalseEasting.ToString())
                .Replace("{n_value}", FalseNorthing.ToString());
        }
    }
}
