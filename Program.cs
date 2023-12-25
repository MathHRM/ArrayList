using System.Collections.Generic;
using System;

class Program
{
    static void Main(string[] args)
    {
        ArrayList<int> teste = new();

        for (var i = 1; i < 31; i++)
        {
            teste.PushElement(i);
        }

        teste.Print();
        Console.WriteLine($"Array Length: {teste.Length}");

        Console.WriteLine();
        for (var i = 0; i < 20; i++)
        {
            teste.RemoveElement(10);
        }

        teste.Print();
        Console.WriteLine($"Array Length: {teste.Length}");
    }
}