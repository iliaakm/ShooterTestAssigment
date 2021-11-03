using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    Camera playerCamera;
    [SerializeField]
    Image crossHair;
    [SerializeField]
    CrosshairColors crosshairColors;

    Ray ray;
    RaycastHit raycastHit;

    void Update()
    {
        ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if(Physics.Raycast(ray, out raycastHit, crosshairColors.crosshairDistance))
        {
             UpdateCrosshairColor(crosshairColors.
                 GetCrosshairColor(raycastHit.collider.gameObject));
        }
    }
    void UpdateCrosshairColor(Color color)
    {
        crossHair.color = color;
    }
}
