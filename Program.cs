class Program
{
    static void Main(string[] args)
    {
        ArrayList<string> l = new ArrayList<string>();

        l.Push("abc");
        l.Push("1");
        l.Push("true");
        l.Push("2");
        l.Push("xyz");
        l.Push("false");
        l.Push("a");
        l.Push("b");
        l.Push("b");
        l.Push("ce23");
        l.Push("ce1");
        l.Push("cez");
        l.Push("cea");

        l.Print();

        Console.WriteLine(l[0]);
        Console.WriteLine(l[2]);
        Console.WriteLine(l[4]);
        l[2] = "false";
        Console.WriteLine(l[0] = "123");
        Console.WriteLine(l[2]);
        

        l.Print();

        l.Sort();
        l.Print();
        l.Pop();
        l.Pop();
        l.Pop();

        l.Print();

        Console.WriteLine(l.FindElement("cez"));
        Console.WriteLine(l.FindElement("1x"));
    }
}