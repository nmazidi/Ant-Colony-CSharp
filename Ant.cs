using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonySimulation
{
    class Ant
    {
        public int X { set; get; }
        public int Y { set; get; }

        public bool isCarrying { set; get; }

        public bool isCloseToAnt { set; get; }
        public bool isCloseToNest { set; get; }
        public bool isCloseToFood { set; get; }
        public bool isCloseToRobberNest { set; get; }
        public int closestAnt { set; get; }

        public bool foodLocationKnown { set; get; }
        public int foodClosest { set; get; }

        public bool nestLocationKnown { set; get; }
        public int nestClosest { set; get; }
        public int robberNestClosest { set; get; }
        

        private Random randomObj;
        
        /// <summary>
        /// Assigns the values of the x and y coordinates given to the existing properties X and Y.
        /// </summary>
        /// <param name="rand">Random object</param>
        public Ant(Random rand)
        {
            randomObj = rand;

            X = randomObj.Next(0,600);
            Y = randomObj.Next(0,450);

            Console.WriteLine("ant created at {0},{1} ", X, Y);
        }        
    }
}
