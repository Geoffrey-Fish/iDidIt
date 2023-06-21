using System.Windows;
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
        public MainWindow()
        {
            InitializeComponent();
        }
        // static objects
        private ParkHouse _parkHouse;
        private Car _car;
        private Motorcycle _mocycle;
        private Truck _truck;
        
        // bool failsafes
        private static bool carBtnActive = false;
        private static bool moCycleBtnActive = false;
        private static bool truckBtnActive =false;
        
        private void CreatePhouse_btn_OnClick(object sender, RoutedEventArgs e)
        {
            // todo: setting for money cost
            if (lotAmount_tbx.Text != null)
            {
                bool bla = int.TryParse(lotAmount_tbx.Text, out int lots);
                _parkHouse = new ParkHouse(lots);
                okLight_btn.Background = _parkHouse.OkLight ? Brushes.Green : Brushes.Gray;
                resultText_tbx.Text = _parkHouse.OkMessage;
            }
            // todo: errormessage if not a number else if
            else
            {
                errorLight_btn.Background = _parkHouse.ErrorLight ? Brushes.Red : Brushes.Gray;
                errorText_tbx.Text = "Error, please enter Lot amount!";
            }
        }

        private void LoadLastPh_btn_OnClick(object sender, RoutedEventArgs e)
        {
            _parkHouse = ParkHouse.LoadParkHouse();
            if (_parkHouse != null)
            {
                resultText_tbx.Text = "Last Parkhouse successfull loaded";
            }
            else
            {
                errorText_tbx.Text = "No Parkhouse found.Please create a new instance.";
            }
        }
/// <summary>
/// create new random car
/// </summary>
        private void Car_btn_OnClick(object sender, RoutedEventArgs e)
        {
            _car =  VehicleGenerator.CarGenerator();
            carBtnActive = true;
            moCycleBtnActive = false;
            truckBtnActive = false;

        }
/// <summary>
/// create new random bike
/// </summary>
        private void MoCy_btn_OnClick(object sender, RoutedEventArgs e)
        {
             _mocycle = VehicleGenerator.MotorcycleGenerator();
            carBtnActive = false;
            moCycleBtnActive = true;
            truckBtnActive = false;
        }

/// <summary>
/// create new random truck
/// </summary>
        private void Truck_btn_OnClick(object sender, RoutedEventArgs e)
        {
            _truck = VehicleGenerator.TruckGenerator();
            carBtnActive = false;
            moCycleBtnActive = false;
            truckBtnActive = true;
        }

        private void ParkIn_btn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void DriveOut_btn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
        // todo: autocompletion by typing and or Combobox instead of textbox
        private void SearchLicence_btn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        // todo: autocompletion by typing and or Combobox instead of textbox
        private void SearchType_btn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        // todo: autocompletion by typing and or Combobox instead of textbox
        private void SearchLot_btn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
// todo: resetbutton active if fields not empty check
        private void ResultReset_btn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

// todo: resetbutton active if fields not empty check
        private void ErrorReset_btn_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
/// <summary>
/// Reset the textboxes and bools and all
/// </summary>

        private void ClearVehicleDataWindows_btn_OnClick(object sender, RoutedEventArgs e) //for the data in bottomleft
        {
            carBtnActive = false;
            moCycleBtnActive = false;
            truckBtnActive = true;
            okLight_btn.Background=Brushes.Gray;
            resultText_tbx.Text = null;
            errorLight_btn.Background = Brushes.Gray;
            errorText_tbx.Text = null;
        }

            //todo: labels for the current lot count free and used -->
    }
}