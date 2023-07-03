using System.Collections;
using UnityEngine;

namespace EnemyBoss2
{
    //Non-Attack state, maybe just a cutesy animation
    public class HomingAttack : AttackPhase<BossPhase2>
    {
        private static HomingAttack _instance;
        private float stateTimer = 0;
        private float timeSinceLastShot = 0;
        private GameObject ballPrefab;

        private HomingAttack()
        {
            if (_instance != null)
                return;

            _instance = this;
        }

        public static HomingAttack Instance
        {
            get
            {
                if (_instance == null)
                    new HomingAttack();

                return _instance;
            }
        }

        public override void EnterState(BossPhase2 _owner)
        {
            ballPrefab = _owner.GetHomingBall();
            stateTimer = 0f;
            //Animation
        }

        public override void ExitState(BossPhase2 _owner)
        {

        }

        public override void UpdateState(BossPhase2 _owner)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > 6f)
            {
                _owner.ChangeState();
            }

            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > 2f)
            {
                SpawnBall();
                timeSinceLastShot = 0f;
            }
        }

        private void SpawnBall()
        {
            Vector3 spawn = new Vector3(70f, 0f, 0);
            GameObject.Instantiate(ballPrefab, spawn, Quaternion.identity);
        }
    }
}