using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Input a number to check if it's prime: ");
        string input = Console.ReadLine();
        int test_num = Int32.Parse(input);
        Tuple<bool, double> result = Algorithms.Primality(test_num);
        if(result.Item1) {
          Console.Write("Yes, with " + result.Item2 * 100 + "% accuracy.\n");
        }else {
          Console.Write("No.\n");
        }
    }
}
