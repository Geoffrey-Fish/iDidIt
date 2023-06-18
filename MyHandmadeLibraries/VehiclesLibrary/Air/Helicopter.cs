﻿namespace Vehicles.Air
{
    public class Helicopter : Vehicle
    {
        public Helicopter(string name, string type, int velocity, double weight, int passengers, int maxAltitude,
            int kiloWatts, int fuelTank, int consumptionPerHour) : base(name, type, velocity, weight, passengers)
        {
            MaxAltitude = maxAltitude;
            KiloWatts = kiloWatts;
            FuelTank = fuelTank;
            ConsumptionPerHour = consumptionPerHour;
        }

        public override string Sound()
        {
            return "fapfapfapfapfapfapfap";
        }

        #region Stats

        public override string Name { get; set; }
        public override string Type { get; set; }
        public override int Velocity { get; set; }
        public override double Weight { get; set; }
        public override int Passengers { get; set; }

        /// <summary>
        ///     How high is too high? Yes
        /// </summary>
        public int MaxAltitude { get; set; }

        /// <summary>
        ///     Good ol' Horsepower
        /// </summary>
        public int KiloWatts { get; set; }

        /// How much fuel is in the tank
        /// </summary>
        private int FuelTank { get; set; }

        /// <summary>
        ///     How much fuel is burned per Hour, Average speed
        /// </summary>
        private int ConsumptionPerHour { get; set; }

        #endregion
    }
}