using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    interface IQueue
    {

        void Insert(int node, List<double> distances);

        int Deletemin(List<double> distances);

        void Decreasekey(int key, List<double> distances);

    }
}
