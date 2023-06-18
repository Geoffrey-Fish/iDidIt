using System;

namespace CatLitterMoneyBox;

public class BankAccount
    {

    public string Name { get; set; }
    public DateTime Date { get; set; }
    public double Salary { get; set; }
    public double Money { get; set; }
    public BankAccount()
        {
        }

    public BankAccount(string name, DateTime date, double salary, double money)
        {
        Name = name;
        Date = date;
        Salary = salary;
        Money = money;
        }


    }