using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CrosshairColors")]
public class CrosshairColors : ScriptableObject
{
    public Color colorNormal;
    public Color colorEnemy;

    public float crosshairDistance;

    public Color GetCrosshairColor(GameObject target)
    {
        if (target.GetComponent<Enemy>())
        {
            return colorEnemy;
        }
        return colorNormal;
    }
}
