﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoulderDash2019.Enums;

namespace BoulderDash2019
{
    public abstract class GravitySensitiveEntity : Entity
    {
        bool falling;
        public GravitySensitiveEntity(ref Tile tile) : base(ref tile)
        {
            bool falling = true;
            updateDelay = 5;
        }

        protected bool move(DirectionEnum direction)
        {
            if (falling)
                return false;

            Tile newTile = tile_.MoveToNeighbour(direction, this, ForceEnum.playerPushExtend);
            if (newTile != null)
            {
                tile_ = newTile;
                return true;
            }
            else return false;
        }

        internal override bool Explode()
        {
            tile_ = null;
            return true;
        }

        internal override void Update(int frameUpdate)
        {
            if (!shouldUpdate(frameUpdate))
                return;
            if (!fall())
                stopFalling();
        }

        protected bool fall()
        {
            Tile newPosition;
            if (falling == false)
                newPosition = tile_.MoveToNeighbour(DirectionEnum.Down, this, ForceEnum.StationaryObject);
            else
                newPosition = tile_.MoveToNeighbour(DirectionEnum.Down, this, ForceEnum.FallingObject);
            if (newPosition != null)
            {
                tile_ = newPosition;
                falling = true;
                return true;
            }
            newPosition = tile_.SlideNeighbour(DirectionEnum.Left, this, ForceEnum.StationaryObject);
            if (newPosition != null)
            {
                tile_ = newPosition;
                falling = true;
                return true;
            }
            newPosition = tile_.SlideNeighbour(DirectionEnum.Right, this, ForceEnum.StationaryObject);
            if (newPosition != null)
            {
                tile_ = newPosition;
                falling = true;
                return true;
            }
            return false;
        }

        private void startFalling()
        {
            falling = true;
        }

        internal virtual void stopFalling()
        {
            if (falling == true)
                falling = false;
        }
    }
}
