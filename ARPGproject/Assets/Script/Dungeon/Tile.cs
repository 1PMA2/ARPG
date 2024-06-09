using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public interface ITile
    {

        enum TileType
    {
            NONE,
            MARBLE,
            BOX,
            END
        }

        void SetWall(Vector2Int dir);
        void OpenWall(Vector2Int dir);
        void CloseDoor(Vector2Int dir);
        void OpenDoor();
        void SetTile(TileType tileType);
        void Reverse();

    }


