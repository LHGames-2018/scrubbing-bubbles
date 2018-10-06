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

        /***** PATHFINDING ******/
        private bool arrivedAtDestination;

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
            if (PlayerInfo.CarriedResources >= PlayerInfo.CarryingCapacity)
            {
                return WalkTowardsTile(PlayerInfo.HouseLocation, map, true); // x + 1 because reasons
            }

            int[] mineralDirection = MineralAdjacentDirection(map);

            if (mineralDirection != null)
            {
                return AIHelper.CreateCollectAction(new Point(mineralDirection[0], mineralDirection[1]));
            }

            else
            {
                var resource = LookForVisibleResource(map);

                if (resource != null)
                {
                    return WalkTowardsTile(new Point(resource.X, resource.Y), map, false);
                }
                // randomly walk
                else
                {
                    if (map.GetTileAt(PlayerInfo.Position.X + _currentDirection, PlayerInfo.Position.Y) ==
                        TileContent.Wall)
                    {
                        _currentDirection *= -1;
                        distanceTravelled = 0;
                        moving = false;
                        previousDirection = INVALID_DIRECTION;
                    }

                    if (movingRandom)
                    {
                        return MoveRandomly(map);
                    }
                }
            }

            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            return AIHelper.CreateMoveAction(new Point(0, -1));
        }

        internal string MoveRandomly(Map map)
        {

            if (!moving)
            {
                Random rnd = new Random();
                randomDirection = rnd.Next(1, 5);
                randomDistance = rnd.Next(1, MAX_RANDOM_DISTANCE);
                moving = true;

                while (randomDirection == previousDirection)
                {
                    randomDirection = rnd.Next(1, 5);
                }
            }

            if (distanceTravelled >= randomDistance)
            {
                moving = false;
                distanceTravelled = 0;
            }

            if (moving)
            {
                Console.WriteLine("moving");
                switch (randomDirection)
                {
                    case 1:
                        if (MeleeTargetExists(map, 1, 0))
                            return AIHelper.CreateMeleeAttackAction(new Point(1, 0));
                        return AIHelper.CreateMoveAction(new Point(1, 0));
                    case 2:
                        if (MeleeTargetExists(map, -1, 0))
                            return AIHelper.CreateMeleeAttackAction(new Point(-1, 0));
                        return AIHelper.CreateMoveAction(new Point(-1, 0));
                    case 3:
                        if (MeleeTargetExists(map, 0, 1))
                            return AIHelper.CreateMeleeAttackAction(new Point(0, 1));
                        return AIHelper.CreateMoveAction(new Point(0, 1));
                    case 4:
                        if (MeleeTargetExists(map, 0, -1))
                            return AIHelper.CreateMeleeAttackAction(new Point(0, -1));
                        return AIHelper.CreateMoveAction(new Point(0, -1));
                }

                distanceTravelled++;
                Console.WriteLine("distanceTravelled: ", distanceTravelled);
            }

            if (distanceTravelled >= randomDistance)
            {
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
            return (map.GetTileAt(PlayerInfo.Position.X + directionX, PlayerInfo.Position.Y + directionY) ==
                    TileContent.Wall ||
                    map.GetTileAt(PlayerInfo.Position.X + directionX, PlayerInfo.Position.Y + directionY) ==
                    TileContent.Player);
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

        internal Point LookForVisibleResource(Map map)
        {
            try
            {
                var allTiles = map.GetVisibleTiles();
                var closestTile = map.GetVisibleTiles().First(x => x.TileType.Equals(TileContent.Resource));
                int closestDistance = Math.Abs(closestTile.Position.X - PlayerInfo.Position.X) + Math.Abs(closestTile.Position.Y - PlayerInfo.Position.Y);
                foreach (var tile in allTiles)
                {
                    if(tile.TileType.Equals(TileContent.Resource))
                    {
                        int tileDistance = Math.Abs(tile.Position.X - PlayerInfo.Position.X) + Math.Abs(tile.Position.Y - PlayerInfo.Position.Y);
                        if (tileDistance < closestDistance)
                        {
                            closestDistance = tileDistance;
                            closestTile = tile;
                        }
                    }
                }
                return closestTile.Position;
            }
            catch
            {
                return null;
                //do nothing
            }
        }

        /// <summary>
        /// Walk right next to a tile
        /// </summary>
        internal string WalkTowardsTile(Point tilePosition, Map map, bool isReturningHome)
        {
            var deltaX = tilePosition.X - PlayerInfo.Position.X;
            var deltaY = tilePosition.Y - PlayerInfo.Position.Y;

            if (!isReturningHome)
            {
                if (deltaX < 0)
                    deltaX += 1;
                else
                    deltaX -= 1;
            }

            //bouger personnage
            if (deltaX != 0)
            {
                var direction = deltaX > 0 ? 1 : -1; // UGLY
                if (map.GetTileAt(PlayerInfo.Position.X + direction, PlayerInfo.Position.Y) ==
                    TileContent.Wall)
                {
                    return AIHelper.CreateMeleeAttackAction(new Point(direction));
                }
                else
                {
                    return AIHelper.CreateMoveAction(new Point(direction));
                }
            }

            if (deltaY != 0)
            {
                var direction = deltaY > 0 ? 1 : -1; // UGLY
                if (map.GetTileAt(PlayerInfo.Position.X + direction, PlayerInfo.Position.Y) ==
                    TileContent.Wall)
                {
                    return AIHelper.CreateMeleeAttackAction(new Point(0, direction));
                }
                else
                {
                    return AIHelper.CreateMoveAction(new Point(0, direction));

                }

            }

            arrivedAtDestination = true;
            return AIHelper.CreateEmptyAction();
        }
    }
}

class TestClass
{
    public string Test { get; set; }
}