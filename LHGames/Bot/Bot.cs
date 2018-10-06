using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LHGames.Helper;

namespace LHGames.Bot
{
    internal class Bot
    {
        public const int INVALID_DIRECTION = 5;
        public const int MAX_RANDOM_DISTANCE = 5;
        internal IPlayer PlayerInfo { get; set; }
        private int _currentDirection = 1;
        // bool foundResource = false;
        // Point resourcePoint;
        private int randomDirection;
        private int previousDirection = INVALID_DIRECTION;
        private int randomDistance;
        private int distanceTravelled = 0;
        bool moving = false;
        bool movingRandom = true;
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
                previousDirection = INVALID_DIRECTION;
            }
            if(movingRandom){
                return moveRandomly();
            }

            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            return AIHelper.CreateMoveAction(new Point(0,-1));
        }

        internal string moveRandomly(){
            if(!moving){
                Random rnd = new Random();
                randomDirection = rnd.Next(1,5);
                randomDistance = rnd.Next(1,MAX_RANDOM_DISTANCE);
                moving = true;

                while(randomDirection == previousDirection){
                    randomDirection = rnd.Next(1,5);
                }
            }

            if(distanceTravelled >= randomDistance){
                moving = false;
                distanceTravelled = 0;
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
            return AIHelper.CreateMoveAction(new Point(0,1));
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