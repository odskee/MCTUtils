using MCTUtils.Geo;
using System.Text.RegularExpressions;

namespace MCTUtils.Airspeed
{
    public static class AirspeedConverter
    {
        // --- Constants ---
        private const double Gamma = 1.4;
        private const double R = 287.05;
        private const double G = 9.80665;
        private const double A0 = 340.294;
        private const double P0 = 101325.0;
        private const double Rho0 = 1.225;
        private const double KtsToMps = 0.514444;
        private const double MpsToKts = 1.0 / KtsToMps;


        public static double IASToCAS(double ias) => ias;
        public static double CASToIAS(double cas) => cas;

        // CAS
        public static double CASToEAS(double casKnots, AtmosphereState atm)
        {
            double cas = casKnots * KtsToMps;

            double qc = P0 * (
                Math.Pow(1 + (Gamma - 1) / 2 * Math.Pow(cas / A0, 2),
                Gamma / (Gamma - 1)) - 1);

            double term = Math.Pow((qc / atm.PressurePa + 1),
                (Gamma - 1) / Gamma) - 1;

            double eas = Math.Sqrt(
                (2 * Gamma / (Gamma - 1)) *
                atm.PressurePa *
                term / Rho0);

            return eas * MpsToKts;
        }


        // EAS 
        public static double EASToCAS(double easKnots, AtmosphereState atm)
        {
            double eas = easKnots * KtsToMps;

            double qc = 0.5 * Rho0 * eas * eas;

            double term = Math.Pow((qc / atm.PressurePa + 1),
                (Gamma - 1) / Gamma) - 1;

            double cas = Math.Sqrt(
                (2 / (Gamma - 1)) *
                A0 * A0 *
                term);

            return cas * MpsToKts;
        }
        public static double EASToTAS(double easKnots, AtmosphereState atm)
        {
            return easKnots * Math.Sqrt(Rho0 / atm.Density);
        }


        // TAS
        public static double TASToEAS(double tasKnots, AtmosphereState atm)
        {
            return tasKnots * Math.Sqrt(atm.Density / Rho0);
        }
        public static double TASToIAS(double tas, double altitudeMeters, MetarData? metar = null)
        {
            var atm = GetAtmosphere(altitudeMeters, metar);

            double eas = TASToEAS(tas, atm);
            double cas = EASToCAS(eas, atm);

            return cas;
        }
        public static double TASToGS(double tasKnots, double headingDeg, double? windSpeedKnots = null, double? windDirDeg = null)
        {
            if (windSpeedKnots == null || windDirDeg == null)
            {
                return tasKnots; // no wind
            }

            double tasX = tasKnots * Math.Cos(Angle.ConvertDegreesToRadians(headingDeg));
            double tasY = tasKnots * Math.Sin(Angle.ConvertDegreesToRadians(headingDeg));

            // Convert "from" to "to"
            double windToDeg = (windDirDeg.Value + 180) % 360;

            double windX = windSpeedKnots.Value * Math.Cos(Angle.ConvertDegreesToRadians(windToDeg));
            double windY = windSpeedKnots.Value * Math.Sin(Angle.ConvertDegreesToRadians(windToDeg));

            double gsX = tasX + windX;
            double gsY = tasY + windY;

            return Math.Sqrt(gsX * gsX + gsY * gsY);
        }
        public static double TASToMach(double tasKnots, double altitudeMeters, MetarData? metar = null)
        {
            var atm = GetAtmosphere(altitudeMeters, metar);

            double tas = tasKnots * KtsToMps;
            double a = Math.Sqrt(Gamma * R * atm.TemperatureK);

            return tas / a;
        }



        // IAS
        public static double IASToTAS(double ias, double altitudeMeters, MetarData? metar = null)
        {
            var atm = GetAtmosphere(altitudeMeters, metar);

            double cas = ias; // IAS � CAS
            double eas = CASToEAS(cas, atm);

            return EASToTAS(eas, atm);
        }
        public static double IASToMach(double ias, double altitudeMeters)
        {
            return TASToMach(IASToTAS(ias, altitudeMeters), altitudeMeters);
        }



