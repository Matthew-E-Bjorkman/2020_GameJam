using System.Collections;
using UnityEngine;

namespace EnemyBoss
{
    public class HeartAttack : AttackPhase<BossPhase1>
    {
        private static HeartAttack _instance;
        private float stateTimer = 0;
        private float timeSinceLastShot = 0;
        private float minHeight = -3f;
        private float maxHeight = 14f;
        private GameObject heartPrefab;

        private HeartAttack()
        {
            if (_instance != null)
                return;

            _instance = this;
        }

        public static HeartAttack Instance
        {
            get
            {
                if (_instance == null)
                    new HeartAttack();

                return _instance;
            }
        }

        public override void EnterState(BossPhase1 _owner)
        {
            heartPrefab = _owner.GetHeart();
            stateTimer = 0f;
            //Boss Animation
        }

        public override void ExitState(BossPhase1 _owner)
        {
            //Boss Animation
        }

        public override void UpdateState(BossPhase1 _owner)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > 7f)
            {
                _owner.ChangeState();
            }

            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > .5f)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }

        private void Shoot()
        {
            float height = UnityEngine.Random.Range(minHeight, maxHeight);
            Vector3 spawn = new Vector3(69f, height, 0);
            GameObject.Instantiate(heartPrefab, spawn, Quaternion.identity);
        }
    }
}