using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBoss3
{
    public class ComboAttack : AttackPhase<BossPhase3>
    {
        private static ComboAttack _instance;
        private float stateTimer = 0;

        //Laser
        private EdgeCollider2D laser;
        private LineRenderer line;
        private Vector2 attackOrigin;
        private Vector2 target;

        //Skyfall
        private float[] row1XSpawns;
        private float[] row2XSpawns;
        private float ySpawn = 15f;
        private float timeSinceLastShot = 0;
        private int spawnRow = 0;
        private GameObject projectilePrefab;

        private ComboAttack()
        {
            if (_instance != null)
                return;

            _instance = this;

            row1XSpawns = new float[12];
            for (int i = 48; i < 72; i += 2)
            {
                Debug.Log("Index: " + ((i / 2) - 24));
                row1XSpawns[(i / 2) - 24] = i;
            }
            Debug.Log("row1: " + row1XSpawns.ToString());

            row2XSpawns = new float[11];
            for (int i = 48; i < 70; i += 2)
            {
                row2XSpawns[(i / 2) - 24] = i + 1;
            }
            Debug.Log("row1: " + row2XSpawns.ToString());
        }

        public static ComboAttack Instance
        {
            get
            {
                if (_instance == null)
                    new ComboAttack();

                return _instance;
            }
        }

        public override void EnterState(BossPhase3 _owner)
        {
            stateTimer = 0f;

            //Laser Portion
            laser = GameObject.Find("LaserOrigin").GetComponent<EdgeCollider2D>();
            line = GameObject.Find("LaserOrigin").GetComponent<LineRenderer>();

            //Charge-up Animation
            CalulateTarget(_owner);

            //Draw a line
            Vector3 lineVector = new Vector3(target.x, target.y, 2);
            
            line.endWidth = 0.5f;
            line.startWidth = 0.5f;
            line.SetPosition(1, lineVector);

            //Skyfall
            projectilePrefab = _owner.GetProjectile();
        }

        public override void ExitState(BossPhase3 _owner)
        {
            //Laser
            List<Vector2> vertices = new List<Vector2>();
            vertices.Add(new Vector2(0, 0));
            vertices.Add(new Vector2(0, 0));
            laser.points = vertices.ToArray();
            Vector3 zeroVector = new Vector3(0, 0, 2);
            line.SetPosition(0, zeroVector);
            line.SetPosition(1, zeroVector);
        }

        public override void UpdateState(BossPhase3 _owner)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > 6f)
            {
                _owner.ChangeState();
            }
            
            line.endWidth = Mathf.Lerp(line.endWidth, 6, 4 * Time.deltaTime);
            line.startWidth = Mathf.Lerp(line.startWidth, 6, 4 * Time.deltaTime);
            
            //Laser
            if (stateTimer > 2f)
            {
                //Create Collider
                List<Vector2> vertices = new List<Vector2>();
                vertices.Add(target);
                vertices.Add(new Vector2(0, 0));
                laser.points = vertices.ToArray();
            }

            //Skyfall
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > .5f && stateTimer < 3f)
            {
                Debug.Log("Shooting row " + spawnRow);
                Shoot(spawnRow);
                spawnRow = (spawnRow + 1) % 2;
                timeSinceLastShot = 0f;
            }
        }

        //laser
        private void CalulateTarget(BossPhase3 _owner)
        {
            attackOrigin = GameObject.Find("LaserOrigin").transform.position;

            target.x = _owner.transform.position.x - _owner.GetPlayer().transform.position.x;
            target.y = -4 + 2 * (Math.Abs(_owner.transform.position.y - _owner.GetPlayer().transform.position.y) + Math.Abs(_owner.transform.position.y - attackOrigin.y));

            if ((attackOrigin.x + _owner.transform.position.x) - _owner.GetPlayer().transform.position.x > 0) target.x *= -1;
            if ((attackOrigin.y + _owner.transform.position.y) - _owner.GetPlayer().transform.position.y > 0) target.y *= -1;

            target *= 5;
        }

        //Skyfall
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