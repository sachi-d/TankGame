using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_1
{
    class Game
    {
        public Map map;
        public AI.Game_AI AI;
        public AI.PathFinder path_finder;

        public Game()
        {
            this.map = new Map();
            this.AI = new AI.Game_AI(map);
            this.path_finder = new AI.PathFinder(map);
        }

        static void Main(string[] args)
        {
            Game game = new Game();
            AI.Cell pos = new AI.Cell();
            pos.x = map.Client.getxCordinate();
            //game.AI.computeTarget(new AI.Cell())

        }
       
    }
}
