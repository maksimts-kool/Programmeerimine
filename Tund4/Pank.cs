using System;

namespace Tund4;

public interface ITooline
{
    double ArvutaPalk();
}
public class Pank
{
    private double saldo;
    public double Saldo
    {
        get { return saldo; }
        set
        {
            if (value >= 0)
                saldo = value;
        }
    }
    public void LisaRaha(double summa)
    {
        if (summa > 0)
            saldo += summa;
    }
    public void VotaRaha(double summa)
    {
        if (summa > 0 && summa <= saldo)
        {
            saldo -= summa;
        }
        else
        {
            Console.WriteLine("Ei ole raha");
        }
        Console.WriteLine($"Teie kontol on {saldo} eurot");
    }
}
