using System.Text.Json;
using MCTUtils.CommunityStandards.Common;
using MCTUtils.CommunityStandards.Serialization;

namespace MCTUtils.CommunityStandards.Tests;

public class EnumTests
{
    private static readonly JsonSerializerOptions Options = JsonSerializerOptionsFactory.CreateDefault();

    [Fact]
    public void Coalition_SerializesAndDeserializes()
    {
        var json = JsonSerializer.Serialize(Coalition.BLUE, Options);
        Assert.Contains("BLUE", json);

        var deserialized = JsonSerializer.Deserialize<Coalition>("\"BLUE\"", Options);
        Assert.Equal(Coalition.BLUE, deserialized);
    }

    [Fact]
    public void FlightRules_SerializesAndDeserializes()
    {
        var json = JsonSerializer.Serialize(FlightRules.I, Options);
        Assert.Contains("\"I\"", json);

        var deserialized = JsonSerializer.Deserialize<FlightRules>("\"V\"", Options);
        Assert.Equal(FlightRules.V, deserialized);
    }

    [Fact]
    public void FlightType_AllValuesRoundTrip()
    {
        foreach (var value in Enum.GetValues<FlightType>())
        {
            var json = JsonSerializer.Serialize(value, Options);
            var deserialized = JsonSerializer.Deserialize<FlightType>(json, Options);
            Assert.Equal(value, deserialized);
        }
    }

    [Fact]
    public void Coalition_AllValuesRoundTrip()
    {
        foreach (var value in Enum.GetValues<Coalition>())
        {
            var json = JsonSerializer.Serialize(value, Options);
            var deserialized = JsonSerializer.Deserialize<Coalition>(json, Options);
            Assert.Equal(value, deserialized);
        }
    }

    [Fact]
    public void MissionType_AllValuesRoundTrip()
    {
        foreach (var value in Enum.GetValues<MissionType>())
        {
            var json = JsonSerializer.Serialize(value, Options);
            var deserialized = JsonSerializer.Deserialize<MissionType>(json, Options);
            Assert.Equal(value, deserialized);
        }
    }

    [Fact]
    public void WaypointType_AllValuesRoundTrip()
    {
        foreach (var value in Enum.GetValues<WaypointType>())
        {
            var json = JsonSerializer.Serialize(value, Options);
            var deserialized = JsonSerializer.Deserialize<WaypointType>(json, Options);
            Assert.Equal(value, deserialized);
        }
    }

    [Fact]
    public void EmergencyCode_UsesCorrectJsonValues()
    {
        var json = JsonSerializer.Serialize(EmergencyCode._7500, Options);
        Assert.Contains("7500", json);

        var deserialized = JsonSerializer.Deserialize<EmergencyCode>("\"7600\"", Options);
        Assert.Equal(EmergencyCode._7600, deserialized);
    }
}
