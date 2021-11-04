using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class ImpactReceiver : MonoBehaviour
{
    [Inject]
    private CharacterController character;

    [SerializeField]
    private float characterMass;
    [SerializeField]
    private float impactThreshorld = 0.2f;
    [SerializeField]
    private float impactReducer;
    private Vector3 impact = Vector3.zero;


    private void Update()
    {
        if (impact.magnitude > impactThreshorld)
        {
            character.Move(impact * Time.deltaTime);
        }
        impact = Vector3.Lerp(impact, Vector3.zero, Time.deltaTime * impactReducer);
    }

    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0)
        {
            dir.y = -dir.y;
        }
        impact += dir.normalized * force / characterMass;
    }
}
