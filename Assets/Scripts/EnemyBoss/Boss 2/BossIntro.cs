using System.Collections;
using UnityEngine;

namespace EnemyBoss2
{
    public class BossIntro : AttackPhase<BossPhase2>
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

        public override void EnterState(BossPhase2 _owner)
        {
            stateTimer = 0f;
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
        }
    }
}