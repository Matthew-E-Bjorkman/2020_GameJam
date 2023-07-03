using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnemyBoss3
{
    class Vacuum : AttackPhase<BossPhase3>
    {
        private static Vacuum _instance;
        private float stateTimer = 0;
        private GameObject vacuumPrefab;

        private Vacuum()
        {
            if (_instance != null)
                return;

            _instance = this;
        }

        public static Vacuum Instance
        {
            get
            {
                if (_instance == null)
                    new Vacuum();

                return _instance;
            }
        }

        public override void EnterState(BossPhase3 _owner)
        {
            vacuumPrefab = _owner.GetVacuum();
            GameObject vacuum = GameObject.Instantiate(vacuumPrefab);
            int direction = UnityEngine.Random.Range(0, 2);
            //50% chance to flip direction
            if (direction == 1)
            {
                vacuum.GetComponent<VacuumObject>().force = new Vector2(230, 0);
            }
            GameObject.Destroy(vacuum, 6f);
            stateTimer = 0f;
        }

        public override void ExitState(BossPhase3 _owner)
        {

        }

        public override void UpdateState(BossPhase3 _owner)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > 6f)
            {
                _owner.ChangeState();
            }
        }
    }
}
