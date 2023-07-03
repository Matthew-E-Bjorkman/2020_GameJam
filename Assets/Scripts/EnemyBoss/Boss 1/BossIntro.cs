using System.Collections;
using UnityEngine;

namespace EnemyBoss
{
    public class BossIntro : AttackPhase<BossPhase1>
    {
        private static BossIntro _instance;
        private float stateTimer = 0;

        private BossIntro()
        {
            if (_instance != null)
                return;

            _instance = this;
        }

        public static BossIntro Instance
        {
            get
            {
                if (_instance == null)
                    new BossIntro();

                return _instance;
            }
        }

        public override void EnterState(BossPhase1 _owner)
        {
            stateTimer = 0f;
        }

        public override void ExitState(BossPhase1 _owner)
        {

        }

        public override void UpdateState(BossPhase1 _owner)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > 6f)
            {
                _owner.ChangeState();
            }
        }
    }
}