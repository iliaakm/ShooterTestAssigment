using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    public void OutlineOn()
    {
        gameObject.layer = GameConfig.Layers.layerOutline;       
    }

    public void OutlineOff()
    {
        gameObject.layer = GameConfig.Layers.layerDefault;       
    }
}
