using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LHGames.Helper;

namespace LHGames.Bot
{
    internal class Bot
    {
        internal IPlayer PlayerInfo { get; set; }
        private int _currentDirection = 1;
        // bool foundResource = false;
        // Point resourcePoint;
        private int randomDirection;
        private int randomDistance;
        private int distanceTravelled;
        
        bool moving = false;

        internal Bot() { 
            
        }

        /// <summary>
        /// Gets called before ExecuteTurn. This is where you get your bot's state.
        /// </summary>
        /// <param name="playerInfo">Your bot's current state.</param>
        internal void BeforeTurn(IPlayer playerInfo)
        {
            PlayerInfo = playerInfo;
        }

        /// <summary>
        /// Implement your bot here.
        /// </summary>
        /// <param name="map">The gamemap.</param>
        /// <param name="visiblePlayers">Players that are visible to your bot.</param>
        /// <returns>The action you wish to execute.</returns>
        internal string ExecuteTurn(Map map, IEnumerable<IPlayer> visiblePlayers)
        {
            Console.WriteLine("execute turn");
            if (map.GetTileAt(PlayerInfo.Position.X + _currentDirection, PlayerInfo.Position.Y) == TileContent.Wall)
            {
                _currentDirection *= -1;
                distanceTravelled = 0;
                moving = false;
            }

            int []mineralDirection = MineralAdjacentDirection(map);
            if (mineralDirection != null)
            {
                return AIHelper.CreateCollectAction(new Point(mineralDirection[0], mineralDirection[1]));
            }

            Console.WriteLine("moving: ", moving);
            if (!moving)
            {
                Random rnd = new Random();
                randomDirection = rnd.Next(1, 5);
                randomDistance = rnd.Next(1, 3);
            }
            
            if(moving){
                Console.WriteLine("moving");
                switch(randomDirection){
                    case 1:
                        distanceTravelled++;
                        if (MeleeTargetExists(map, 1, 0))
                            return AIHelper.CreateMeleeAttackAction(new Point(1, 0));
                        return AIHelper.CreateMoveAction(new Point(1, 0));
                    case 2:
                        distanceTravelled++;
                        if (MeleeTargetExists(map, -1, 0))
                            return AIHelper.CreateMeleeAttackAction(new Point(-1, 0));
                        return AIHelper.CreateMoveAction(new Point(-1, 0));
                    case 3:
                        distanceTravelled++;
                        if (MeleeTargetExists(map, 0, 1))
                            return AIHelper.CreateMeleeAttackAction(new Point(0, 1));
                        return AIHelper.CreateMoveAction(new Point(0, 1));
                    case 4:
                        distanceTravelled++;
                        if (MeleeTargetExists(map, 0, -1))
                            return AIHelper.CreateMeleeAttackAction(new Point(0, -1));
                        return AIHelper.CreateMoveAction(new Point(0, -1));    
                }
            }

            if(distanceTravelled >= randomDistance){
                moving = false;
                distanceTravelled = 0;
            }

            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            return AIHelper.CreateEmptyAction();
        }

        /// <summary>
        /// Gets called after ExecuteTurn.
        /// </summary>
        internal void AfterTurn()
        {
        }

        // Checks if there is anything to destroy in the direction to move
        internal Boolean MeleeTargetExists(Map map, int directionX, int directionY)
        {
            return (map.GetTileAt(PlayerInfo.Position.X + directionX, PlayerInfo.Position.Y + directionY) == TileContent.Wall || map.GetTileAt(PlayerInfo.Position.X + directionX, PlayerInfo.Position.Y + directionY) == TileContent.Player);
        }

        // Checks if there is a mineral deposit in any adjacent tile
        internal int[] MineralAdjacentDirection(Map map)
        {
            if (map.GetTileAt(PlayerInfo.Position.X + 1, PlayerInfo.Position.Y) == TileContent.Resource)
                return new int[2] {1, 0};
            else if (map.GetTileAt(PlayerInfo.Position.X - 1, PlayerInfo.Position.Y) == TileContent.Resource)
                return new int[2] {-1, 0};
            else if (map.GetTileAt(PlayerInfo.Position.X, PlayerInfo.Position.Y + 1) == TileContent.Resource)
                return new int[2] {0, 1};
            else if (map.GetTileAt(PlayerInfo.Position.X, PlayerInfo.Position.Y - 1) == TileContent.Resource)
                return new int[2] {0, -1};
            return null;
        }
    }
}

class TestClass
{
    public string Test { get; set; }
}