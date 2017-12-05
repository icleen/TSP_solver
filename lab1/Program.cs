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

class Algorithms
{
  public static int ModExp(int x, int y, int n)
  {
    if (y == 0) {
      return 1;
    }
    int z = ModExp(x, (y/2), n);
    if (y%2 == 0) {
      return (int)(Math.Pow(z, 2) % n);
    }else {
      return (int)((x * Math.Pow(z, 2)) % n);
    }
  }

  public static Tuple<bool, double> Primality(int n, int k=50)
  {
    if (n < k) {
      k = n/2;
    }
    double p = 1.0;
    bool prime = true;
    int a = 0, an = 0;
    Random rand = new Random();

    for (int i = 0; i < k; i++) {
      a = rand.Next(1, n);
      an = ModExp(a, (n-1), n);
      if (an == 1) {
        p *= 0.5;
      }else {
        prime = false;
        break;
      }
    }

    return Tuple.Create(prime, (1.0 - p));
  }

}
