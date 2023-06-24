using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Vehicles;
using Vehicles.Land;

namespace CarParking
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // bool failsafes what kind of vehicle is currently generated
        private static bool carBtnActive;
        private static bool moCycleBtnActive;
        private static bool truckBtnActive;
        private Car _car;
        private Motorcycle _mocycle;
        private Truck _truck;

        // static object Instances
        public ParkHouse parkHouse = new ParkHouse(10); //Main Instance Code logic

        public MainWindow()
        {
            InitializeComponent();
          /*  //Binding licencePlate searchbox
            var bindingLP = new Binding("ItemsSource");
            bindingLP.Source = parkHouse.LicencePlates;
            searchLicence_cobx.SetBinding(ItemsControl.ItemsSourceProperty, bindingLP);
            //Binding Type Searchbox
            var bindingTP = new Binding("ItemsSource");
            bindingTP.Source = parkHouse.Models;
            searchType_cobx.SetBinding(ItemsControl.ItemsSourceProperty, bindingTP);
            //Binding Lots used SearchBox
            var bindingLT = new Binding("ItemsSource");
            bindingLT.Source = parkHouse.UsedLotNumbers;
            searchLot_cobx.SetBinding(ItemsControl.ItemsSourceProperty, bindingLT);
            //Binding Free lots ticker
            var bindingFL = new Binding("FreeLots");
            bindingFL.Source = parkHouse;
            freeTicker_tbx.SetBinding(TextBox.TextProperty, bindingFL);
            //Binding used lots ticker
            var bindingUT = new Binding("UsedLots");
            bindingUT.Source = parkHouse;
            usedTicker_tbx.SetBinding(TextBox.TextProperty, bindingUT);
            
        */
          DataContext = parkHouse;
        }

        private void CreatePhouse_btn_OnClick(object sender, RoutedEventArgs e)
        {
            // todo: setting for money cost
            if (lotAmount_tbx.Text != null)
            {
                var bla = int.TryParse(lotAmount_tbx.Text, out var lots);
                parkHouse = new ParkHouse(lots);
                if (parkHouse.OkLight)
                {
                    okLight_btn.Background = Brushes.Green;
                    resultText_tbx.Text = parkHouse.OkMessage;
                }
                else if (parkHouse.ErrorLight)
                {
                    errorLight_btn.Background = Brushes.Red;
                }
            }
            else
            {
                errorLight_btn.Background = Brushes.Red;
                errorText_tbx.Text = "Error, please enter Lot amount!";
            }

            createPhouse_btn.Focusable = false;
            lotAmount_tbx.Text = null;
        }

        /// <summary>
        ///     Load last configured Parkhouse
        /// </summary>
        private void LoadLastPh_btn_OnClick(object sender, RoutedEventArgs e)
        {
            parkHouse = ParkHouse.LoadParkHouse();
            if (parkHouse != null)
            {
                okLight_btn.Background = Brushes.Green;
                resultText_tbx.Text = "Last Parkhouse successfull loaded";
            }
            else
            {
                errorLight_btn.Background = Brushes.Red;
                errorText_tbx.Text = "No Parkhouse found.Please create a new instance.";
            }

            loadLastPh_btn.Focusable = false;
            createPhouse_btn.Focusable = true;
        }

        /// <summary>
        ///     This fills the infoboxes with data of current chosen vehicle, wether by generating or search result.
        /// </summary>
        /// <param name="vec"></param>
        private void showVehicleInfos(Vehicle vec)
        {
            name_tbx.Text = vec.Name;
            if (vec is Car car)
            {
                licence_tbx.Text = car.LicencePlate;
                type_tbx.Text = "Car";
                model_tbx.Text = car.Type;
            }
            else if (vec is Motorcycle mo)
            {
                licence_tbx.Text = mo.LicencePlate;
                type_tbx.Text = "MotorBike";
                model_tbx.Text = mo.Type;
            }
            else if (vec is Truck truck)
            {
                licence_tbx.Text = truck.LicencePlate;
                type_tbx.Text = "Truck";
                model_tbx.Text = truck.Type;
            }
        }

        /// <summary>
        ///     Park in Call, check which type is currently chosen via BtnActive
        /// </summary>
        private void ParkIn_btn_OnClick(object sender, RoutedEventArgs e)
        {
            if (carBtnActive)
            {
                parkHouse.Parking(_car);
            }
            else if (moCycleBtnActive)
            {
                parkHouse.Parking(_mocycle);
            }
            else if (truckBtnActive)
            {
                parkHouse.Parking(_truck);
            }
            else
            {
                errorLight_btn.Background = Brushes.Red;
                errorText_tbx.Text = "You need to choose a vehicle first";
            }

            if (parkHouse.OkLight)
            {
                okLight_btn.Background = Brushes.Green;
                resultText_tbx.Text = parkHouse.OkMessage;
            }
            else if (parkHouse.ErrorLight)
            {
                errorLight_btn.Background = Brushes.Red;
                errorText_tbx.Text = parkHouse.ErrorMessage;
            }
        }

        private void DriveOut_btn_OnClick(object sender, RoutedEventArgs e)
        {
            //todo implement buttonbool when vehicle chosen via dropboxes
            if (carBtnActive)
            {
                parkHouse.Leaving(_car);
                okLight_btn.Background = Brushes.Green;
                resultText_tbx.Text = parkHouse.OkMessage;
            }
            else if (moCycleBtnActive)
            {
                parkHouse.Leaving(_mocycle);
                okLight_btn.Background = Brushes.Green;
                resultText_tbx.Text = parkHouse.OkMessage;
            }
            else if (truckBtnActive)
            {
                parkHouse.Leaving(_truck);
                okLight_btn.Background = Brushes.Green;
                resultText_tbx.Text = parkHouse.OkMessage;
            }
        }

        private void SearchLicence_btn_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var vehicle in parkHouse.parkingLots)
                if (vehicle is Car car)
                {
                    if (car.LicencePlate == searchLicence_cobx.Text) showVehicleInfos(car);
                }
                else if (vehicle is Motorcycle mo)
                {
                    if (mo.LicencePlate == searchLicence_cobx.Text) showVehicleInfos(mo);
                }
                else if (vehicle is Truck truck)
                {
                    if (truck.LicencePlate == searchLicence_cobx.Text) showVehicleInfos(truck);
                }
        }

        private void SearchType_btn_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var vehicle in parkHouse.parkingLots)
                if (vehicle is Car car)
                {
                    if (car.Type == searchLicence_cobx.Text) showVehicleInfos(car);
                }
                else if (vehicle is Motorcycle mo)
                {
                    if (mo.Type == searchLicence_cobx.Text) showVehicleInfos(mo);
                }
                else if (vehicle is Truck truck)
                {
                    if (truck.Type == searchLicence_cobx.Text) showVehicleInfos(truck);
                }
        }

        private void SearchLot_btn_OnClick(object sender, RoutedEventArgs e)
        {
            var vehicle = parkHouse.parkingLots[Convert.ToInt32(searchLot_cobx.Text)];
            {
                if (vehicle is Car car)
                {
                    if (car.LicencePlate == searchLicence_cobx.SelectedItem) showVehicleInfos(car);
                }
                else if (vehicle is Motorcycle mo)
                {
                    if (mo.LicencePlate == searchLicence_cobx.SelectedItem) showVehicleInfos(mo);
                }
                else if (vehicle is Truck truck)
                {
                    if (truck.LicencePlate == searchLicence_cobx.SelectedItem) showVehicleInfos(truck);
                }
            }
        }

