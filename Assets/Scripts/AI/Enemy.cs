using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EnemyAI
{
    public enum DirectPlayerVisibility
    {
        Visible,
        Invisible
    }

    public enum ShootRange
    {
        InRange,
        TooFar
    }

    public enum AliveStatus
    {
        Alive,
        Dead
    }

    public class Enemy : MonoBehaviour
    {   
        [Inject(Id = GameConfig.ZenjectConfig.playerTransform)]
        readonly Transform playerTransform;
        [Inject]
        ImpactReceiver playerImpactReceiver;

        [SerializeField]
        float shootRange;
        [SerializeField]
        float shootFireRatePerMin;
        [SerializeField]
        float shootImpact;
        [SerializeField, Range(0f, 1f)]
        float shootAccuracy;
        [SerializeField]
        float shotTracerTime;
        [SerializeField]
        AnimationCurve dissolveCurve;       
        [SerializeField]
        LineRenderer shotTracer;
        [SerializeField]
        DamageReceiver damageReceiver;
        [SerializeField]
        Outline outline;

        public ShootRange ShootRangeState { get; set; }
        public AliveStatus AliveStatusState { get; set; }

        float timeBetweenShots;

        DirectPlayerVisibility _directPlayerVisibilityState;
        DirectPlayerVisibility DirectPlayerVisibilityState
        {
            get { return _directPlayerVisibilityState; }
            set
            {
                _directPlayerVisibilityState = value;
                if (_directPlayerVisibilityState == DirectPlayerVisibility.Invisible)
                {
                    outline.OutlineOn();
                }
                if (_directPlayerVisibilityState == DirectPlayerVisibility.Visible)
                {
                    outline.OutlineOff();
                }
            }
        }


        private void Start()
        {
            DirectPlayerVisibilityState = DirectPlayerVisibility.Invisible;
            ShootRangeState = ShootRange.TooFar;
            AliveStatusState = AliveStatus.Alive;          

            if (damageReceiver.onDeath == null)
                damageReceiver.onDeath = new UnityEngine.Events.UnityEvent();
            damageReceiver.onDeath.AddListener(Death);

            timeBetweenShots = 60f / shootFireRatePerMin;
            StartCoroutine(ShootLoopCor());
        }

        private void FixedUpdate()
        {
            if (AliveStatusState == AliveStatus.Dead)
                return;

            CheckVisible();
        }

        private void ShootToPlayer()
        {
            float hitChance = Random.Range(0f, 1f);
            if (hitChance < shootAccuracy)
            {
                if (DirectPlayerVisibilityState == DirectPlayerVisibility.Visible)
                    if (ShootRangeState == ShootRange.InRange)
                    {
                        Vector3 pushDirection = playerTransform.position - transform.position;
                        HitPlayer(pushDirection);
                        StartCoroutine(DrawShotTrailCor());
                    }
            }
        }

        IEnumerator ShootLoopCor()
        {
            while (true)
            {
                if (AliveStatusState == AliveStatus.Dead)
                    yield break;

                ShootToPlayer();

                yield return new WaitForSeconds(timeBetweenShots);
            }
        }

        void HitPlayer(Vector3 direction)
        {
            playerImpactReceiver.AddImpact(direction, shootImpact);
            Debug.Log("Hit player", this);
        }

        private void OnDestroy()
        {
            StopCoroutine(ShootLoopCor());
        }

        void CheckVisible()
        {
            DirectPlayerVisibilityState = DirectPlayerVisibility.Invisible;
            ShootRangeState = ShootRange.TooFar;

            Ray ray = new Ray(this.transform.position, playerTransform.position - this.transform.position);
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == playerTransform)
                {
                    DirectPlayerVisibilityState = DirectPlayerVisibility.Visible;
                    if (raycastHit.distance < shootRange)
                    {
                        ShootRangeState = ShootRange.InRange;
                    }
                }
            }
        }

        IEnumerator DrawShotTrailCor()
        {
            shotTracer.SetPosition(0, transform.position);
            shotTracer.SetPosition(1, playerTransform.position);
            shotTracer.enabled = true;

            yield return new WaitForSeconds(shotTracerTime);

            shotTracer.enabled = false;
        }


        void Death()
        {
            if(AliveStatusState == AliveStatus.Dead)
            {
                return;
            }    

            AliveStatusState = AliveStatus.Dead;
            DirectPlayerVisibilityState = DirectPlayerVisibility.Visible;
            StartCoroutine(DeathCor());
        }

        IEnumerator DeathCor()
        {
            Material material = GetComponent<Renderer>().material;
            float alpha = 0;
            float time = 0;

            while (time < 1)
            {
                alpha = dissolveCurve.Evaluate(time);
                material.SetFloat(GameConfig.Animation.dissolveParameter, alpha);
                time += Time.deltaTime;

                yield return new WaitForSeconds(Time.deltaTime);
            }

            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootRange);
        }
    }
}