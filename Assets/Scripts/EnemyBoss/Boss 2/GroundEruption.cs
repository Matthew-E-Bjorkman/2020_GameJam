using System.Collections;
using UnityEngine;

namespace EnemyBoss2
{
    //Non-Attack state, maybe just a cutesy animation
    public class GroundEruption : AttackPhase<BossPhase2>
    {        
        private static GroundEruption _instance;
        private float stateTimer = 0;
        private float slowMoveSpeed = 2f;
        private float fastMoveSpeed = 12f;
        private GameObject eruptionPrefab;
        private Rigidbody2D rb2d;

        private GroundEruption()
        {
            if (_instance != null)
                return;

            _instance = this;
        }

        public static GroundEruption Instance
        {
            get
            {
                if (_instance == null)
                    new GroundEruption();

                return _instance;
            }
        }

        public override void EnterState(BossPhase2 _owner)
        {
            eruptionPrefab = _owner.GetEruption();
            GameObject eruption = GameObject.Instantiate(eruptionPrefab);
            GameObject.Destroy(eruption, 3.1f);
            rb2d = eruption.GetComponent<Rigidbody2D>();
            rb2d.velocity = new Vector2(0, slowMoveSpeed);
            stateTimer = 0f;
            //Animation
        }

        public override void ExitState(BossPhase2 _owner)
        {

        }

        public override void UpdateState(BossPhase2 _owner)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > 3.1f)
            {
                _owner.ChangeState();
            }

            if (stateTimer > 2f)
            {
                rb2d.velocity = new Vector2(0, fastMoveSpeed);
            }
            //0f slow rise, this part wont have collider
            //~1f fast rise, this part has collider
        }
    }
}