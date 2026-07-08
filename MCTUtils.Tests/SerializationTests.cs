using System.Text.Json;
using MCTUtils.CommunityStandards.Common;
using FlightPlanDocument = MCTUtils.CommunityStandards.CommunityFlightPlan.CommunityFlightPlan;
using OpTaskAirDocument = MCTUtils.CommunityStandards.OpTaskAir.OpTaskAir;
using MCTUtils.CommunityStandards.OpTaskAir;
using MCTUtils.CommunityStandards.Serialization;

namespace MCTUtils.CommunityStandards.Tests;

public class SerializationTests
{
    private static readonly JsonSerializerOptions Options = JsonSerializerOptionsFactory.CreateDefault();

    private static Route CreateRoute()
    {
        var route = new Route { Id = Guid.NewGuid(), AssetId = Guid.NewGuid() };
        route.Legs.Add(new RouteLeg { LegName = "leg 1", StartWaypoint = Guid.NewGuid(), EndWaypoint = Guid.NewGuid() });
        return route;
    }

    private static Asset CreateAsset(string callsign, string airframe)
    {
        return new Asset
        {
            Id = Guid.NewGuid(),
            PackageId = Guid.NewGuid(),
            Callsign = callsign,
            FlightNumber = 2,
            Airframe = airframe,
            MissionType = MissionType.Strike,
            RouteId = Guid.NewGuid()
        };
    }

    [Fact]
    public void CommunityFlightPlan_SerializationRoundTrip()
    {
        var plan = new FlightPlanDocument
        {
            SchemaVersion = "1.1.0",
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            Coalition = Coalition.BLUE
        };
        plan.Package.Add(new Package { Id = Guid.NewGuid(), Name = "PACKAGE TEST" });
        plan.Assets.Add(CreateAsset("VIPER 1", "F-16CM"));
        plan.Routes.Add(CreateRoute());

        var json = JsonSerializer.Serialize(plan, Options);
        Assert.NotNull(json);
        Assert.Contains("community-flightplan", json);
        Assert.Contains("VIPER 1", json);
        Assert.Contains("F-16CM", json);

        var deserialized = JsonSerializer.Deserialize<FlightPlanDocument>(json, Options);
        Assert.NotNull(deserialized);
        Assert.Equal(plan.SchemaVersion, deserialized.SchemaVersion);
        Assert.Equal(plan.Id, deserialized.Id);
        Assert.Equal(plan.Coalition, deserialized.Coalition);
        Assert.Single(deserialized.Assets);
        Assert.Equal("VIPER 1", deserialized.Assets[0].Callsign);
    }

    [Fact]
    public void OpTaskAir_SerializationRoundTrip()
    {
        var opTaskAir = new OpTaskAirDocument
        {
            SchemaVersion = "1.1.0",
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            Coalition = Coalition.RED,
            MissionContext = new MissionContext
            {
                Theatre = "Caucasus",
                Date = new DateOnly(2025, 3, 15),
                TimeSeconds = 25200
            }
        };
        opTaskAir.Package.Add(new Package { Id = Guid.NewGuid(), Name = "PACKAGE SWIFT" });
        opTaskAir.Assets.Add(CreateAsset("VIPER 1", "Su-27"));
        opTaskAir.Routes.Add(CreateRoute());

        var json = JsonSerializer.Serialize(opTaskAir, Options);
        Assert.NotNull(json);
        Assert.Contains("community-op-task-air", json);
        Assert.Contains("Caucasus", json);
        Assert.Contains("Su-27", json);

        var deserialized = JsonSerializer.Deserialize<OpTaskAirDocument>(json, Options);
        Assert.NotNull(deserialized);
        Assert.Equal(opTaskAir.SchemaVersion, deserialized.SchemaVersion);
        Assert.Equal(opTaskAir.Id, deserialized.Id);
        Assert.Equal("Caucasus", deserialized.MissionContext.Theatre);
    }

    [Fact]
    public void LatLon_SerializationRoundTrip()
    {
        var point = new LatLon { Latitude = 42.1673, Longitude = 41.611 };

        var json = JsonSerializer.Serialize(point, Options);
        Assert.Contains("42.1673", json);
        Assert.Contains("41.611", json);

        var deserialized = JsonSerializer.Deserialize<LatLon>(json, Options);
        Assert.NotNull(deserialized);
        Assert.Equal(point.Latitude, deserialized.Latitude);
        Assert.Equal(point.Longitude, deserialized.Longitude);
    }

    [Fact]
    public void CommsEntry_SerializationRoundTrip()
    {
        var entry = new CommsEntry
        {
            Role = CommsRole.ASSET,
            Label = "STRIKE COMMON",
            FrequencyMhz = 264.0,
            Modulation = Modulation.AM,
            Callsign = null
        };

        var json = JsonSerializer.Serialize(entry, Options);
        Assert.Contains("STRIKE COMMON", json);
        Assert.Contains("264", json);

        var deserialized = JsonSerializer.Deserialize<CommsEntry>(json, Options);
        Assert.NotNull(deserialized);
        Assert.Equal(CommsRole.ASSET, deserialized.Role);
        Assert.Equal(264.0, deserialized.FrequencyMhz);
    }

    [Fact]
    public void HelperMethods_RoundTrip()
    {
        var plan = new FlightPlanDocument
        {
            SchemaVersion = "1.1.0",
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            Coalition = Coalition.BLUE
        };
        plan.Package.Add(new Package { Id = Guid.NewGuid(), Name = "TEST" });
        plan.Assets.Add(CreateAsset("TEST", "F-16CM"));
        plan.Routes.Add(CreateRoute());

        var json = plan.ToJson();
        Assert.NotNull(json);

        var deserialized = FlightPlanDocument.FromJson(json);
        Assert.NotNull(deserialized);
        Assert.Equal(plan.Id, deserialized.Id);
    }
}