        // Mach
        public static double MachToTAS(double mach, double altitudeMeters, MetarData? metar = null)
        {
            var atm = GetAtmosphere(altitudeMeters, metar);

            double a = Math.Sqrt(Gamma * R * atm.TemperatureK);

            return mach * a * MpsToKts;
        }
        public static double MachToIAS(double mach, double altitudeMeters)
        {
            return TASToIAS(MachToTAS(mach, altitudeMeters), altitudeMeters);
        }
        public static double MachToGS(double mach, double altitudeMeters, MetarData? metar = null)
        {
            double tas = MachToTAS(mach, altitudeMeters, metar);

            // No wind ? GS = TAS
            if (metar?.WindSpeedKnots == null)
                return tas;

            throw new ArgumentException("Wind present: use overload with track");
        }
        public static double MachToGS(double mach, double altitudeMeters, double trackDeg, MetarData? metar)
        {
            if (metar?.WindSpeedKnots == null || metar?.WindDirectionDeg == null)
                return MachToTAS(mach, altitudeMeters, metar);

            double tas = MachToTAS(mach, altitudeMeters, metar);

            return TASToGS(
                tas,
                trackDeg,
                metar.WindSpeedKnots,
                metar.WindDirectionDeg);
        }



        // GS
        public static double GSToTAS(double gsKnots, double trackDeg, double? windSpeedKnots = null, double? windDirDeg = null)
        {
            // No wind -> TAS = GS
            if (windSpeedKnots == null || windDirDeg == null)
                return gsKnots;

            // GS vector (track direction)
            double gsX = gsKnots * Math.Cos(Angle.ConvertDegreesToRadians(trackDeg));
            double gsY = gsKnots * Math.Sin(Angle.ConvertDegreesToRadians(trackDeg));

            // Convert wind "from - to" direction
            double windToDeg = (windDirDeg.Value + 180) % 360;

            double windX = windSpeedKnots.Value * Math.Cos(Angle.ConvertDegreesToRadians(windToDeg));
            double windY = windSpeedKnots.Value * Math.Sin(Angle.ConvertDegreesToRadians(windToDeg));

            // TAS = GS - wind
            double tasX = gsX - windX;
            double tasY = gsY - windY;

            return Math.Sqrt(tasX * tasX + tasY * tasY);
        }
        public static double GSToTAS(double gsKnots, double trackDeg, MetarData? metar = null)
        {
            if (metar?.WindSpeedKnots == null || metar?.WindDirectionDeg == null)
                return gsKnots;

            return GSToTAS(
                gsKnots,
                trackDeg,
                metar.WindSpeedKnots,
                metar.WindDirectionDeg);
        }
        public static double GSToIAS(double gsKnots, double trackDeg, double altitudeMeters, MetarData? metar = null)
        {
            double tas = GSToTAS(gsKnots, trackDeg, metar);
            return TASToIAS(tas, altitudeMeters, metar);
        }
        public static double GSToMach(double gsKnots, double trackDeg, double altitudeMeters, MetarData? metar = null)
        {
            double tas = GSToTAS(gsKnots, trackDeg, metar);
            return TASToMach(tas, altitudeMeters, metar);
        }




        private static AtmosphereState GetAtmosphere(double geometricAltitudeMeters, MetarData? metar = null)
        {
            // 1 - Sea-level
            double seaLevelPressure = metar?.PressureHPa != null
                ? metar.PressureHPa.Value * 100.0
                : 101325.0;

            double seaLevelTempK = metar?.TemperatureC != null
                ? metar.TemperatureC.Value + 273.15
                : 288.15;

            // 2 - ISA baseline at alt
            double isaTemp = GetISATemperature(geometricAltitudeMeters);
            double deltaISA = (seaLevelTempK - 288.15); // delta_ISA based on surface temp deviation
            double actualTemp = isaTemp + deltaISA; // Apply deviation through column (standard aviation simplification)

            // 3 -  Pressure (barometric)
            double pressure = GetPressureWithSeaLevel(
                geometricAltitudeMeters,
                seaLevelPressure,
                actualTemp);

            // 4 - Density
            double density = pressure / (R * actualTemp);

            // 5-  Pressure Altitude
            double pressureAltitude = GetPressureAltitude(pressure);

            // 6 - Density Altitude (physical)
            double densityAltitude = GetDensityAltitude(density);

            return new AtmosphereState
            {
                TemperatureK = actualTemp,
                PressurePa = pressure,
                Density = density,
                PressureAltitudeMeters = pressureAltitude,
                DensityAltitudeMeters = densityAltitude,
                DeltaISA = actualTemp - isaTemp
            };
        }


