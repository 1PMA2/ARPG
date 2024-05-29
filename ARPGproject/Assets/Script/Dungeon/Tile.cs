using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    void SetWall(Vector2Int dir);
    void OpenWall(Vector2Int dir);

}

