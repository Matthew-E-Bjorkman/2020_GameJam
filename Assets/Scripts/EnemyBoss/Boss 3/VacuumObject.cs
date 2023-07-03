using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnemyBoss3
{
    class VacuumObject : MonoBehaviour
    {
        public Vector2 force;

        private Collider2D playerCollider;
        
        void FixedUpdate()
        {
            if (playerCollider != null)
            {
                Debug.Log("Pushing");
                Rigidbody2D rb2d = playerCollider.attachedRigidbody;
                Debug.Log(rb2d);
                Debug.Log(force);
                rb2d.AddForce(force);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "player") playerCollider = other;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "player") playerCollider = null;
        }
    }
}
