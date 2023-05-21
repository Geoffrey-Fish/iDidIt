using System;
using System.IO;
using System.Windows;

using Newtonsoft.Json;

namespace CatLitterMoneyBox;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow: Window
    {
    #region statics
    public MainWindow()
        {
        InitializeComponent();
        }

    //Some static values so my code works per se
    public static bool CheckOne; //Checkbox checked
    public static bool CheckTwo; //Checkbox checked
    public static bool PauliDidIt; //Radiobutton checked
    public static bool FeliDidIt; //Radiobutton checked
    public static double BufferSum; //unneccesary for calculation, only for my brain
    public static DateTime Today;
    public static string pathF = @"D:\GitHub\iDidIt\CatLitterMoneyBox\feliBank.json";
    public static string pathP = @"D:\GitHub\iDidIt\CatLitterMoneyBox\pauliBank.json";
    #endregion

    #region Checkboxes & Radios
    private void Kloeins_cb_Checked(object sender, RoutedEventArgs e)
        {
        CheckOne = CheckOne ? false : true;
        }
    private void Klozwei_cb_Checked(object sender, RoutedEventArgs e)
        {
        CheckTwo = CheckTwo ? false : true;
        }
    private void Feli_radbt_Checked(object sender, RoutedEventArgs e)
        {
        FeliDidIt = FeliDidIt ? false : true;
        }
    private void Pauli_radbt_Checked(object sender, RoutedEventArgs e)
        {
        PauliDidIt = PauliDidIt ? false : true;
        }
    #endregion

    //Get Account data and serve it.If never run before, generate it
    //With Newtons Json converted into bits
    public static BankAccount GetFeliAccount()
        {
        if(File.Exists(pathF))
            {
            BankAccount fe = JsonConvert.DeserializeObject<BankAccount>(File.ReadAllText(pathF));
            return fe;
            }
        else
            {
            return new BankAccount("Feli", DateTime.Now, 0.00);
            }
        }
    public static BankAccount GetPauliAccount()
        {
        if(File.Exists(pathP))
            {
            BankAccount pa = JsonConvert.DeserializeObject<BankAccount>(File.ReadAllText(pathP));
            return pa;
            }
        else
            {
            return new BankAccount("Pauli", DateTime.Now, 0.0);
            }
        }


    //Adding money when Berechnen pressed with read and write
    private void Berechnen_btn_Click(object sender, RoutedEventArgs e)
        {
        Today = Calendar_cal.SelectedDate.Value;
        double i = 0;
        if(CheckOne)
            i += 0.10;
        if(CheckTwo)
            i += 0.10;
        BufferSum += i;
        BankAccount bankbuffer;
        if(FeliDidIt)
            {
            bankbuffer = GetFeliAccount();
            bankbuffer.Money += BufferSum;
            bankbuffer.Date = Today;
            File.WriteAllText(pathF, JsonConvert.SerializeObject(bankbuffer));
            }
        if(PauliDidIt)
            {
            bankbuffer = GetPauliAccount();
            bankbuffer.Money += BufferSum;
            bankbuffer.Date = Today;
            File.WriteAllText(pathP, JsonConvert.SerializeObject(bankbuffer));
            }
        MessageBox.Show("Geld erfolgreich verbucht!");
        }


    //Ask for current balance of all accounts.
    //Could be changed to only show account of
    //radiobutton selected user.
    private void AbfrageGuthaben_btn_Click(object sender, RoutedEventArgs e)
        {
        BankAccount pauli = GetPauliAccount();
        BankAccount feli = GetFeliAccount();
        MessageBox.Show($"Feli hat: {feli.Money.ToString()} Euro. Stand: {feli.Date}.\n" +
            $"Pauli hat: {pauli.Money.ToString()} Euro. Stand: {pauli.Date}.");
        }

    #region Abbuchungsvorgang
    //Inputbox only active if button pressed. For security lol
    private void AbhebenGuthaben_btn_Click(object sender, RoutedEventArgs e)
        {
        Abhebebox_tb.IsEnabled = true;
        }
    //Moneywithdrawal if sufficient balance on account
    private void Buchen_btn_Click(object sender, RoutedEventArgs e)
        {
        Today = Calendar_cal.SelectedDate.Value;
        BankAccount bufferAccount = FeliDidIt ? GetFeliAccount() : GetPauliAccount();
        string path = FeliDidIt ? pathF : pathP;
        double abhebung = double.Parse(Abhebebox_tb.Text);
        if(abhebung > bufferAccount.Money)
            {
            MessageBox.Show($"Soviel ist nicht vorhanden!\n" +
                $"{bufferAccount.Name} hat\n" +
                $"{bufferAccount.Money} Euro\n" +
                $"auf dem Konto.");
            Abhebebox_tb.Text = bufferAccount.Money.ToString();
            }
        else
            {
            bufferAccount.Money -= abhebung;
            MessageBox.Show($"{abhebung} Euro erfolgreich abgebucht!\n" +
                $"Es verbleiben {bufferAccount.Money} Euro\n" +
                $" auf dem Konto von {bufferAccount.Name}.");
            bufferAccount.Date = Today;
            File.WriteAllText(path, JsonConvert.SerializeObject(bufferAccount));
            }
        }
    }
#endregion


public class BankAccount
    {
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public double Money { get; set; }

    public BankAccount() { }
    public BankAccount(string name, DateTime date, double money)
        {
        this.Name = name;
        this.Date = date;
        this.Money = money;
        }
    }



/* schau dir das nochmal in ruhe an
/// <summary>
/// Writes the given object instance to a binary file.
/// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
/// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute;
/// cannot be applied to properties.</para>
/// </summary>
/// <typeparam name="T">The type of object being written to the binary file.</typeparam>
/// <param name="filePath">The file path to write the object instance to.</param>
/// <param name="objectToWrite">The object instance to write to the binary file.</param>
/// <param name="append">If false the file will be overwritten if it already exists.
/// If true the contents will be appended to the file.</param>
public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
{
using(Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
    {
    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
    binaryFormatter.Serialize(stream, objectToWrite);
    }
}

/// <summary>
/// Reads an object instance from a binary file.
/// </summary>
/// <typeparam name="T">The type of object to read from the binary file.</typeparam>
/// <param name="filePath">The file path to read the object instance from.</param>
/// <returns>Returns a new instance of the object read from the binary file.</returns>
public static BankAccount ReadFromBinaryFile<BankAccount>(string filePath)
{
using(Stream stream = File.Open(filePath, FileMode.Open))
    {
    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
    return (BankAccount)binaryFormatter.Deserialize(stream);
    }
}
}

*/

