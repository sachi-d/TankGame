using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank_1.AI
{
    public class Game_AI
    {
        public enum Mode
        {
            Greedy,
            Defensive,
            Offensive  
        }
        private Mode mode;
        private Map map;
        int criticalHealth = 50;
        private Stack<Cell> path;
        public bool targetComputed = false;
        private Cell target;
        //should implement pathfinder

        public Game_AI(Map map)
        {
            this.map = map;
            mode = Mode.Greedy;
        }

        public Map Map
        {
            get
            {
                return map;
            }
            set
            {
                map = value;
            }
        }

        public Cell Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }



        public void setMode(Player player)
        {
            if (player.getHealth() == 100)
            {
                mode = Mode.Offensive;
            }
            else if (player.getHealth() <= criticalHealth)
            {
                mode = Mode.Defensive;
            }
            else{
                mode = Mode.Greedy;
            }
        }

        public Mode getMode()
        {
            return mode;
        }

        public Cell getCell(int x, int y)
        {
            Cell cell = new Cell();
            Actor actor = map.getActor(x, y);
            cell.x = x;
            cell.y = y;
            if (actor == null)
            {
                cell.Type = Actors.ActorType.Empty;
            }
            else
            {
                cell.Type = actor.Type;
                cell.cellDirection = actor.getcDirection();
            }
            return cell;
        }

        public Cell getCurrentCell()
        {
            return getCell(map.Client.getxCordinate(), map.Client.getyCordinate());
        }

        public List<Cell> getTankCells()
        {
            List<Cell> tanks = new List<Cell>();
            foreach (Player tank in map.tanks)
            {
                if (tank.getid() != map.Client.getid())
                {
                    Cell cell = new Cell();
                    cell.Type = Actors.ActorType.Player;
                    cell.x = tank.getxCordinate();
                    cell.y = tank.getyCordinate();
                    cell.cellDirection = tank.getcDirection();
                    cell.health = tank.getHealth();
                    cell.points = tank.getPoints();
                    tanks.Add(cell);
                }
            }
            return tanks;
        }

        public List<Cell> getCoinCells()
        {
            List<Cell> coins = new List<Cell>();
            foreach (Coin coin in map.coinPiles)
            {
                Cell cell = new Cell();
                cell.Type = Actors.ActorType.Coin;
                cell.x = coin.getxCordinate();
                cell.y = coin.getyCordinate();
                cell.value = coin.getValue();
                coins.Add(cell);
            }
            return coins;
        }

        public List<Cell> getLifeCells()
        {
            List<Cell> lifePacks = new List<Cell>();
            foreach (LifePack lp in map.lifePacks)
            {
                Cell cell = new Cell();
                cell.Type = Actors.ActorType.LifePack;
                cell.x = lp.getxCordinate();
                cell.y = lp.getyCordinate();
                cell.life = lp.getTime(); // **check whether lp.time is the remaining time
                lifePacks.Add(cell);
            }
            return lifePacks;
        }

        public List<Cell> getBulletCells()
        {
            List<Cell> bullets = new List<Cell>();
            foreach (Bullet bullet in map.bullets)
            {
                Cell cell = new Cell();
                cell.Type = Actors.ActorType.Bullet;
                cell.x = bullet.getxCordinate();
                cell.y = bullet.getyCordinate();
                cell.cellDirection = bullet.getDirection();
                bullets.Add(cell);
            }
            return bullets;
        }

        public List<Cell> getBrickCells()
        {
            List<Cell> bricks = new List<Cell>();
            foreach (Brick brick in map.bricks)
            {
                Cell cell = new Cell();
                cell.Type = Actors.ActorType.Brick;
                cell.x = brick.getxCordinate();
                cell.y = brick.getyCordinate();
                cell.life = long.Parse(brick.getLife());
                bricks.Add(cell);
            }
            return bricks;
        }

        public List<Cell> getStoneCells()
        {
            List<Cell> stones = new List<Cell>();
            foreach (Stone stone in map.stones)
            {
                Cell cell = new Cell();
                cell.Type = Actors.ActorType.Stone;
                cell.x = stone.getxCordinate();
                cell.y = stone.getyCordinate();
                stones.Add(cell);
            }
            return stones;
        }

        public List<Cell> getWaterCells()
        {
            List<Cell> waterPits = new List<Cell>();
            foreach (Water water in map.waterPits)
            {
                Cell cell = new Cell();
                cell.Type = Actors.ActorType.Water;
                cell.x = water.getxCordinate();
                cell.y = water.getyCordinate();
                waterPits.Add(cell);
            }
            return waterPits;
        }

        /*public List<Cell> getFreeCells()
        {
            List<Cell> freeCells = new List<Cell>();
            foreach (Block freeCell in map.freeCells)
            {
                Cell cell = new Cell();
                if (freeCell != getCurrentCell())
                {
                    cell.Type = Actors.ActorType.Empty;
                    cell.x = freeCell.getxCordinate();
                    cell.y = freeCell.getyCordinate();
                    freeCells.Add(cell);
                }
                
            }
            return freeCells;
        }*/
        public int getDistance(Cell cell_1, Cell cell_2)
        {
            int dx = Math.Abs(cell_2.x - cell_1.x);
            int dy = Math.Abs(cell_2.y - cell_1.y);
            return dx + dy;
        }

        public Cell getTankTarget(Cell cell)
        {
            List <Cell> tankTargets = getTankCells();
            int distance = 1000;
            Cell target = null;
            int calculated_distance;
            foreach (Cell tank in tankTargets)
            {
                calculated_distance = getDistance(cell, tank);
                if (calculated_distance < distance)
                {
                    distance = calculated_distance;
                    target = tank;
                }
            }
            return target;
        }

        public Cell getCoinPileTarget(Cell cell)
        {
            List<Cell> coinPileTargets = getCoinCells();
            int distance = 1000;
            Cell target = null;
            int calculated_distance;
            foreach (Cell coinPile in coinPileTargets)
            {
                calculated_distance = getDistance(cell, coinPile);
                if (calculated_distance < distance && (calculated_distance/map.Client.Speed)<coinPile.life)
                {
                    distance = calculated_distance;
                    target = coinPile;
                }
            }
            return target;
        }

        public Cell getLifePackTarget(Cell cell)
        {
            List<Cell> LifePackTargets = getLifeCells();
            int distance = 1000;
            Cell target = null;
            int calculated_distance;
            foreach (Cell lifePack in LifePackTargets)
            {
                calculated_distance = getDistance(cell, lifePack);
                if (calculated_distance < distance && (calculated_distance / map.Client.Speed) < lifePack.life)
                {
                    distance = calculated_distance;
                    target = lifePack;
                }
            }
            return target;
        }

        public Cell getBrickTarget(Cell cell)
        {
            List<Cell> brickTargets = getBrickCells();
            int distance = 1000;
            Cell target = null;
            int calculated_distance;
            foreach (Cell brick in brickTargets)
            {
                calculated_distance = getDistance(cell, brick);
                if (calculated_distance < distance)
                {
                    distance = calculated_distance;
                    target = brick;
                }
            }
            return target;
        }

        /*public Cell getFreeCellTarget(Cell cell)
        {
            List<Cell> freeCellTargets = getFreeCells();
            int distance = 1000;
            Cell target = null;
            int calculated_distance;
            foreach (Cell freeCell in freeCellTargets)
            {
                calculated_distance = getDistance(cell, freeCell);
                if (calculated_distance < distance && computeTarget(freeCell)!=null)     //**implement for freecelltarget
                {
                    distance = calculated_distance;
                    target = freeCell;
                }
            }
            return target;
        }*/

        public void computeTarget(Cell cell)
        {
            //Cell Target = null;
            switch (mode)
            {
                case Mode.Offensive:
                    Target = getTankTarget(cell);
                    if (Target == null)
                    {
                        Target = getCoinPileTarget(cell);
                    }
                    if (Target == null)
                    {
                        Target = getBrickTarget(cell);
                    }
                    if (Target == null)
                    {
                        if (map.Client.getHealth() < 100)
                        {
                            Target = getLifePackTarget(cell);
                        }
                    }
                    break;
                case Mode.Greedy:
                    Target = getCoinPileTarget(cell);
                    if (Target == null)
                    {
                        Target = getTankTarget(cell);           //**check the prioroties
                    }
                    if (Target == null)
                    {
                        Target = getBrickTarget(cell);
                    }
                    if (Target == null)
                    {
                        if (map.Client.getHealth() < 100)
                        {
                            Target = getLifePackTarget(cell);
                        }
                    }
                    break;
                case Mode.Defensive:
                    Target = getLifePackTarget(cell);
                    if (Target == null)
                    {
                        Target = getCoinPileTarget(cell);
                    }
                    if (Target == null)
                    {
                        Target = getBrickTarget(cell);           //**check the prioroties
                    }
                    if (Target == null)
                    {
                        Target = getTankTarget(cell);
                    }
                    break;
            }
            if (Target == null)
            {
                Target = getCurrentCell();
            }

        }
       
       

       
    }
}
