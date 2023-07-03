using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBoss
{
    //Locks onto player at start of the attack and fires a laser after a delay
    public class LaserAttack : AttackPhase<BossPhase1>
    {
        private static LaserAttack _instance;
        private float stateTimer = 0;
        private EdgeCollider2D laser;
        private LineRenderer line;
        private Vector2 attackOrigin;
        private Vector2 target; 


        private LaserAttack()
        {
            if (_instance != null)
                return;

            _instance = this;
        }

        public static LaserAttack Instance
        {
            get
            {
                if (_instance == null)
                    new LaserAttack();

                return _instance;
            }
        }

        public override void EnterState(BossPhase1 _owner)
        {
            stateTimer = 0f;
            laser = GameObject.Find("LaserOrigin").GetComponent<EdgeCollider2D>();
            line = GameObject.Find("LaserOrigin").GetComponent<LineRenderer>();

            //Charge-up Animation
            CalulateTarget(_owner);

            //Draw a line
            Vector3 lineVector = new Vector3(target.x, target.y, 1);
            line.endWidth = 0.5f;
            line.startWidth = 0.5f;
            line.SetPosition(1, lineVector);
        }

        public override void ExitState(BossPhase1 _owner)
        {
            List<Vector2> vertices = new List<Vector2>();
            vertices.Add(new Vector2(0, 0));
            vertices.Add(new Vector2(0, 0));
            laser.points = vertices.ToArray();
            Vector3 zeroVector = new Vector3(0, 0, 1);
            line.SetPosition(1, zeroVector);
        }

        public override void UpdateState(BossPhase1 _owner)
        {
            stateTimer += Time.deltaTime;

            line.endWidth = Mathf.Lerp(line.endWidth, 6, 4 * Time.deltaTime);
            line.startWidth = Mathf.Lerp(line.startWidth, 6, 4 * Time.deltaTime);

            if (stateTimer > 1f)
            {
                //Create Collider
                List<Vector2> vertices = new List<Vector2>();
                vertices.Add(target);
                vertices.Add(new Vector2(0,0));
                laser.points = vertices.ToArray();
                
                // line.endWidth = 6f;
                // line.startWidth = 6f;
            }


            if (stateTimer > 6f)
            {
                _owner.ChangeState();
            }
        }

        private void CalulateTarget(BossPhase1 _owner)
        {
            attackOrigin = GameObject.Find("LaserOrigin").transform.position;

            target.x = _owner.transform.position.x - _owner.GetPlayer().transform.position.x;
            target.y = -4 + 2 * (Math.Abs(_owner.transform.position.y - _owner.GetPlayer().transform.position.y) + Math.Abs(_owner.transform.position.y - attackOrigin.y));

            if ((attackOrigin.x + _owner.transform.position.x) - _owner.GetPlayer().transform.position.x > 0) target.x *= -1;
            if ((attackOrigin.y + _owner.transform.position.y) - _owner.GetPlayer().transform.position.y > 0) target.y *= -1;

            target *= 5;
        }
    }
}