// todo: resetbutton active if fields not empty check
        private void ResultReset_btn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

// todo: resetbutton active if fields not empty check
        private void ErrorReset_btn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Reset the textboxes and bools and all
        /// </summary>
        private void ClearVehicleDataWindows_btn_OnClick(object sender, RoutedEventArgs e) //for the data in bottomleft
        {
            carBtnActive = false;
            moCycleBtnActive = false;
            truckBtnActive = false;
            okLight_btn.Background = Brushes.Gray;
            resultText_tbx.Text = null;
            errorLight_btn.Background = Brushes.Gray;
            errorText_tbx.Text = null;
            name_tbx.Text = null;
            type_tbx.Text = null;
            licence_tbx.Text = null;
            model_tbx.Text = null;
            clearVehicleDataWindows_btn.Focusable = false; //inactive button again
        }

        #region Vehicle buttons

        /// <summary>
        ///     create new random car
        /// </summary>
        private void Car_btn_OnClick(object sender, RoutedEventArgs e)
        {
            _car = VehicleGenerator.CarGenerator();
            showVehicleInfos(_car);
            carBtnActive = true;
            moCycleBtnActive = false;
            truckBtnActive = false;
            clearVehicleDataWindows_btn.Focusable = true;
        }

        /// <summary>
        ///     create new random bike
        /// </summary>
        private void MoCy_btn_OnClick(object sender, RoutedEventArgs e)
        {
            _mocycle = VehicleGenerator.MotorcycleGenerator();
            showVehicleInfos(_mocycle);
            carBtnActive = false;
            moCycleBtnActive = true;
            truckBtnActive = false;
            clearVehicleDataWindows_btn.Focusable = true;
        }

        /// <summary>
        ///     create new random truck
        /// </summary>
        private void Truck_btn_OnClick(object sender, RoutedEventArgs e)
        {
            _truck = VehicleGenerator.TruckGenerator();
            showVehicleInfos(_truck);
            carBtnActive = false;
            moCycleBtnActive = false;
            truckBtnActive = true;
            clearVehicleDataWindows_btn.Focusable = true;
        }

        #endregion

        //todo: stop: debugging for different actions needed
    }
}