using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class EditMatrix
{
    private const int Large = 10000;

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

    private const int Bound = 4; // the bandwith value

    private char **prev;
    private int **matrix;
    private int m, n;

    private string mString;
    private string nString;

    public EditMatrix(string mString, string nString)
    {
        this.m = mString.length() + 1;
        this.n = nString.length() + 1;
        this.mString = mString;
        this.nString = nString;
        matrix = new int[m, n];
		prev = new char[m, n];
		for (int i = 0; i < m; i++)
		{
			matrix[i][0] = i * InsertDelete;
			for (j = 1; j < n; j++)
			{
				matrix[i][j] = Large;
			}
			prev[i][0] = Top;
		}
		for (int j = 0; j < Bound; j++)
		{
			matrix[0][j] = j * InsertDelete;
			prev[0][j] = Left;
		}
		prev[0][0] = ' ';
    }

    public int min(int i, int j)
    {
        int d = matrix[i-1][j-1] + diff(i-1, j-1);
        int t = matrix[i-1][j] + InsertDelete;
        int l = matrix[i][j-1] + InsertDelete;
        if (t < l && t < d) {
            prev[i][j] = Top;
            return t;
        }else if (l < t && l < d) {
            prev[i][j] = Left;
            return l;
        }else {
            prev[i][j] = Diag;
            return d;
        }
    }

    public int diff(int i, int j)
    {
        if(mString[i] == nString[j]) {
            return Match;
        }
        return Substitute;
    }

    public void determine(int i, int j)
    {
        matrix[i][j] = min(i, j);
    }

    public void solve()
    {
        for (int i = 1; i < m; i++) {
            determine(i, i);
            for (int j = 1; j < Bound; j++) {
                if (i + j < m)
                    determine(i + j, i);
                if (i + j < n)
                    determine(i, i + j);
            }
        }
    }

    public string findPath()
    {
        StringBuilder ss;
        int i = m - 1;
        int j = n - 1;
        char cur = prev[i][j];
        while(cur != EndPoint)
        {
            ss.Append(cur);
            // cout << "cur: " << cur << "\n";
            switch (cur) {
                case Diag:
                    cur = prev[--i][--j];
                    break;
                case Top:
                    cur = prev[--i][j];
                    break;
                case Left:
                    cur = prev[i][--j];
                    break;
                default:
                    // cerr << "Error; prev matrix should not have this value: " << cur << endl;
                    cur = EndPoint;
            }
        }
        char[] ret = ss.ToString().ToCharArray();
        Array.Reverse(ret);
        return new string(ret);
    }

    public string interpretPath(string path)
    {
        StringBuilder mss;
        StringBuilder nss;
        char cur;
        int i = 0;
        int j = 0;
        for (int k = 0; k < path.length(); k++) {
            cur = path[k];
            // cout << "cur: " << cur << "\n";
            switch (cur) {
                case Diag:
                    mss.Append(mString[i++]);
                    nss.Append(nString[j++]);
                    break;
                case Top:
                    mss.Append(mString[i++]);
                    nss.Append('-');
                    break;
                case Left:
                    mss.Append('-');
                    nss.Append(nString[j++]);
                    break;
                default:
                    // cerr << "Error; prev matrix should not have this value: " << cur << endl;
                    k += 100;
            }
        }
        nss.Append("\n");
        nss.Append(mss.ToString());
        nss.Append("\n");
        return nss.ToString();
    }

    public string results()
    {
        solve();
        return interpretPath(findPath());
    }

    public string toString()
    {
        StringBuilder ss;
        ss.Append("matrix:\n");
        int j;
        for (int i = 0; i < m; i++) {
            for (j = 0; j < n; j++) {
                ss.Append(matrix[i][j]);
                ss.Append("\t");
            }
            ss.Append("\n");
        }
        ss.Append("prev:\n");
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                ss.Append(prev[i][j]);
                ss.Append("\t");
            }
            ss.Append("\n");
        }

        return ss.ToString();
    }

}
