using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerspective
{
    class PerspectiveData
    {
        public double[,] XLeft { get; set; }
        public double[,] XRight { get; set; }
        public int[,] Lines { get; set; }
    }
}
