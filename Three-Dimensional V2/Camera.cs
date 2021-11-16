using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Three_Dimensional_V2
{
    class Camera
    {
        public double[] position;
        public PointD dir;
        public double fov;

        public Camera(double[] _position, double _fov, PointD _dir)
        {
            position = _position;
            fov = _fov;
            dir = _dir;
        }
    }
}
