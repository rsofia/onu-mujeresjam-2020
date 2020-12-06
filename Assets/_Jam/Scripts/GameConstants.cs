using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
    public const string characterTag = "Character";
    public static LayerMask characterMask = LayerMask.NameToLayer("People");
    public const string floorTag = "Floor";
    public const string obstacleTag = "Obstacle";
}
