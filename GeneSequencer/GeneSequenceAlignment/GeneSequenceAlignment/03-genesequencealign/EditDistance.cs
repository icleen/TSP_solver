using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class EditDistance
{
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
        this->m = mString.length() + 1;
        this->n = nString.length() + 1;
        this->mString = mString;
        this->nString = nString;
        matrix = new int[m,n];
        prev = new char[m,n];
        for(int i = 0; i < m; i++) {
            matrix[i][0] = i  * InsertDelete;
            prev[i][0] = Top;
        }
        for(int j = 0; j < n; j++) {
            matrix[0][j] = j * InsertDelete;
            prev[0][j] = Left;
        }
        prev[0][0] = ' ';
    }

    public int min(int i, int j)
    {
        int t = matrix[i-1][j] + InsertDelete;
        int l = matrix[i][j-1] + InsertDelete;
        int d = matrix[i-1][j-1] + diff(i-1, j-1);
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
            for (int j = 1; j < n; j++) {
                determine(i, j);
            }
        }
    }

    public string findPath()
    {
        stringstream ss;
        int i = m - 1;
        int j = n - 1;
        char cur = prev[i][j];
        while(cur != EndPoint)
        {
            ss << cur;
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
                    cerr << "Error; prev matrix should not have this value: " << cur << endl;
                    cur = EndPoint;
            }
        }
        string ret = ss.str();
        reverse(ret.begin(), ret.end());
        return ret;
    }

    public string interpretPath(string path)
    {
        stringstream mss;
        stringstream nss;
        char cur;
        int i = 0;
        int j = 0;
        for (int k = 0; k < path.length(); k++) {
            cur = path[k];
            // cout << "cur: " << cur << "\n";
            switch (cur) {
                case Diag:
                    mss << mString[i++];
                    nss << nString[j++];
                    break;
                case Top:
                    mss << mString[i++];
                    nss << '-';
                    break;
                case Left:
                    mss << '-';
                    nss << nString[j++];
                    break;
                default:
                    cerr << "Error; prev matrix should not have this value: " << cur << endl;
                    k += 100;
            }
        }
        nss << "\n" << mss.str() << "\n";
        return nss.str();
    }

    public string results()
    {
        solve();
        return interpretPath(findPath());
    }

    public string toString()
    {
        stringstream ss;
        ss << "matrix:\n";
        int j;
        for (int i = 0; i < m; i++) {
            for (j = 0; j < n; j++) {
                ss << matrix[i][j] << "\t";
            }
            ss << "\n";
        }
        ss << "prev:\n";
        for (int i = 0; i < m; i++) {
            for (int j = 0; j < n; j++) {
                ss << prev[i][j] << "\t";
            }
            ss << "\n";
        }

        return ss.str();
    }

}
