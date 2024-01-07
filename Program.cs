using System.Collections.Generic;
using System;

class Program
{
    static void Main(string[] args)
    {
        ArrayList<string> teste = new();
        teste.Push("palavra1");
        teste.Push("a");
        teste.Push("b");

        teste.Print();

        teste[1] = "naolavra";

        teste.Print();

        foreach(string palavra in teste){
            Console.Write(palavra);
        }
    }
}