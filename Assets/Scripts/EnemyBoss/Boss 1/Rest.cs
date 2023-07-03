using System.Collections;
using UnityEngine;

namespace EnemyBoss
{
    //Non-Attack state, maybe just a cutesy animation
    public class Rest : AttackPhase<BossPhase1>
    {
        private static Rest _instance;
        private float stateTimer = 0;

        private Rest()
        {
            if (_instance != null)
                return;

            _instance = this;
        }

        public static Rest Instance
        {
            get
            {
                if (_instance == null)
                    new Rest();

                return _instance;
            }
        }

        public override void EnterState(BossPhase1 _owner)
        {
            stateTimer = 0f;
            //Animation
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