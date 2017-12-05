using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// Space complexity of this class is O(2mn) since there are two matrices,
// prev and matrix, that are kept and used to solve the problem.  
class EditDistance
{
    private const int Large = 10000;
    private const int Bound = 4; // the bandwith value

    private const char Diag = 'd';
    private const char Top = 't';
    private const char Left = 'l';
    private const char EndPoint = ' ';

    // Normal weights
    // private const int InsertDelete = 1;
    // private const int Match = 0;
    // private const int Substitute = 1;

    // Needleman/Wunsch weights
    private const int InsertDelete = 5;
    private const int Match = -3;
    private const int Substitute = 1;

    private char[,] prev;
    private int[,] matrix;
    private int m, n;

    private string mString;
    private string nString;

    public EditDistance(string mString, string nString)
    {
        this.m = mString.Count() + 1;
        this.n = nString.Count() + 1;
        this.mString = mString;
        this.nString = nString;
        matrix = new int[m,n];
        prev = new char[m,n];
    }

    // Sets up the matrix for the unbanded algorithm.
    // It sets the values along the sides of the matrix.
    // O(2n) time complexity.
    public void setupUnbanded()
    {
        for (int i = 0; i < m; i++)
        {
            matrix[i,0] = i * InsertDelete;
            prev[i,0] = Top;
        }
        for (int j = 0; j < n; j++)
        {
            matrix[0,j] = j * InsertDelete;
            prev[0,j] = Left;
        }
        prev[0,0] = ' ';
    }

    // Sets up the matrix for the banded algorithm.  It goes through each row
    // and sets the numbers on either side of the bandwidth to be Large (10000).
    // This forces the min function to choose a value from within the bandwidth
    // O(m + Bound) complexity (Bound is 4 in this case, so it's really just O(m))
    public void setupBanded()
    {
        for (int i = 0; i < m; i++)
        {
            if (i - Bound >= 0)
                matrix[i,i - Bound] = Large;
            if (i + Bound < n)
                matrix[i,i + Bound] = Large;
        }
        for (int j = 0; j < Bound; j++)
        {
            matrix[j, 0] = j * InsertDelete;
            prev[j, 0] = Top;
            matrix[0,j] = j * InsertDelete;
            prev[0,j] = Left;
        }
        prev[0,0] = ' ';
    }

    // O(1) time complexity; it takes constant time to check three values
    private int min(int i, int j)
    {
        int t = matrix[i-1,j] + InsertDelete;
        int l = matrix[i,j-1] + InsertDelete;
        int d = matrix[i-1,j-1] + diff(i-1, j-1);
        if (d <= l && d <= t) {
            prev[i, j] = Diag;
            return d;
        }else if (t < l && t < d) {
            prev[i, j] = Top;
            return t;
        }else {
            prev[i, j] = Left;
            return l;
        }
    }
    // O(1) time complexity to check char values
    private int diff(int i, int j)
    {
        if(mString[i] == nString[j]) {
            return Match;
        }
        return Substitute;
    }
    // O(1) time complexity since min() is O(1)
    private void determine(int i, int j)
    {
        matrix[i,j] = min(i, j);
    }

    // This is the unbanded solve and has to go through each value in the matrix.
    // O(mn) time complexity to go through each value.
    public void solve()
    {
        for (int i = 1; i < m; i++) {
            for (int j = 1; j < n; j++) {
                determine(i, j);
            }
        }
    }

    // This is the banded solve and has to go through each value within
    // the band of the  matrix.
    // O(m + n) time complexity; The outer loop is O(m) and
    // the inner loop is only O(Bound) = O(4), so it's constant time.
    // It becomes O(m + n) because overall you will have to visit each value
    // in m and each value in n once.
    public void bandedSolve()
    {
        for (int i = 1; i < m; i++)
        {
            determine(i, i);
            for (int j = 1; j < Bound; j++)
            {
                if (i + j < m)
                    determine(i + j, i);
                if (i + j < n)
                    determine(i, i + j);
            }
        }
    }

    // This has to go along the previous matrix that holds the path
    // given while running the algorithm.  In the worst case it has
    // O(m + n) time complexity since
    // it will have to traverse acrossthe length of each side, m and n,
    // and thus visit each letter (this would only happen if it was all
    // deletions and inserts).  Realistically this is much better through.
    // O(m + n) space complexity since it has to store the path it goes through.
    private string findPath()
    {
        StringBuilder ss = new StringBuilder();
        int i = m - 1;
        int j = n - 1;
        char cur = prev[i,j];
        while (cur != EndPoint && i >= 0 && j >= 0)
        {
            ss.Append(cur);
            if (cur == Diag)
            {
                cur = prev[--i, --j];
            }else if (cur == Top)
            {
                cur = prev[--i, j];
            }else if (cur == Left)
            {
                cur = prev[i, --j];
            }else
            {
                Console.Write("cur is unexpected: ");
                Console.WriteLine(cur);
            }
        }
        char[] ret = ss.ToString().ToCharArray();
        Array.Reverse(ret);
        return new string(ret);
    }

    // Worst case O(m + n) time and space complexity as that is the worst case
    // path found in the algorithm above.
    private string[] interpretPath(string path)
    {
        StringBuilder mss = new StringBuilder();
        StringBuilder nss = new StringBuilder();
        char cur;
        int i = 0;
        int j = 0;
        for (int k = 0; k < path.Count(); k++)
        {
            cur = path[k];
            if (cur == Diag)
            {
                mss.Append(mString[i++]);
                nss.Append(nString[j++]);
            }
            else if (cur == Top)
            {
                mss.Append(mString[i++]);
                nss.Append('-');
            }
            else if (cur == Left)
            {
                mss.Append('-');
                nss.Append(nString[j++]);
            }
            else
            {
                Console.Write("cur is unexpected: ");
                Console.WriteLine(cur);
            }
        }
        string[] bob = { nss.ToString(), mss.ToString() };
        return bob;
    }

    // O(mn + m + n) ~= O(mn) to go through the matrix and get the path
    // using the functions above.
    public string[] results()
    {
        solve();
        return interpretPath(findPath());
    }

    // O(2m + 2n) ~= O(m + n) to go through the matrix and get the path
    // using the functions above.
    public string[] bandedResults()
    {
        bandedSolve();
        return interpretPath(findPath());
    }

    // O(1) to return the bottom right value which is the cost of edit distance.
    public int value()
    {
        return matrix[m - 1, n - 1];
    }

    public string toString()
    {
        StringBuilder ss = new StringBuilder();
        ss.Append("\nmatrix:\n");
        int i, j;
        int a = m, b = n;
        if (m > 10)
        {
            a = 10;
        }
        if (n > 10)
        {
            b = 10;
        }
        for (i = 0; i < a; i++)
        {
            for (j = 0; j < b; j++)
            {
                if (matrix[i, j] == Large)
                {
                    ss.Append("--\t");
                }else
                {
                    ss.Append(matrix[i, j]);
                    ss.Append("\t");
                }
            }
            ss.Append("\n");
        }
        ss.Append("prev:\n");
        for (i = 0; i < a; i++)
        {
            for (j = 0; j < b; j++)
            {
                if (prev[i,j] != Diag && prev[i,j] != Top && prev[i,j] != Left)
                {
                    ss.Append("--\t");
                }else
                {
                    ss.Append(prev[i, j]);
                    ss.Append("\t");
                }
            }
            ss.Append("\n");
        }

        return ss.ToString();
    }

}
