using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Three_Dimensional_V2
{
    class Triangle3
    {
        public List<double[]> ps = new List<double[]>();
        public Triangle3(double[] _p1, double[] _p2, double[] _p3)
        {
            ps.Add(_p1);
            ps.Add(_p2);
            ps.Add(_p3);
        }
    }
}
