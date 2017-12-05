using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS312_lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Solve_Click(object sender, EventArgs e)
        {
            int n = Convert.ToInt32(Input.Text);
            int k = 50;
            if (K.Text != "")
            {
                k = Convert.ToInt32(K.Text);
            }
            Tuple<bool, double> result = Primality(n, k); // O(n^4 * log n)
            if (result.Item1)
            {
                Output.Text = "yes with probability " + result.Item2;
            }
            else
            {
                Output.Text = "no";
            }
        }

        private int ModExp(int x, int y, int n)
        {
            if (y == 0)
            {
                return 1;
            }
            int z = ModExp(x, (y / 2), n); // O(log y/2) because it goes log y/2 (base 2) layers down
            // O(log y/2) + O(n^2) for the division
            if (y % 2 == 0)
            {
                return (int)(Math.Pow(z, 2) % n); // O(n^2) because it's just z*z which is a basic multiplication
            }
            else
            {
                return (int)((x * Math.Pow(z, 2)) % n); // O(2n^2) because there are 2 n^2 multiplications
            }
        } // O(n^2 * log n) because you have an O(n^2) at every level

        private Tuple<bool, double> Primality(int n, int k = 50)
        {
            if (k > n/2) { // O(2n^2) for 2 divisions; only runs when n is too low of a number
                k = n/2;
            }
            double p = 1.0;
            bool prime = true;
            Random rand = new Random();
            int a = rand.Next(1, n), an = 0;
            List<int> as = new List();

            for (int i = 0; i < k; i++) // O(n) because you do it a maximum of n/2 times
            {
                while(as.Contains(a)) { // O(n) because can run up to n times
                    a = rand.Next(1, n); // O(1) because generating a random number is linear
                }
                an = ModExp(a, (n - 1), n); // O(n^2 * log n) because it goes log n (base 2) layers down
                if (an == 1)
                {
                    p *= 0.5; // O(n^2) for multiplication
                    // I multiply the probability each time because each time you test
                    // primality the probability of it being a prime goes up by 50%.
                    // (ie. the probability of n being a prime is 1/(2^k) where k is the number of tests)
                    // At the end I subtract this number from 1 to get the correct probability
                }
                else
                {
                    prime = false;
                    break;
                }
            } // O(n^2 * n^2 * log n) = O(n^4 * log n)

            return Tuple.Create(prime, (1.0 - p));
        }

    }
}
