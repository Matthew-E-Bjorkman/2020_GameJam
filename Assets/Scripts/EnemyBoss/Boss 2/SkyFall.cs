using System.Collections;
using UnityEngine;

namespace EnemyBoss2
{
    //Non-Attack state, maybe just a cutesy animation
    public class SkyFall : AttackPhase<BossPhase2>
    {
        private static SkyFall _instance;
        private float stateTimer = 0;
        private float[] row1XSpawns;
        private float[] row2XSpawns;
        private float ySpawn = 15f;
        private float timeSinceLastShot = 0;
        private int spawnRow = 0;
        private GameObject projectilePrefab;

        private SkyFall()
        {
            if (_instance != null)
                return;

            _instance = this;

            row1XSpawns = new float[12];
            for (int i = 48; i < 72; i+=2)
            {
                Debug.Log("Index: " + ((i / 2) - 24));
                row1XSpawns[(i / 2) - 24] = i;
            }
            Debug.Log("row1: " + row1XSpawns.ToString());

            row2XSpawns = new float[11];
            for (int i = 48; i < 70; i += 2)
            {
                row2XSpawns[(i / 2) - 24] = i+1;
            }
            Debug.Log("row1: " + row2XSpawns.ToString());
        }

        public static SkyFall Instance
        {
            get
            {
                if (_instance == null)
                    new SkyFall();

                return _instance;
            }
        }

        public override void EnterState(BossPhase2 _owner)
        {
            projectilePrefab = _owner.GetProjectile();
            stateTimer = 0f;
            //Animation
        }

        public override void ExitState(BossPhase2 _owner)
        {

        }

        public override void UpdateState(BossPhase2 _owner)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > 9f)
            {
                _owner.ChangeState();
            }

            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > .5f)
            {
                Debug.Log("Shooting row " + spawnRow);
                Shoot(spawnRow);
                spawnRow = (spawnRow + 1) % 2;
                timeSinceLastShot = 0f;
            }
        }

        private void Shoot(int spawnRow)
        {
            switch (spawnRow)
            {
                case 0:
                    foreach (float xSpawn in row1XSpawns)
                    {
                        Vector3 spawn = new Vector3(xSpawn, ySpawn, 0);
                        Debug.Log("Row 1 spawn " + xSpawn + ", " + spawn);
                        GameObject.Instantiate(projectilePrefab, spawn, Quaternion.identity);
                    }
                    break;
                case 1:
                    foreach (float xSpawn in row2XSpawns)
                    {
                        Vector3 spawn = new Vector3(xSpawn, ySpawn, 0);
                        Debug.Log("Row 2 spawn " + xSpawn + ", " + spawn);
                        GameObject.Instantiate(projectilePrefab, spawn, Quaternion.identity);
                    }
                    break;
                default:
                    Debug.LogError("SkyFall spawn row invalid index");
                    break;
            }
        }
    }
}