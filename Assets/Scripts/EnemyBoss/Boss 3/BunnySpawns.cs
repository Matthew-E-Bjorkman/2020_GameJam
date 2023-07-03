using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnemyBoss3
{
    public class BunnySpawns : AttackPhase<BossPhase3>
    {
        private static BunnySpawns _instance;
        private float stateTimer = 0;
        private Vector2[] spawnLocations;
        private GameObject[] bunnies;
        private PlayerController player;

        private BunnySpawns()
        {
            if (_instance != null)
                return;

            _instance = this;

            spawnLocations = new Vector2[6];
            spawnLocations[0] = new Vector2(57, 0);
            spawnLocations[1] = new Vector2(52, 3.25f);
            spawnLocations[2] = new Vector2(62, 3.3f);
            spawnLocations[3] = new Vector2(57.2f, 6.24f);
            spawnLocations[4] = new Vector2(52.5f, 9);
            spawnLocations[5] = new Vector2(62, 9.15f);
            bunnies = new GameObject[6];

            player = GameObject.Find("Player").GetComponent<PlayerController>();
            player.PlayerDied.AddListener(KillWithPlayer);
        }

        public static BunnySpawns Instance
        {
            get
            {
                if (_instance == null)
                    new BunnySpawns();

                return _instance;
            }
        }

        public override void EnterState(BossPhase3 _owner)
        {
            stateTimer = 0f;
            int i = 0;
            foreach (Vector2 spawn in spawnLocations)
            {
                GameObject bunnyPrefab = _owner.GetBunny();
                GameObject bunny = GameObject.Instantiate(bunnyPrefab, spawn, Quaternion.identity);
                bunnies[i] = bunny;
                i++;
                GameObject.Destroy(bunny, 12f);
            }
        }

        public override void ExitState(BossPhase3 _owner)
        {

        }

        public override void UpdateState(BossPhase3 _owner)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > 3f)
            {
                _owner.ChangeState();
            }
        }

        private void KillWithPlayer()
        {
            foreach (GameObject bunny in bunnies)
            {
                GameObject.Destroy(bunny);
            }
        }
    }
}
