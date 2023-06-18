namespace Vehicles.Land
{
    public class Bycicle : Vehicle
    {
        public Bycicle(string name, string type, int velocity, double weight, int passengers, bool pedelec) : base(name,
            type, velocity, weight, passengers)
        {
            Pedelec = pedelec;
        }

        public override string Sound()
        {
            return "*Wheezing noises*";
        }

        #region Stats

        public override string Name { get; set; }
        public override string Type { get; set; }
        public override int Velocity { get; set; }
        public override double Weight { get; set; }
        public override int Passengers { get; set; }

        /// <summary>
        ///     Is it a mans bike or Grannys?
        /// </summary>
        public bool Pedelec { get; set; }

        #endregion
    }
}