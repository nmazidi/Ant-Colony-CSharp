using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonySimulation
{
    class RobberNest
    {
        public int X { set; get; }
        public int Y { set; get; }

        /// <summary>
        /// Assigns the values of the x and y coordinates given to the existing properties X and Y.
        /// </summary>
        /// <param name="x">Horizontal location of the nest</param>
        /// <param name="y">Vertical location of the nest</param>
        public RobberNest(int x, int y)
        {
            X = x;
            Y = y;
            Console.WriteLine("Robber nest created at {0},{1} ", X, Y);
        }
    }































































}
