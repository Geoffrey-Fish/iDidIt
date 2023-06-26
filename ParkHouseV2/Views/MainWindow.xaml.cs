using System;
using System.Windows;
using System.Windows.Media;

using ParkHouseV2.Models;
using ParkHouseV2.ViewModel;

using Vehicles;
using Vehicles.Land;

namespace ParkHouseV2;

public partial class MainWindow
	{
	#region Statics

	// bool failsafes what kind of vehicle is currently generated
	private static bool carBtnActive;
	private static bool moCycleBtnActive;

	private static bool truckBtnActive;

	//Instances of the vehicles
	private Car _car;
	private Motorcycle _mocycle;

	private Truck _truck;

	// The Queen Bitch of the town
	public ParkHouse parkHouse; //Main Instance Code logic
	public MainViewModel viewModel;

	#endregion

	#region initialization

	public MainWindow()
		{
		InitializeComponent();
		//Binding
		viewModel = new MainViewModel();
		DataContext = viewModel;
		}

	#endregion

	#region Create & Load Parkhouse

	private void CreatePhouse_btn_OnClick(object sender, RoutedEventArgs e)
		{
		// todo: setting for money cost
		if(lotAmount_tbx.Text != null) //Is Value entered
			{
			var bla = int.TryParse(lotAmount_tbx.Text,
				out var lots); //Check if user dumb
			if(!bla)
				{
				errorText_tbx.Text
					= "Error, please just integers"; //scold user.Bad user!
				return;
				}

			parkHouse
				= new ParkHouse(
					lots); //Ok, new instance with defined amount of lots
			if(parkHouse.OkLight) //All is cool? Cool
				{
				okLight_btn.Background = Brushes.Green;
				resultText_tbx.Text = parkHouse.OkMessage;
				}
			else if(parkHouse.ErrorLight) //Oh nose
				{
				errorLight_btn.Background = Brushes.Red;
				}
			}
		else //User dumb
			{
			errorLight_btn.Background = Brushes.Red;
			errorText_tbx.Text = "Error, please enter Lot amount!";
			}


		//Clear up
		createPhouse_btn.Focusable = false; //save button from idots
		loadLastPh_btn.Focusable = false; //same
		lotAmount_tbx.Text = null;  //same
		}

	/// <summary>
	///     Load last configured Parkhouse
	/// </summary>
	private void LoadLastPh_btn_OnClick(object sender, RoutedEventArgs e)
		{
		parkHouse = ParkHouse.LoadParkHouse();
		if(parkHouse !=
			null) //Will check if null is returned. if so, there aint no file, brotha
			{
			okLight_btn.Background = Brushes.Green;
			resultText_tbx.Text = "Last Parkhouse successfull loaded";
			}
		else
			{
			errorLight_btn.Background = Brushes.Red;
			errorText_tbx.Text
				= "No Parkhouse found.Please create a new instance.";
			}

		//Clear up
		createPhouse_btn.Focusable = false; //save button from idots
		loadLastPh_btn.Focusable = false; //same
		lotAmount_tbx.Text = null;  //same
		}

	#endregion


	#region In and out buttons

	/// <summary>
	///     Park in Call, check which type is currently chosen via BtnActive
	/// </summary>
	private void ParkIn_btn_OnClick(object sender, RoutedEventArgs e)
		{
		if(carBtnActive)
			{
			parkHouse.Parking(_car);
			}
		else if(moCycleBtnActive)
			{
			parkHouse.Parking(_mocycle);
			}
		else if(truckBtnActive)
			{
			parkHouse.Parking(_truck);
			}
		else
			{
			errorLight_btn.Background = Brushes.Red;
			errorText_tbx.Text = "You need to choose a vehicle first";
			}

		if(parkHouse.OkLight)
			{
			okLight_btn.Background = Brushes.Green;
			resultText_tbx.Text = parkHouse.OkMessage;
			UpdateLotCounters(); //todo: needs to be in viewmodel also
			viewModel.UpdateListsFromParkhouse(parkHouse);
			}
		else if(parkHouse.ErrorLight)
			{
			errorLight_btn.Background = Brushes.Red;
			errorText_tbx.Text = parkHouse.ErrorMessage;
			}
		}

	private void DriveOut_btn_OnClick(object sender, RoutedEventArgs e)
		{
		if(carBtnActive)
			{
			parkHouse.Leaving(_car);
			okLight_btn.Background = Brushes.Green;
			resultText_tbx.Text = parkHouse.OkMessage;
			}
		else if(moCycleBtnActive)
			{
			parkHouse.Leaving(_mocycle);
			okLight_btn.Background = Brushes.Green;
			resultText_tbx.Text = parkHouse.OkMessage;
			}
		else if(truckBtnActive)
			{
			parkHouse.Leaving(_truck);
			okLight_btn.Background = Brushes.Green;
			resultText_tbx.Text = parkHouse.OkMessage;
			}

		viewModel.UpdateListsFromParkhouse(parkHouse);
		UpdateLotCounters();
		}

	#endregion

	#region SearchButtons

	private void SearchLicence_btn_OnClick(object sender, RoutedEventArgs e)
		{
		foreach(var vehicle in parkHouse.parkingLots)
			if(vehicle is Car car)
				{
				if(car.LicencePlate.ToString() == searchLicence_cobx.Text)
					showVehicleInfos(car);
				return;
				}
			else if(vehicle is Motorcycle mo)
				{
				if(mo.LicencePlate.ToString() == searchLicence_cobx.Text)
					showVehicleInfos(mo);
				return;
				}
			else if(vehicle is Truck truck)
				{
				if(truck.LicencePlate.ToString() == searchLicence_cobx.Text)
					showVehicleInfos(truck);
				return;
				}
		}

	private void SearchType_btn_OnClick(object sender, RoutedEventArgs e)
		{
		foreach(var vehicle in parkHouse.parkingLots)
			if(vehicle is Car car)
				{
				if(car.Type.ToString() == searchType_cobx.Text)
					showVehicleInfos(car);
				}
			else if(vehicle is Motorcycle mo)
				{
				if(mo.Type.ToString() == searchType_cobx.Text)
					showVehicleInfos(mo);
				}
			else if(vehicle is Truck truck)
				{
				if(truck.Type.ToString() == searchType_cobx.Text)
					showVehicleInfos(truck);
				}
		}

	private void SearchLot_btn_OnClick(object sender, RoutedEventArgs e)
		{
		var vehicle
			= parkHouse.parkingLots[Convert.ToInt32(searchLot_cobx.Text)];
			{
			if(vehicle is Car car)
				{
				showVehicleInfos(car);
				return;
				}
			else if(vehicle is Motorcycle mo)
				{
				showVehicleInfos(mo);
				return;
				}
			else if(vehicle is Truck truck)
				{
				showVehicleInfos(truck);
				return;
				}
			}
		}

	#endregion

	#region Vehicle buttons

	/// <summary>
	///     create new random car
	/// </summary>
	private void Car_btn_OnClick(object sender, RoutedEventArgs e)
		{
		ClearVehicleDataWindows_btn_OnClick(this, e);
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
		ClearVehicleDataWindows_btn_OnClick(this, e);
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
		ClearVehicleDataWindows_btn_OnClick(this, e);
		_truck = VehicleGenerator.TruckGenerator();
		showVehicleInfos(_truck);
		carBtnActive = false;
		moCycleBtnActive = false;
		truckBtnActive = true;
		clearVehicleDataWindows_btn.Focusable = true;
		}

	#endregion

	#region Clear All & miscallenous

	/// <summary>
	///     Reset the textboxes and bools and all
	/// </summary>
	private void ClearVehicleDataWindows_btn_OnClick(
		object sender, RoutedEventArgs e) //for the data in bottomleft
		{
		carBtnActive = false;
		moCycleBtnActive = false;
		truckBtnActive = false;
		//
		okLight_btn.Background = Brushes.Gray;
		resultText_tbx.Text = null;
		errorLight_btn.Background = Brushes.Gray;
		errorText_tbx.Text = null;
		//
		name_tbx.Text = null;
		type_tbx.Text = null;
		licence_tbx.Text = null;
		model_tbx.Text = null;
		//
		clearVehicleDataWindows_btn.Focusable = false; //inactive button again
		}

	/// <summary>
	/// This fills the infoboxes with data of current chosen vehicle, wether by generating or search result.
	/// </summary>
	/// <param name="vec"></param>
	private void showVehicleInfos(Vehicle vec)
		{
		name_tbx.Text = vec.Name;
		if(vec is Car car)
			{
			licence_tbx.Text = car.LicencePlate;
			type_tbx.Text = "Car";
			model_tbx.Text = car.Type;
			}
		else if(vec is Motorcycle mo)
			{
			licence_tbx.Text = mo.LicencePlate;
			type_tbx.Text = "MotorBike";
			model_tbx.Text = mo.Type;
			}
		else if(vec is Truck truck)
			{
			licence_tbx.Text = truck.LicencePlate;
			type_tbx.Text = "Truck";
			model_tbx.Text = truck.Type;
			}
		}

	/// <summary>
	/// Little 
	/// </summary>
	private void UpdateLotCounters()
		{
		usedTicker_tbx.Text = parkHouse.UsedLots.ToString();
		freeTicker_tbx.Text = parkHouse.FreeLots.ToString();
		}

	#endregion

	#region TestArea

	private void Testing_btn_OnClick(object sender, RoutedEventArgs e)
		{
		errorText_tbx.Text = parkHouse.ShowTime();
		}

	#endregion
	}