        public static (double pressureAlt, double densityAlt) GetAltitudes(double altitudeMeters, MetarData? metar = null)
        {
            var atm = GetAtmosphere(altitudeMeters, metar);

            return (
                atm.PressureAltitudeMeters,
                atm.DensityAltitudeMeters
            );
        }
        private static double GetTemperature(double altitudeMeters, MetarData? metar = null)
        {
            if (metar?.TemperatureC != null)
            {
                // Simple linear deviation from ISA at sea level
                double isaTemp = 288.15 - 0.0065 * altitudeMeters;
                double delta = metar.TemperatureC.Value - 15.0;
                return isaTemp + delta;
            }

            // ISA fallback
            if (altitudeMeters <= 11000)
                return 288.15 - 0.0065 * altitudeMeters;

            return 216.65;
        }
        private static double GetISATemperature(double altitudeMeters)
        {
            if (altitudeMeters <= 11000)
                return 288.15 - 0.0065 * altitudeMeters;

            return 216.65;
        }
        private static double GetPressure(double altitudeMeters, MetarData? metar = null)
        {
            double seaLevelPressure = metar?.PressureHPa != null
                ? metar.PressureHPa.Value * 100.0
                : 101325.0;

            if (altitudeMeters <= 11000)
            {
                double t = GetTemperature(altitudeMeters, metar);
                return seaLevelPressure * Math.Pow(t / 288.15, 9.80665 / (287.05 * 0.0065));
            }
            else
            {
                double p11 = GetPressure(11000, metar);
                return p11 * Math.Exp(-9.80665 * (altitudeMeters - 11000) / (287.05 * 216.65));
            }
        }
        private static double GetPressureWithSeaLevel(double altitudeMeters, double seaLevelPressure, double tempK)
        {
            if (altitudeMeters <= 11000)
            {
                return seaLevelPressure *
                        Math.Pow(tempK / 288.15, G / (R * 0.0065));
            }
            else
            {
                double p11 = GetPressureWithSeaLevel(11000, seaLevelPressure, 216.65);
                return p11 * Math.Exp(-G * (altitudeMeters - 11000) / (R * 216.65));
            }
        }
        private static double GetPressureAltitude(double pressurePa)
        {
            return (288.15 / 0.0065) *
                   (1 - Math.Pow(pressurePa / 101325.0, (R * 0.0065) / G));
        }
        private static double GetDensityAltitude(double density)
        {
            // Invert ISA density equation numerically (this made my head hurt!)
            double h = 0; // initial guess

            for (int i = 0; i < 10; i++)
            {
                double t = GetISATemperature(h);
                double p = 101325.0 * Math.Pow(t / 288.15, G / (R * 0.0065));
                double rho = p / (R * t);

                double error = rho - density;

                h += error * 1000; // simple correction step
            }

            return h;
        }
    }


    public class MetarData
    {
        public double? WindDirectionDeg { get; set; }
        public double? WindSpeedKnots { get; set; }
        public double? TemperatureC { get; set; }
        public double? PressureHPa { get; set; }
    }

    public static class MetarParser
    {
        public static MetarData Parse(string metar)
        {
            var data = new MetarData();

            if (string.IsNullOrWhiteSpace(metar))
                return data;

            // Wind: 18015KT
            var windMatch = Regex.Match(metar, @"(\d{3})(\d{2,3})KT");
            if (windMatch.Success)
            {
                data.WindDirectionDeg = double.Parse(windMatch.Groups[1].Value);
                data.WindSpeedKnots = double.Parse(windMatch.Groups[2].Value);
            }

            // Temp/Dew: 15/08 or M05/M10
            var tempMatch = Regex.Match(metar, @"(M?\d{2})/(M?\d{2})");
            if (tempMatch.Success)
            {
                data.TemperatureC = ParseTemp(tempMatch.Groups[1].Value);
            }

            // Pressure QNH: Q1013
            var qnhMatch = Regex.Match(metar, @"Q(\d{4})");
            if (qnhMatch.Success)
            {
                data.PressureHPa = double.Parse(qnhMatch.Groups[1].Value);
            }

            // Pressure inches: A2992
            var altMatch = Regex.Match(metar, @"A(\d{4})");
            if (altMatch.Success)
            {
                double inchesHg = double.Parse(altMatch.Groups[1].Value) / 100.0;
                data.PressureHPa = inchesHg * 33.8639;
            }

            return data;
        }

        private static double ParseTemp(string val)
        {
            return val.StartsWith("M")
                ? -double.Parse(val.Substring(1))
                : double.Parse(val);
        }
    }

    public class AtmosphereState
    {
        public double TemperatureK { get; set; }
        public double PressurePa { get; set; }
        public double Density { get; set; }

        public double PressureAltitudeMeters { get; set; }
        public double DensityAltitudeMeters { get; set; }

        public double DeltaISA { get; set; } // �C
    }
}
