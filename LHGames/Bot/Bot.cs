using System;
using System.Collections.Generic;
using LHGames.Helper;

namespace LHGames.Bot
{
    internal class Bot
    {
        internal IPlayer PlayerInfo { get; set; }
        private int _currentDirection = 1;
        bool foundResource = false;
        Point resourcePoint;
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
            if(!this.foundResource){
                for(int i = PlayerInfo.Position.X - 100; i < PlayerInfo.Position.X + 100; i++){
                    for(int j = PlayerInfo.Position.Y - 100; j < PlayerInfo.Position.Y + 100; i++){
                        if(map.GetTileAt(i, j) == TileContent.Resource){
                            resourcePoint = new Point(i, j);
                            foundResource = true;
                        }
                    }
                }

            }
            // TODO: Implement your AI here.
            if (map.GetTileAt(PlayerInfo.Position.X + _currentDirection, PlayerInfo.Position.Y) == TileContent.Wall)
            {
                _currentDirection *= -1;
            }

            if(resourcePoint.X != PlayerInfo.Position.X && foundResource){
                int diffX = resourcePoint.X - PlayerInfo.Position.X;
                return AIHelper.CreateMoveAction(new Point(diffX, 0));

            }

            if(resourcePoint.Y != PlayerInfo.Position.Y && foundResource){
                int diffY = resourcePoint.Y - PlayerInfo.Position.Y;
                return AIHelper.CreateMoveAction(new Point(0, diffY));

            }

            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            Console.WriteLine(foundResource);
            Console.WriteLine(resourcePoint.X.ToString(), resourcePoint.Y.ToString());
            return AIHelper.CreateMoveAction(new Point(_currentDirection, 0));
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