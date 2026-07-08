using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MCTUtils.Internal.DCS
{
    public partial class DCSTerrain
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();


        public string TerrainName { get; set; } = String.Empty;
        public string DisplayName { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string SimName { get; set; } = String.Empty;
        public double CenterLatitudeDD { get; set; } = 0;
        public double CenterLongitudeDD { get; set; } = 0;
        public bool Enabled { get; set; } = true;
        public TheatreTranslation? Translation { get; set; } = null;
        public bool SimTranslation { get; set; } = false;
        public int YearOfTerrain { get; set; } = 2001;
    }
}
