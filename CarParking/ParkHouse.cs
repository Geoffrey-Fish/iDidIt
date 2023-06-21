using System;
using System.IO;
using System.Linq;
using System.Net;
using Vehicles;
using Vehicles.Land;
using Newtonsoft.Json;

namespace CarParking
{
    public class ParkHouse
    {
        #region stats
        /// <summary>
        /// Status Light if void job successfull
        /// </summary>
        public bool OkLight { get; set; }= false;
        /// <summary>
        /// Error Signal for ErrortextBox
        /// </summary>
        public bool ErrorLight { get; set; }= false;
        /// <summary>
        /// MessageBoard if action is success
        /// </summary>
        public string OkMessage = "";
        /// <summary>
        /// MessageBoard if Error occours
        /// </summary>
        public string ErrorMessage = "";
        /// <summary>
        /// Sum of all Lots
        /// </summary>
        private int TotalLots { get; set; }
        /// <summary>
        /// Free Lots
        /// </summary>
        private int FreeLots { get; set; }
        /// <summary>
        /// Used Lots
        /// </summary>
        private int UsedLots { get; set; }
        /// <summary>
        /// List of all parked vehicles
        /// </summary>
        private Vehicle[] parkingLots;
       #endregion

       /// <summary>
       /// Empty stub for loading from json save
       /// </summary>
       public ParkHouse()
       {
       }

       public static ParkHouse LoadParkHouse()
       {
           string path = "..\\parkHouseData.json";
           if (File.Exists(path))
           {
               string json = File.ReadAllText(path);
               return JsonConvert.DeserializeObject<ParkHouse>(json);
           }
           else
           {
               return null;
           }
       }

       /// <summary>
        /// New ParkHouse
        /// </summary>
        /// <param name="Lots">Amount of Lots available</param>
        public ParkHouse(int Lots)
        {
            parkingLots = new Vehicle[Lots]; //New Array for the Vehicles to come
            for (int i = 0; i < Lots; i++)//Assign null for every lot
            {
                parkingLots[i] = null;
            }
            TotalLots = Lots;
            FreeLots = Lots;
            UsedLots = 0;
            OkMessage = $"Parkhouse with {Lots.ToString()} successfull created.";
            OkLight = true;
            SaveAllData();
        }
        
        /// <summary>
        /// Check if there is space
        /// </summary>
        /// <param name="vehicle">the car to park</param>
        /// <returns>string if success, else error</returns>
        public void Parking(Vehicle vehicle)
        {
            if(IsHouseFull()) ErrorMessage = "Error, full house!";
            int searchFreeLot = RandomLotChooser(); //search random lot
            if (vehicle.GetType() == typeof(Car))
            {
                parkingLots[searchFreeLot] = vehicle as Car;
            }
            else if (vehicle.GetType() == typeof(Motorcycle))
            {
                parkingLots[searchFreeLot] = vehicle as Motorcycle;
            }
            OkLight = true;
            FreeLots--;
            UsedLots++;
            SaveAllData();
            //OkMessage = "Is parked.";
            OkMessage = $"{vehicle.Name} parked at Lot Number: {searchFreeLot.ToString()}.";
        }

        /// <summary>
        /// remove car from lot and give it free again
        /// </summary>
        public void Leaving(int lot)
        {
            if (IsLotFree(lot)) 
            {
                ErrorLight = true;
                ErrorMessage = "Lot is allready free";
            }
            parkingLots[lot] = null;
            OkLight = true;
            FreeLots++;
            UsedLots--;
            SaveAllData();
        }

        /// <summary>
        /// Checks if Lot is free(for whatever reason)
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        public bool IsLotFree(int lot)
        {
            return parkingLots[lot] == null;
        }

        /// <summary>
        ///     Is Parkhouse full
        /// </summary>
        /// <returns>true if full</returns>
        public bool IsHouseFull()
        {
            return !parkingLots.Contains(null);
        }

        /// <summary>
        ///Are there free Lots
        /// </summary>
        /// <returns>amount of free lots</returns>
        public int GetFreeLots()
        {
            int i = 0;
            foreach (Vehicle vec in parkingLots)
            {
                if (vec != null)
                {
                    i++;
                }
            }

            return i;
        }

        /// <summary>
        ///Search Vehicle by Plate
        /// </summary>
        /// <returns>LotNumber</returns>
        public int GetVehicleByLicencePlate(string licencePlate)
        {
            //GENIUS: That was quite a piece of Work
            var correctVehicle = parkingLots.FirstOrDefault(vehicle => vehicle is Car car && car.LicencePlate == licencePlate ||
                                                            vehicle is Motorcycle moto &&
                                                            moto.LicencePlate == licencePlate);
            if (correctVehicle != null)
            {
                return Array.IndexOf(parkingLots, correctVehicle);
            }

            return 0;
        }
        /// <summary>
        /// Search for Vehicles by type //Todo: make dropdown with types current in house
        /// </summary>
        /// <returns>Amount with same type</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetVehicleCountByType(string type)
        {
        return parkingLots.Count(vehicle => vehicle is Car car && car.Type == type ||
                                                                       vehicle is Motorcycle moto &&
                                                                       moto.Type == type);
        }
        /// <summary>
        /// For more fun and diversity, choose random parkinglot
        /// </summary>
        /// <returns>number of parking lot to use</returns>
        private int RandomLotChooser()
        {
            Random rng = new Random();
            int place = rng.Next(TotalLots);
            place = parkingLots[place] != null ? RandomLotChooser() : place; //Look if chosen plase is free, else reroll until it is
            return place;
        }
/// <summary>
/// TestSetup, stupid, but works
/// </summary>
/// <param name="vec">which vehicle to place</param>
/// <param name="i">on which position in the Array</param>
        public void TestSetParkLot(Vehicle vec, int i)
        {
            parkingLots[i] = vec;
        }

        /// <summary>
        /// Save the state of the Parkhouse
        /// idea: this is a test if i can save that parkhouse itself
        /// </summary>
        public void SaveAllData()//Vehicle[] parkingLots)
        {
            string path = "..\\parkHouseData.json";
            try
            {
                var serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(path))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, this);
                }
                OkMessage = "Data saved successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error occurred while saving data: {ex.Message}";
            }
        }
    }
}