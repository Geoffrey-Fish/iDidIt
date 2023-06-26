using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using ParkHouseV2.Models;

namespace ParkHouseV2.ViewModel;

public class MainViewModel: INotifyPropertyChanged
	{
	public event PropertyChangedEventHandler? PropertyChanged;

	#region Lists

	private ObservableCollection<string> _licencePlates { get; set; }

	public ObservableCollection<string> LicencePlates
		{
		get => _licencePlates;
		set
			{
			_licencePlates = value;
			OnPropertyChanged(nameof(LicencePlates));
			}
		}

	private ObservableCollection<string> _models { get; set; }

	public ObservableCollection<string> Models
		{
		get => _models;
		set
			{
			_models = value;
			OnPropertyChanged(nameof(Models));
			}
		}

	private ObservableCollection<int> _usedLotNumbers { get; set; }

	public ObservableCollection<int> UsedLotNumbers
		{
		get => _usedLotNumbers;
		set
			{
			_usedLotNumbers = value;
			OnPropertyChanged(nameof(UsedLotNumbers));
			}
		}

	#endregion

	public MainViewModel()
		{
		_licencePlates = new ObservableCollection<string>();
		LicencePlates = new ObservableCollection<string>();
		_models = new ObservableCollection<string>();
		Models = new ObservableCollection<string>();
		_usedLotNumbers = new ObservableCollection<int>();
		UsedLotNumbers = new ObservableCollection<int>();
		}


	/// <summary>
	/// Whenever something changed, it gets updated here.
	/// </summary>
	/// <param name="parkhouse"></param>
	public void UpdateListsFromParkhouse(ParkHouse parkhouse)
		{
		LicencePlates.Clear();
		Models.Clear();
		UsedLotNumbers.Clear();

		foreach(var licencePlate in parkhouse.LicencePlates)
			LicencePlates.Add(licencePlate);

		foreach(var model in parkhouse.Models)
			Models.Add(model);

		foreach(var lot in parkhouse.UsedLotNumbers)
			UsedLotNumbers.Add(lot);
		}


	protected virtual void OnPropertyChanged(
		[CallerMemberName] string? propertyName = null)
		{
		PropertyChanged?.Invoke(this,
			new PropertyChangedEventArgs(propertyName));
		}

	protected bool SetField<T>(ref T field, T value,
							   [CallerMemberName] string? propertyName = null)
		{
		if(EqualityComparer<T>.Default.Equals(field, value))
			return false;
		field = value;
		OnPropertyChanged(propertyName);
		return true;
		}
	}