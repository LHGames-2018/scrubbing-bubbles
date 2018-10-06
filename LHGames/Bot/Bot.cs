using System;
using System.Collections;
using System.Collections.Generic;
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

            IErnumerable tilecontents = CheckCurrentTileContentAround(map);
            switch (tilecontent)
            {
                case TileContent.Resource:
                    break;
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

        internal IEnumerable<TileContent> CheckCurrentTileContentAround(Map map)
        {
            yield return map.GetTileAt(PlayerInfo.Position.X + 1, PlayerInfo.Position.Y);
            yield return map.GetTileAt(PlayerInfo.Position.X - 1, PlayerInfo.Position.Y);
            yield return map.GetTileAt(PlayerInfo.Position.X, PlayerInfo.Position.Y + 1);
            yield return map.GetTileAt(PlayerInfo.Position.X, PlayerInfo.Position.Y - 1);
        }
    }
}

class TestClass
{
    public string Test { get; set; }
}