namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// Basic fuel planning data.
/// </summary>
public class FuelPlan
{
    /// <summary>
    /// Bingo fuel state in pounds.
    /// </summary>
    [JsonPropertyName("bingo_lbs")]
    public double? BingoLbs { get; set; }

    /// <summary>
    /// Joker fuel state in pounds.
    /// </summary>
    [JsonPropertyName("joker_lbs")]
    public double? JokerLbs { get; set; }

    /// <summary>
    /// Planned initial fuel load in pounds.
    /// </summary>
    [JsonPropertyName("initial_fuel_lbs")]
    public double? InitialFuelLbs { get; set; }
}
