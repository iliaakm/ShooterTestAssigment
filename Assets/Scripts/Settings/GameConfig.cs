using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfig
{
    public static class Axis
    {
        public const string axisFire = "Fire1";
        public const string axisMoveVertical = "Vertical";
        public const string AxisMoveHorizontal = "Horizontal";
        public const string AxisMouseHorizontal = "Mouse X";
        public const string AxisMouseVertical = "Mouse Y";
        public const string AxisJump = "Jump";
    }

    public static class ZenjectConfig
    {
        public const string playerTransform = "PlayerTransform";
        public const string loseTrigger = "LoseTrigger";
        public const string wonTrigger = "WonTrigger";

    }

    public static class Animation
    {
        public const string cannonAnimatorReloading = "Reload";
    }

    public static class  Layers
    {
        public const int layerDefault = 0;
        public const int layerOutline = 8;
    }
}