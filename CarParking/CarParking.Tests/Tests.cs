using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Vehicles.Land;


namespace CarParking.Tests
{ // MethodToBeTested_WhatIsTested_WhatIsResult
    [TestFixture]
    public class ParkHouseTests
    {
        
        public static Car car1 = new Car("Car1", "bus", 40, 500, 6, "green", 4,
            90, 50, 8);
        public static Car car2 = new Car("Car1", "bus", 40, 500, 6, "green", 4,
            90, 50, 8);
        public static Car car3 = new Car("Car1", "bus", 40, 500, 6, "green", 4,
            90, 50, 8);
        public static Car car4 = new Car("Car1", "bus", 40, 500, 6, "green", 4,
            90, 50, 8);
        public static Car car5 = new Car("Car1", "bus", 40, 500, 6, "green", 4,
            90, 50, 8);
        public static Car car6 = new Car("Car1", "bus", 40, 500, 6, "green", 4,
            90, 50, 8);
        
        [Test]
        public void Parking_WhenCalled_ReturnStatusOk()
        {
            ParkHouse parkhouse = new ParkHouse(5);
            parkhouse.Parking(car1);
            //string match = "Is parked.";
             string match = @"Car1 parked at Lot Number: \d+\.$";
            Assert.That(Regex.IsMatch(parkhouse.OkMessage, match));
        }


        [Test]
        public void Leaving_WhenCalled_OkLightTrue()
        {
            ParkHouse parkhouse = new ParkHouse(5);
            parkhouse.TestSetParkLot(car2,0);
            parkhouse.OkLight = false;
            parkhouse.Leaving(0);
            Assert.That(parkhouse.OkLight == true);
        }
        
        
        [Test]
        public void IsLotFree_WhenLotsFree_ReturnTrue()
        {
            ParkHouse parkhouse = new ParkHouse(5);
            parkhouse.TestSetParkLot(car3,3);
            bool result = parkhouse.IsLotFree(2);
            Assert.That(result == true);
        }

        [Test]
        public void IsLotFree_WhenLotsFull_ReturnFalse()
        {
            ParkHouse parkhouse = new ParkHouse(5);
            parkhouse.TestSetParkLot(car3,3);
            bool result = parkhouse.IsLotFree(3);
            Assert.That(result == false);
        }
        
        [Test]
        public void IsHouseFull_WhenHouseFull_ReturnTrue() 
        {
            ParkHouse parkhouse = new ParkHouse(5);
            parkhouse.TestSetParkLot(car1,0);
            parkhouse.TestSetParkLot(car2,1);
            parkhouse.TestSetParkLot(car3,2);
            parkhouse.TestSetParkLot(car4,3);
            parkhouse.TestSetParkLot(car5,4);
            bool result = parkhouse.IsHouseFull();
            Assert.That(result == true);
        }
        [Test]
        public void IsHouseFull_WhenHouseNotFull_ReturnFalse() 
        {
            ParkHouse parkhouse = new ParkHouse(5);
            parkhouse.TestSetParkLot(car1,0);
            parkhouse.TestSetParkLot(car2,1);
            parkhouse.TestSetParkLot(car3,2);
            parkhouse.TestSetParkLot(car4,3);
            bool result = parkhouse.IsHouseFull();
            Assert.That(result == false);
        }
        [Test]
        public void GetFreeLots_WhenCalled_ReturnFreeLotsAmount()
        {
            ParkHouse parkhouse = new ParkHouse(5);
            parkhouse.TestSetParkLot(car1,0);
            parkhouse.TestSetParkLot(car2,1);
            parkhouse.TestSetParkLot(car3,2);
            parkhouse.TestSetParkLot(car4,3);
            int result = parkhouse.GetFreeLots();
            Assert.That(result == 4);
        }
        [Test]
        public void GetVehicleByLicencePlate_WhenTested_ReturnLotNumber()
        {
            ParkHouse parkhouse = new ParkHouse(5);
            string lPlate = car1.LicencePlate;
            parkhouse.TestSetParkLot(car1,0);
            parkhouse.TestSetParkLot(car3, 3);
            int result = parkhouse.GetVehicleByLicencePlate(lPlate);
            Assert.That(result == 0);
        }
        [Test]
        public void GetVehicleCountByType_WhenTested_ReturnAmountOfVehicles() 
        {
            ParkHouse parkhouse = new ParkHouse(5);
            parkhouse.TestSetParkLot(car1,0);
            parkhouse.TestSetParkLot(car3, 3);
            string tType = car1.Type;
            int result = parkhouse.GetVehicleCountByType(tType);
            Assert.That(result == 2);
        }
    }
}