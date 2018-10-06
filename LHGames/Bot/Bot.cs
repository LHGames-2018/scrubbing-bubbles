using System;
using System.Collections.Generic;
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

            if(!moving){
                Random rnd = new Random();
                randomDirection = rnd.Next(1,5);
                randomDistance = rnd.Next(1,3);
                moving = true;
            }
            
            if(moving){
                Console.WriteLine("moving");
                switch(randomDirection){
                    case 1:
                        distanceTravelled++;
                        return AIHelper.CreateMoveAction(new Point(1, 0));
                    case 2:
                        distanceTravelled++;
                        return AIHelper.CreateMoveAction(new Point(-1, 0));
                    case 3:
                        distanceTravelled++;
                        return AIHelper.CreateMoveAction(new Point(0, 1));
                    case 4:
                        distanceTravelled++;
                        return AIHelper.CreateMoveAction(new Point(0, -1));    
                }
            }

            if(distanceTravelled >= randomDistance){
                moving = false;
                distanceTravelled = 0;
            }

            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            return AIHelper.CreateMoveAction(new Point(0,-1));
        }

        /// <summary>
        /// Gets called after ExecuteTurn.
        /// </summary>
        internal void AfterTurn()
        {
        }
    }
}

class TestClass
{
    public string Test { get; set; }
}