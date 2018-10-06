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
            // if(!this.foundResource){
            //     for(int i = PlayerInfo.Position.X - 100; i < PlayerInfo.Position.X + 100; i++){
            //         for(int j = PlayerInfo.Position.Y - 100; j < PlayerInfo.Position.Y + 100; i++){
            //             if(map.GetTileAt(i, j) == TileContent.Resource){
            //                 resourcePoint = new Point(i, j);
            //                 foundResource = true;
            //             }
            //         }
            //     }

            // }

            // if(resourcePoint.X != PlayerInfo.Position.X && foundResource){
            //     int diffX = resourcePoint.X - PlayerInfo.Position.X;
            //     return AIHelper.CreateMoveAction(new Point(diffX, 0));

            // }

            // if(resourcePoint.Y != PlayerInfo.Position.Y && foundResource){
            //     int diffY = resourcePoint.Y - PlayerInfo.Position.Y;
            //     return AIHelper.CreateMoveAction(new Point(0, diffY));

            // }
            // TODO: Implement your AI here.
            if (map.GetTileAt(PlayerInfo.Position.X + _currentDirection, PlayerInfo.Position.Y) == TileContent.Wall)
            {
                _currentDirection *= -1;
            }

            // check adjacent tiles contents
            Dictionary<Point, TileContent> tileContents = new Dictionary<Point, TileContent>();

            var x = PlayerInfo.Position.X;
            var y = PlayerInfo.Position.Y;

            //todo check if x and y are null
            try
            {
                tileContents.Add(new Point(x + 1, y), map.GetTileAt(x + 1, y));
                tileContents.Add(new Point(x - 1, y), map.GetTileAt(x - 1, y));
                tileContents.Add(new Point(x, y + 1), map.GetTileAt(x, y + 1));
                tileContents.Add(new Point(x, y - 1), map.GetTileAt(x, y - 1));
            }
            catch (Exception)
            {
                // do nothing this turn
            }

            foreach (var res in tileContents.Where(kvp => kvp.Value.Equals(TileContent.Resource)).ToList())
            {
                //collect every resource around player
                if (PlayerInfo.CarriedResources < PlayerInfo.CarryingCapacity)
                {
                    AIHelper.CreateCollectAction(new Point(res.Key.X - x, res.Key.Y - y));
                }
            }
            foreach (var wall in tileContents.Where(kvp => kvp.Value.Equals(TileContent.Resource)).ToList())
            {
                return AIHelper.CreateMeleeAttackAction(wall.Key);
            }

            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            return AIHelper.CreateMoveAction(new Point(0, 1));
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