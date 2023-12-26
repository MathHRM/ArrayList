using System.Collections.Generic;
using System;

class Program
{
    static void Main(string[] args)
    {
        ArrayList<int> teste = new();

        for (var i = 0; i < 20; i++)
        {
            teste.Push(i);
        }
        teste.Print();

        for (var i = 0; i < teste.Length; i++)
        {
            teste.SetElement(i, i*2);
        }
        teste.Print();

        teste.Take();
        teste.Take();
        teste.Pop();
        teste.Take();
        teste.Push(0);
        teste.Add(0);
        teste.Print();
        Console.WriteLine(teste.Length);
    }
}