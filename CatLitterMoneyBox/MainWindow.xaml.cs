using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

using Newtonsoft.Json;

namespace CatLitterMoneyBox;

public partial class MainWindow: Window
    {
    #region statics

    public MainWindow()
        {
        InitializeComponent();
        DataContext = this; //This is a binder that says the mainwindow itself is source for all interface elements
        User_drpdwn.ItemsSource =
            BankAccounts; //This is the connection between the dropdown menu and my List!it is that easy!!!
        InitializeBankAccounts();
        Closing += MainWindow_Closing;
        }

    //Some static values so my code works per se
    public static bool CheckOne; //Checkbox checked
    public static bool CheckTwo; //Checkbox checked
    public static bool CheckLottery; //Checkbox checked
    public static bool UserCreation; // New user button pressed
    public static bool UserDeletion; // User deletion button pressed
    public static bool SonderZahlung; // Extra money,yeah
    public static bool LohnAnpassung; //We want more!We want more!
    private static List<BankAccount> BankAccounts; //Placeholder list for the database
    public static DateTime Today;
    private static readonly string path = ".\\BankAccounts.json"; //Path to database

    #endregion statics

    #region Checkboxes & Radios

    private void Kloeins_cb_Checked(object sender, RoutedEventArgs e)
        {
        CheckOne = CheckOne ? false : true;
        }

    private void Klozwei_cb_Checked(object sender, RoutedEventArgs e)
        {
        CheckTwo = CheckTwo ? false : true;
        }

    private void BonusLottery_cb_Checked(object sender, RoutedEventArgs e)
        {
        CheckLottery = CheckLottery ? false : true;
        }

    #endregion Checkboxes & Radios

    #region Bankaccounts

    private void InitializeBankAccounts() //Load and initialize the database list of Bankaccounts
        {
        if(File.Exists(path))
            BankAccounts = JsonConvert.DeserializeObject<List<BankAccount>>(File.ReadAllText(path));
        }

    public BankAccount SelectedAccount { get; set; } //getter from the dropdown menu

    private void UserAnlegen_btn_Click(object sender, RoutedEventArgs e) //New User creation
        {
        //visibility on
        UserCreationName_tbx.Visibility = 0;
        UserCreationName_lbl.Visibility = 0;
        UserManipulation_btn.Visibility = 0;
        UserVorgang_lbl.Content = "Erstellmodus";
        //Set flags for submit button
        UserCreation = true;
        UserDeletion = false;
        }

    private void UserLöschen_btn_Click(object sender, RoutedEventArgs e) //Userdeletion
        {
        //visibility on
        UserCreationName_tbx.Visibility = 0;
        UserCreationName_lbl.Visibility = 0;
        UserManipulation_btn.Visibility = 0;
        UserVorgang_lbl.Content = "Löschvorgang";
        //Set flags for submit button
        UserDeletion = true;
        UserCreation = false;
        }

    private void UserManipulation_btn_Click(object sender, RoutedEventArgs e) //process chosen order
        {
        if(UserCreation)
            {
            var userName = UserCreationName_tbx.Text;
            BankAccounts.Add(new BankAccount(userName, DateTime.Today, 0.10, 0));
            }

        if(UserDeletion)
            {
            UserCreationName_tbx.Focusable = false; //just read, dont touch
            UserCreationName_tbx.Text = SelectedAccount.Name; //Show name for ref
            BankAccounts.Remove(SelectedAccount);
            }

        //Visibility off
        UserCreationName_tbx.Visibility = (Visibility)1;
        UserCreationName_lbl.Visibility = (Visibility)1;
        UserManipulation_btn.Visibility = (Visibility)1;
        //Clear to status quo
        UserVorgang_lbl.Content = "";
        UserCreation = false;
        UserDeletion = false;
        }

    #endregion BankAccounts

    #region Money special buttons

    //Change the amount given for the job done
    private void LohnAnpassen_btn_Click(object sender, RoutedEventArgs e)
        {
        ValueChange_tbx.Focus();
        ValueChange_tbx.Text = SelectedAccount.Salary.ToString("C");
        //Flags for submit button
        LohnAnpassung = true;
        SonderZahlung = false;
        }

    //Adding Money
    private void SonderEinzahlung_btn_Click(object sender, RoutedEventArgs e)
        {
        ValueChange_tbx.Focus();
        //Flags for submit button
        SonderZahlung = true;
        LohnAnpassung = false;
        }

    private void ValueChange_btn_Click(object sender, RoutedEventArgs e)
        {
        if(LohnAnpassung)
            {
            double.TryParse(ValueChange_tbx.Text, out var result);
            SelectedAccount.Salary = result;
            }

        if(SonderZahlung)
            {
            double.TryParse(ValueChange_tbx.Text, out var result);
            SelectedAccount.Money += result;
            }

        //set Flags back to negative
        LohnAnpassung = false;
        SonderZahlung = false;
        }

    #endregion Money special buttons

    #region Money manipulations

    //Little fun lottery for extraordinary jobs done.Gets added
    private static double Lottery()
        {
        var wins = new[] { 0.5, 0.10, 0.5, 0.15, 0.10, 0.20, 0.15, 0.30 };
        var rand = new Random();
        return wins[rand.Next(8)];
        }

    private void JobBuchen_btn_Click(object sender, RoutedEventArgs e)
        {
        var salary = SelectedAccount.Salary;
        Today = Calendar_cal.SelectedDate.Value;
        double i = 0;
        if(CheckOne)
            i += salary;
        if(CheckTwo)
            i += salary;
        if(CheckLottery)
            {
            var lottery = Lottery();
            i += lottery;
            MessageBox.Show($"{lottery.ToString("C")} Euro gewonnen!!");
            }

        SelectedAccount.Money += i;
        SelectedAccount.Date = Today;
        MessageBox.Show("Geld erfolgreich eingezahlt!");
        }

    private void AbfrageGuthaben_btn_Click(object sender, RoutedEventArgs e)
        {
        MessageBox.Show(
            $"{SelectedAccount.Name} hat: {SelectedAccount.Money.ToString("0.00")} Euro. Stand: {SelectedAccount.Date.ToShortDateString()}.\n");
        }

    #endregion Money manipulations

    #region Abbuchungsvorgang

    //Inputbox only active if button pressed. For security lol
    private void AbhebenGuthaben_btn_Click(object sender, RoutedEventArgs e)
        {
        Abhebebox_tbx.IsEnabled = true;
        }

    //Moneywithdrawal if sufficient balance on account
    private void Abheben_btn_Click(object sender, RoutedEventArgs e)
        {
        Today = Calendar_cal.SelectedDate.Value;
        var abhebung = double.Parse(Abhebebox_tbx.Text);
        if(abhebung > SelectedAccount.Money)
            {
            MessageBox.Show($"Soviel ist nicht vorhanden!\n" +
                            $"{SelectedAccount.Name} hat\n" +
                            $"{SelectedAccount.Money.ToString("0.00")} Euro\n" +
                            $"auf dem Konto.");
            Abhebebox_tbx.Text = SelectedAccount.Money.ToString("0.00");
            }
        else
            {
            SelectedAccount.Money -= abhebung;
            MessageBox.Show($"{abhebung.ToString("0.00")}" +
                            $" Euro erfolgreich abgebucht!\n" +
                            $"Es verbleiben {SelectedAccount.Money.ToString("0.00")} Euro\n" +
                            $" auf dem Konto von {SelectedAccount.Name}.");
            SelectedAccount.Date = Today;
            }
        }

    #endregion Abbuchungsvorgang

    #region Clean off before close

    //Safe on close statement
    private void SafeBankAccounts()
        {
        File.WriteAllText(path, JsonConvert.SerializeObject(BankAccounts));
        }

    #region Safe on close statement alternative

    /*  private void SaveDataToCSV()
      {
          // Assuming you have a list of BankAccounts called "BankAccounts"
          string filePath = "data.csv"; // Specify the file path for your CSV file

          using(StreamWriter writer = new StreamWriter(filePath))
          {
              // Write the header
              writer.WriteLine("Account Number,Name,Balance"); // Adjust the header based on your BankAccount properties

              // Write each BankAccount data
              foreach(BankAccount account in BankAccounts)
              {
                  string line = $"{account.AccountNumber},{account.Name},{account.Balance}"; // Adjust the properties based on your BankAccount class
                  writer.WriteLine(line);
              }
          }
      }*/

    #endregion Safe on close statement alternative

    private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
        // Perform your save operation here
        // SaveDataToCSV();
        }

    #endregion
    }