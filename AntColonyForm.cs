using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace AntColonySimulation
{

    public partial class AntColonyForm : Form
    {

        private List<Ant> antList;
        private List<Food> foodList;
        private List<Nest> nestList;
        private List<RobberAnt> robberAntList;
        private List<RobberNest> robberNestList;
        
        private Random randomAntObject;
        
        public AntColonyForm()
        {
            InitializeComponent();
            
            foodList = new List<Food>();
            nestList = new List<Nest>();
            robberNestList = new List<RobberNest>();

            randomAntObject = new Random();
            MakeAnts();
            myTimer.Start();            
        }

        private void MakeAnts()
        {
            Ant tempAnt;
            RobberAnt tempRobber;
            antList = new List<Ant>();
            robberAntList = new List<RobberAnt>();

            for (int i = 0; i < 150; i++)
            {
                tempAnt = new Ant(randomAntObject);
                antList.Add(tempAnt);
            }            
            for (int i = 0; i < 20; i++)
            {
                tempRobber = new RobberAnt(randomAntObject);
                robberAntList.Add(tempRobber);
            }
        }
        /// <summary>
        /// Draws a specified ant on the panel and wipes it's previous position
        /// </summary>
        /// <param name="antID">Position of the ant in 'AntList' list</param>
        /// <param name="oldPosX">Previous X coordinate of the ant</param>
        /// <param name="oldPosY">previous Y coordinate of the ant</param>
        private void DrawAnt(int antID, int oldPosX, int oldPosY)
        {
            int posX, posY;

            using (Graphics worldGraphics = myPanel.CreateGraphics())
            using (Brush brushAnt = new SolidBrush(Color.Black))
            using (Brush brushCarrying = new SolidBrush(Color.Yellow))
            using (Brush brushClear = new SolidBrush(Color.White))
            {
                worldGraphics.FillRectangle(brushClear, oldPosX, oldPosY, 5, 5);
                
                posX = antList.ElementAt(antID).X;
                posY = antList.ElementAt(antID).Y;
                    
                worldGraphics.FillRectangle(brushAnt, posX, posY, 5, 5);
                
                if(antList.ElementAt(antID).isCarrying == true)
                {
                    worldGraphics.FillRectangle(brushCarrying, posX+1, posY+1, 3, 3);
                }                
            }
            
        }
        private void DrawRobberAnt(int antID, int oldPosX, int oldPosY)
        {
            int posX, posY;

            using (Graphics worldGraphics = myPanel.CreateGraphics())
            using (Brush brushRobberAnt = new SolidBrush(Color.Red))
            using (Brush brushCarrying = new SolidBrush(Color.Yellow))
            using (Brush brushClear = new SolidBrush(Color.White))
            {
                worldGraphics.FillRectangle(brushClear, oldPosX, oldPosY, 5, 5);

                posX = robberAntList.ElementAt(antID).X;
                posY = robberAntList.ElementAt(antID).Y;

                worldGraphics.FillRectangle(brushRobberAnt, posX, posY, 5, 5);

                if (robberAntList.ElementAt(antID).isCarrying == true)
                {
                    worldGraphics.FillRectangle(brushCarrying, posX + 1, posY + 1, 3, 3);
                }    
            }
        }
        /// <summary>
        /// Draws the food-source on the panel as a yellow ellipse 
        /// </summary>
        /// <param name="foodID">Position of the food in 'FoodList' list</param>
        private void DrawFood(int foodID)
        {
            int posX, posY;

            using (Graphics worldGraphics = myPanel.CreateGraphics())
            using (Brush brushFood = new SolidBrush(Color.Yellow))
            {
                posX = foodList.ElementAt(foodID).X;
                posY = foodList.ElementAt(foodID).Y;

                worldGraphics.FillEllipse(brushFood, posX, posY, 15, 15);
            }
        }
        /// <summary>
        /// Erases the food source off of the panel and deletes from foodList when empty
        /// </summary>
        /// <param name="foodID">Position of the food in 'FoodList' list</param>
        private void EraseFood(int foodID)
        {
            int posX, posY;

            using (Graphics worldGraphics = myPanel.CreateGraphics())
            using (Brush brushClearFood = new SolidBrush(Color.White))
            {
                posX = foodList.ElementAt(foodID).X;
                posY = foodList.ElementAt(foodID).Y;

                worldGraphics.FillEllipse(brushClearFood, posX, posY, 15, 15);
            }
            
            for (int i = 0; i < antList.Count; i++)
            {
                if (antList.ElementAt(i).foodClosest == foodID)
                {
                    antList.ElementAt(i).foodLocationKnown = false;
                }
            }

            //Remove food from list
            foodList.RemoveAt(foodID);
            foodList.Insert(foodID, null);

            Console.WriteLine("No more food");
        }
        /// <summary>
        /// Draws the nest on the panel as a brown ellipse
        /// </summary>
        /// <param name="nestID">position of nest in the 'NestList' list</param>
        ///         
        private void DrawNest(int nestID)
        {
            int posX, posY;

            using (Graphics worldGraphics = myPanel.CreateGraphics())
            using (Brush brushNest = new SolidBrush(Color.SaddleBrown))
            {
                posX = nestList.ElementAt(nestID).X;
                posY = nestList.ElementAt(nestID).Y;

                worldGraphics.FillEllipse(brushNest, posX, posY, 15, 15);
            }
        }
        /// <summary>
        /// Draws the robber nest on the panel as a green ellipse
        /// </summary>
        /// <param name="nestID">position of nest in the 'NestList' list</param>
        ///         
        private void DrawRobberNest(int nestID)
        {
            int posX, posY;

            using (Graphics worldGraphics = myPanel.CreateGraphics())
            using (Brush brushNest = new SolidBrush(Color.Green))
            {
                posX = robberNestList.ElementAt(nestID).X;
                posY = robberNestList.ElementAt(nestID).Y;

                worldGraphics.FillEllipse(brushNest, posX, posY, 15, 15);
            }
        }
        private void myTimer_Tick(object sender, EventArgs e)
        {

            for (int i = 0; i < antList.Count; i++)
            {
                antList.ElementAt(i).isCloseToNest = false;
                antList.ElementAt(i).isCloseToFood = false;
                for (int j = 0; j < foodList.Count; j++)
                {
                    if (foodList.ElementAt(j) != null)
                    {
                        if (antList.ElementAt(i).X <= (foodList.ElementAt(j).X + 20) && antList.ElementAt(i).X >= (foodList.ElementAt(j).X - 5))
                        //if ant is horizontally close to food
                        {
                            if (antList.ElementAt(i).Y <= (foodList.ElementAt(j).Y + 20) && antList.ElementAt(i).Y >= (foodList.ElementAt(j).Y - 5))
                            //if ant is vertically close to food
                            {
                                antList.ElementAt(i).isCloseToFood = true;
                                antList.ElementAt(i).foodClosest = j;
                            }
                        }
                    }
                }       
                for (int k = 0; k < nestList.Count; k++)
                {
                    if (antList.ElementAt(i).X <= (nestList.ElementAt(k).X + 20) && antList.ElementAt(i).X >= (nestList.ElementAt(k).X - 5))
                    //if ant is horizontally close to nest
                    {
                        if (antList.ElementAt(i).Y <= (nestList.ElementAt(k).Y + 20) && antList.ElementAt(i).Y >= (nestList.ElementAt(k).Y - 5))
                        //if ant is vertically close to nest
                        {
                            antList.ElementAt(i).isCloseToNest = true;
                            antList.ElementAt(i).nestClosest = k;
                            antList.ElementAt(i).nestLocationKnown = true;
                        }
                    }
                }
                for (int r = 0; r < robberNestList.Count; r++)
                {
                    if (antList.ElementAt(i).X <= (robberNestList.ElementAt(r).X + 20) && antList.ElementAt(i).X >= (robberNestList.ElementAt(r).X - 5))
                    //if ant is horizontally close to nest
                    {
                        if (antList.ElementAt(i).Y <= (robberNestList.ElementAt(r).Y + 20) && antList.ElementAt(i).Y >= (robberNestList.ElementAt(r).Y - 5))
                        //if ant is vertically close to nest
                        {
                            antList.ElementAt(i).isCloseToRobberNest = true;
                            antList.ElementAt(i).robberNestClosest = r;
                        }
                    }
                }
                antList.ElementAt(i).isCloseToAnt = false;
                for (int l = 0; l < antList.Count; l++)
                //loop though all ants again
                {
                    if (antList.ElementAt(i).X <= (antList.ElementAt(l).X + 10) && antList.ElementAt(i).X >= (antList.ElementAt(l).X - 5))
                    //if ant is horizontally close to another ant
                    {
                        if (antList.ElementAt(i).Y <= (antList.ElementAt(l).Y + 10) && antList.ElementAt(i).Y >= (antList.ElementAt(l).Y - 5))
                        //if ant is vertically close to another ant
                        {
                            antList.ElementAt(i).isCloseToAnt = true;
                            antList.ElementAt(i).closestAnt = l;
                            break;
                        }
                    }
                }
                if (antList.ElementAt(i).isCarrying == true)
                {
                    if (antList.ElementAt(i).isCloseToNest == true)
                    {
                        DepositFood(i);                            
                    }
                    else
                    {
                        if (antList.ElementAt(i).nestLocationKnown == true)
                        {
                            MoveToNest(i);
                        }
                        else
                        {
                            RandomMove(i);
                        }
                    }
                }
                else
                {
                    if (antList.ElementAt(i).isCloseToFood == true)
                    {                           
                        PickUpFood(i);
                    }
                    else
                    { 
                        if (antList.ElementAt(i).foodLocationKnown == true)
                        {
                            MoveToFood(i);
                        }
                        else
                        {
                            RandomMove(i);                                
                        }
                    }
                }
                if (antList.ElementAt(i).isCloseToAnt == true)
                {
                    AskForLocations(i);
                }
            }
            for (int i = 0; i < robberAntList.Count; i++)
            //loop though all robber ants 
            {
                robberAntList.ElementAt(i).isCloseToAnt = false;
                robberAntList.ElementAt(i).isCloseToRobberNest = false;

                for (int j = 0; j < antList.Count; j++)
                //loop though all ants
                {
                    if (robberAntList.ElementAt(i).X <= (antList.ElementAt(j).X + 10) && robberAntList.ElementAt(i).X >= (antList.ElementAt(j).X - 5))
                    //if robber ant is horizontally close to ant
                    {
                        if (robberAntList.ElementAt(i).Y <= (antList.ElementAt(j).Y + 10) && robberAntList.ElementAt(i).Y >= (antList.ElementAt(j).Y - 5))
                        //if robber ant is vertically close to ant
                        {
                            robberAntList.ElementAt(i).isCloseToAnt = true;
                            robberAntList.ElementAt(i).closestAnt = j;
                            break;
                        }
                    }
                }
                for (int k = 0; k < foodList.Count; k++)
                {
                    if (foodList.ElementAt(k) != null)
                    {
                        if (robberAntList.ElementAt(i).X <= (foodList.ElementAt(k).X + 20) && robberAntList.ElementAt(i).X >= (foodList.ElementAt(k).X - 5))
                        //if ant is horizontally close to food
                        {
                            if (robberAntList.ElementAt(i).Y <= (foodList.ElementAt(k).Y + 20) && robberAntList.ElementAt(i).Y >= (foodList.ElementAt(k).Y - 5))
                            //if ant is vertically close to food
                            {
                                robberAntList.ElementAt(i).isCloseToFood = true;
                                robberAntList.ElementAt(i).foodClosest = k;
                            }
                        }
                    }
                }
                for (int l = 0; l < nestList.Count; l++)
                {
                    if (robberAntList.ElementAt(i).X <= (nestList.ElementAt(l).X + 20) && robberAntList.ElementAt(i).X >= (nestList.ElementAt(l).X - 5))
                    //if ant is horizontally close to nest
                    {
                        if (robberAntList.ElementAt(i).Y <= (nestList.ElementAt(l).Y + 20) && robberAntList.ElementAt(i).Y >= (nestList.ElementAt(l).Y - 5))
                        //if ant is vertically close to nest
                        {
                            robberAntList.ElementAt(i).isCloseToNest = true;
                            robberAntList.ElementAt(i).nestClosest = l;
                        }
                    }
                }
                for (int r = 0; r < robberNestList.Count; r++)
                {
                    if (robberAntList.ElementAt(i).X <= (robberNestList.ElementAt(r).X + 20) && robberAntList.ElementAt(i).X >= (robberNestList.ElementAt(r).X - 5))
                    //if ant is horizontally close to nest
                    {
                        if (robberAntList.ElementAt(i).Y <= (robberNestList.ElementAt(r).Y + 20) && robberAntList.ElementAt(i).Y >= (robberNestList.ElementAt(r).Y - 5))
                        //if ant is vertically close to nest
                        {
                            robberAntList.ElementAt(i).isCloseToRobberNest = true;
                            robberAntList.ElementAt(i).robberNestClosest = r;
                            robberAntList.ElementAt(i).robberNestLocationKnown = true;
                        }
                    }
                }
                if (robberAntList.ElementAt(i).isCarrying == true)
                {
                    if (robberAntList.ElementAt(i).isCloseToRobberNest == true)
                    {                            
                        RobberDepositFood(i);
                    }
                    else
                    {
                        if (robberAntList.ElementAt(i).robberNestLocationKnown == true)
                        {
                            RobberMoveToNest(i);
                        }
                        else
                        {
                            RobberRandomMove(i);
                        }
                    }
                }
                else
                {
                    if (robberAntList.ElementAt(i).isCloseToAnt == true)
                    {
                        if (antList.ElementAt(robberAntList.ElementAt(i).closestAnt).isCarrying == true)
                        {
                            StealFood(i);
                        }
                        else
                        {
                            RobberRandomMove(i);
                        }                        
                    }
                    else
                    {                        
                        if (robberAntList.ElementAt(i).antLocationKnown == true)
                        {
                            MoveToKnownAntLocation(i);
                            if (robberAntList.ElementAt(i).X == robberAntList.ElementAt(i).lastAntX && robberAntList.ElementAt(i).Y == robberAntList.ElementAt(i).lastAntY)
                            {                               
                                    robberAntList.ElementAt(i).antLocationKnown = false;                                                                   
                            }
                        }
                        else
                        {
                            RobberRandomMove(i);
                        }
                    }
                }                
            }
        }
        /// <summary>
        /// Makes the specified ant move in a random direction
        /// </summary>
        /// <param name="antID">Position of the ant in 'AntList' list</param>
        private void RandomMove(int antID)
        {
            int randomNum;
            int tempX, tempY;

            tempX = antList.ElementAt(antID).X;
            tempY = antList.ElementAt(antID).Y;

            randomNum = randomAntObject.Next(1, 9);
            // 1 to 8 clockwise (i.e. 1 = northwest)
            
            switch (randomNum)
            {
                case 1:
                        antList.ElementAt(antID).Y--;
                        antList.ElementAt(antID).X--;
                    break;
                case 2:
                        antList.ElementAt(antID).Y--;
                        break;
                case 3:
                        antList.ElementAt(antID).Y--;
                        antList.ElementAt(antID).X++;
                    break;
                case 4:
                        antList.ElementAt(antID).X++;
                    break;
                case 5:
                        antList.ElementAt(antID).Y++;
                        antList.ElementAt(antID).X++;
                    break;
                case 6:
                        antList.ElementAt(antID).Y++;
                    break;
                case 7:
                        antList.ElementAt(antID).Y++;
                        antList.ElementAt(antID).X--;

                    break;
                case 8:
                        antList.ElementAt(antID).X--;
                    break;
            }
            DrawAnt(antID, tempX, tempY);
            if (antList.ElementAt(antID).isCloseToNest == true)
            {
                DrawNest(antList.ElementAt(antID).nestClosest);
            }
            if (antList.ElementAt(antID).isCloseToFood == true)
            {
                DrawFood(antList.ElementAt(antID).foodClosest);
            }
            if (antList.ElementAt(antID).isCloseToRobberNest == true)
            {
                DrawRobberNest(antList.ElementAt(antID).robberNestClosest);
            }        
        }
        /// <summary>
        /// Makes the specified ant move in a random direction
        /// </summary>
        /// <param name="antID">Position of the ant in 'AntList' list</param>
        private void RobberRandomMove(int antID)
        {
            int randomNum;
            int tempX, tempY;

            tempX = robberAntList.ElementAt(antID).X;
            tempY = robberAntList.ElementAt(antID).Y;

            randomNum = randomAntObject.Next(1, 9);
            // 1 to 8 clockwise (i.e. 1 = northwest)

            switch (randomNum)
            {
                case 1:
                    robberAntList.ElementAt(antID).Y--;
                    robberAntList.ElementAt(antID).X--;
                    break;
                case 2:
                    robberAntList.ElementAt(antID).Y--;
                    break;
                case 3:
                    robberAntList.ElementAt(antID).Y--;
                    robberAntList.ElementAt(antID).X++;
                    break;
                case 4:
                    robberAntList.ElementAt(antID).X++;
                    break;
                case 5:
                    robberAntList.ElementAt(antID).Y++;
                    robberAntList.ElementAt(antID).X++;
                    break;
                case 6:
                    robberAntList.ElementAt(antID).Y++;
                    break;
                case 7:
                    robberAntList.ElementAt(antID).Y++;
                    robberAntList.ElementAt(antID).X--;

                    break;
                case 8:
                    robberAntList.ElementAt(antID).X--;
                    break;
            }
            DrawRobberAnt(antID, tempX, tempY);
            if (robberAntList.ElementAt(antID).isCloseToNest == true)
            {
                DrawNest(robberAntList.ElementAt(antID).nestClosest);
            }
            if (robberAntList.ElementAt(antID).isCloseToFood == true)
            {
                DrawFood(robberAntList.ElementAt(antID).foodClosest);
            }
            if (robberAntList.ElementAt(antID).isCloseToRobberNest == true)
            {
                DrawRobberNest(robberAntList.ElementAt(antID).robberNestClosest);
            }
        }
        /// <summary>
        /// Makes the specified ant pick up food by setting it's 'isCarrying' property to true
        /// </summary>
        /// <param name="antID">Position of the ant in 'AntList' list</param>
        private void PickUpFood(int antID)
        {
            int foodClosest;
            foodClosest = antList.ElementAt(antID).foodClosest;

            foodList.ElementAt(foodClosest).Quantity--;
            antList.ElementAt(antID).isCarrying = true;
            antList.ElementAt(antID).foodLocationKnown = true;
            Console.WriteLine("food picked up");

            if (foodList.ElementAt(foodClosest).Quantity == 0)
            {
                EraseFood(foodClosest);
            }
        }
        /// <summary>
        /// Makes the specified ant drop the food that it's carrying in the nest that it is next to
        /// </summary>
        /// <param name="antID">Position of the ant in 'AntList' list</param>
        private void DepositFood(int antID)
        {
            antList.ElementAt(antID).isCarrying = false;

            antList.ElementAt(antID).nestLocationKnown = true;
            Console.WriteLine("deposited");
        }
        /// <summary>
        /// Makes the specified robber ant drop the food that it's carrying in the nest that it is next to
        /// </summary>
        /// <param name="antID">Position of the ant in 'robberAntList' list</param>
        private void RobberDepositFood(int antID)
        {
            robberAntList.ElementAt(antID).isCarrying = false;

            robberAntList.ElementAt(antID).robberNestLocationKnown = true;
            Console.WriteLine("robber deposited");
        }
        /// <summary>
        /// Makes the specified ant move closer to the nest that it knows the location of.
        /// </summary>
        /// <param name="antID">Position of the ant in 'AntList' list</param>
        private void MoveToNest(int antID)
        {
            int tempX, tempY;
            tempX = antList.ElementAt(antID).X;
            tempY = antList.ElementAt(antID).Y;

            if (antList.ElementAt(antID).X < nestList.ElementAt(antList.ElementAt(antID).nestClosest).X)
            {
                antList.ElementAt(antID).X++;
            }
            else
            {
                antList.ElementAt(antID).X--;
            }

            if (antList.ElementAt(antID).Y < nestList.ElementAt(antList.ElementAt(antID).nestClosest).Y)
            {
                antList.ElementAt(antID).Y++;
            }
            else
            {
                antList.ElementAt(antID).Y--;
            }
            DrawAnt(antID, tempX, tempY);
            if (antList.ElementAt(antID).isCloseToFood == true)
            {
                DrawFood(antList.ElementAt(antID).foodClosest);
            }
            if (antList.ElementAt(antID).isCloseToNest == true)
            {
                DrawNest(antList.ElementAt(antID).nestClosest);
            }
            if (antList.ElementAt(antID).isCloseToRobberNest == true)
            {
                DrawRobberNest(antList.ElementAt(antID).robberNestClosest);
            } 
        }
        /// <summary>
        /// Makes the specified robber ant move closer to the nest that it knows the location of.
        /// </summary>
        /// <param name="antID">Position of the robber ant in 'AntList' list</param>
        private void RobberMoveToNest(int antID)
        {
            int tempX, tempY;
            tempX = robberAntList.ElementAt(antID).X;
            tempY = robberAntList.ElementAt(antID).Y;

            if (robberAntList.ElementAt(antID).X < robberNestList.ElementAt(robberAntList.ElementAt(antID).robberNestClosest).X)
            {
                robberAntList.ElementAt(antID).X++;
            }
            else
            {
                robberAntList.ElementAt(antID).X--;
            }

            if (robberAntList.ElementAt(antID).Y < robberNestList.ElementAt(robberAntList.ElementAt(antID).robberNestClosest).Y)
            {
                robberAntList.ElementAt(antID).Y++;
            }
            else
            {
                robberAntList.ElementAt(antID).Y--;
            }
            DrawRobberAnt(antID, tempX, tempY);
            if (robberAntList.ElementAt(antID).isCloseToFood == true)
            {
                DrawFood(robberAntList.ElementAt(antID).foodClosest);
            }
            if (robberAntList.ElementAt(antID).isCloseToNest == true)
            {
                DrawNest(robberAntList.ElementAt(antID).nestClosest);
            }
            if (robberAntList.ElementAt(antID).isCloseToRobberNest == true)
            {
                DrawRobberNest(robberAntList.ElementAt(antID).robberNestClosest);
            }
        }
        /// <summary>
        /// Makes the specified ant move closer to the food-source that it knows the location of. 
        /// </summary>
        /// <param name="antID">Position of the ant in 'AntList' list</param>
        private void MoveToFood(int antID)
        {
            int tempX, tempY;
            tempX = antList.ElementAt(antID).X;
            tempY = antList.ElementAt(antID).Y;

            if (antList.ElementAt(antID).X < foodList.ElementAt(antList.ElementAt(antID).foodClosest).X)
            {
                antList.ElementAt(antID).X++;
            }
            else
            {
                antList.ElementAt(antID).X--;
            }

            if (antList.ElementAt(antID).Y < foodList.ElementAt(antList.ElementAt(antID).foodClosest).Y)
            {
                antList.ElementAt(antID).Y++;
            }
            else
            {
                antList.ElementAt(antID).Y--;
            }
            DrawAnt(antID, tempX, tempY);
            if (antList.ElementAt(antID).isCloseToFood == true)
            {
                DrawFood(antList.ElementAt(antID).foodClosest);
            }
            if (antList.ElementAt(antID).isCloseToNest == true)
            {
                DrawNest(antList.ElementAt(antID).nestClosest);
            }
            if (antList.ElementAt(antID).isCloseToRobberNest == true)
            {
                DrawRobberNest(antList.ElementAt(antID).robberNestClosest);
            }
        }
        /// <summary>
        /// Makes the specified ant learn the location of the food/nest from closest other ant 
        /// </summary>
        /// <param name="antID">Position of the ant in 'AntList' list</param>
        private void AskForLocations(int antID)
        {
            int closestAnt;
            closestAnt = antList.ElementAt(antID).closestAnt;

            if (antList.ElementAt(antID).foodLocationKnown == false && antList.ElementAt(closestAnt).foodLocationKnown == true)
            {
                antList.ElementAt(antID).foodClosest = antList.ElementAt(closestAnt).foodClosest;
                antList.ElementAt(antID).foodLocationKnown = true;
                Console.WriteLine("food learned");
            }
            if (antList.ElementAt(antID).nestLocationKnown == false && antList.ElementAt(closestAnt).nestLocationKnown == true)
            {
                antList.ElementAt(antID).nestClosest = antList.ElementAt(closestAnt).nestClosest;
                antList.ElementAt(antID).nestLocationKnown = true;
                Console.WriteLine("nest learned");
            }
        }
        /// <summary>
        /// Makes the robber ant steal the ant's food
        /// </summary>
        /// <param name="antID">position of the robber ant in the 'robberAntList' list</param>
        private void StealFood(int antID)
        {
            int closestAnt;
            closestAnt = robberAntList.ElementAt(antID).closestAnt;

            robberAntList.ElementAt(antID).isCarrying = true;
            antList.ElementAt(closestAnt).isCarrying = false;

            robberAntList.ElementAt(antID).antLocationKnown = true;
            robberAntList.ElementAt(antID).lastAntX = antList.ElementAt(closestAnt).X;
            robberAntList.ElementAt(antID).lastAntY = antList.ElementAt(closestAnt).Y;

            Console.WriteLine("food stolen");
        }
        /// <summary>
        /// Moves he robber ant to the last known location of the ant that it stole from
        /// </summary>
        /// <param name="antID">position of the robber ant in the 'robberAntList' list</param>
        private void MoveToKnownAntLocation(int antID)
        {
            int tempX, tempY;

            tempX = robberAntList.ElementAt(antID).X;
            tempY = robberAntList.ElementAt(antID).Y;

            if (robberAntList.ElementAt(antID).X < robberAntList.ElementAt(antID).lastAntX)
            {
                robberAntList.ElementAt(antID).X++;
            }
            else
            {
                robberAntList.ElementAt(antID).X--;
            }

            if (robberAntList.ElementAt(antID).Y < robberAntList.ElementAt(antID).lastAntY)
            {
                robberAntList.ElementAt(antID).Y++;
            }
            else
            {
                robberAntList.ElementAt(antID).Y--;
            }

            DrawRobberAnt(antID, tempX, tempY);
            if (robberAntList.ElementAt(antID).isCloseToFood == true)
            {
                DrawFood(robberAntList.ElementAt(antID).foodClosest);
            }
            if (robberAntList.ElementAt(antID).isCloseToNest == true)
            {
                DrawNest(robberAntList.ElementAt(antID).nestClosest);
            }
            if (robberAntList.ElementAt(antID).isCloseToRobberNest == true)
            {
                DrawRobberNest(robberAntList.ElementAt(antID).robberNestClosest);
            }
        }
        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            
            Point clickPoint;
            MouseButtons buttonPressed;

            clickPoint = e.Location;
            buttonPressed = e.Button;

            if (buttonPressed == MouseButtons.Left)
            {
                Food tempFood;
                tempFood = new Food(clickPoint.X, clickPoint.Y);
                tempFood.Quantity = 500;
                foodList.Add(tempFood);
                DrawFood(foodList.Count - 1);
            }
            else if (buttonPressed == MouseButtons.Right)
            {
                Nest tempNest;
                tempNest = new Nest(clickPoint.X, clickPoint.Y);
                nestList.Add(tempNest);
                DrawNest(nestList.Count - 1);
            }
            else if (buttonPressed == MouseButtons.Middle)
            {
                RobberNest tempRobberNest;
                tempRobberNest = new RobberNest(clickPoint.X, clickPoint.Y);
                robberNestList.Add(tempRobberNest);
                DrawRobberNest(robberNestList.Count - 1);
            }
        }
        private void Panel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
