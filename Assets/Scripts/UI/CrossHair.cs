using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CrossHair : MonoBehaviour
{
    [Inject]
    Camera playerCamera;

    [SerializeField]
    CrossHairColors crosshairColors;
    [SerializeField]
    Image crosshairImage;
    Ray ray;

    void Update()
    {
        ColorCheck();
    }

    void ColorCheck()
    {
        ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction, Color.white);

        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, crosshairColors.crosshairDistance))
        {
            UpdateCrosshairColor(crosshairColors.
                     GetCrosshairColor(raycastHit.collider.gameObject));
        }
        else
        {
            UpdateCrosshairColor(crosshairColors.colorNormal);
        }
    }

    void UpdateCrosshairColor(Color color)
    {
        crosshairImage.color = color;
    }
}
