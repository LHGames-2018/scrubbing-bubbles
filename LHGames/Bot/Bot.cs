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
        

        internal Bot() { }

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
                    AIHelper.CreateCollectAction(new Point(res.Key.X, res.Key.Y));
                }
            }

            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
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