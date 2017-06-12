using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberman
{
    public abstract class Enemy : MovingObject, IDestroyable
    {
        public event EventHandler OnDie;
        protected bool canMove = true;

        protected override void Start() {
            base.Start();
            GameManager.instance.AddEnemy(this);
        }

        void Update() {
            MoveEnemy();
        }


        //Implementing the logic of enemy behavior
        protected abstract void MoveEnemy();


        protected IEnumerator SmoothMovement(Vector3 end) {
            canMove = false;
            //Calculate the remaining distance to move
            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            while (sqrRemainingDistance > float.Epsilon) {
                //Find a new position proportionally closer to the end, based on the moveTime
                Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, speed * Time.deltaTime);

                rb2D.MovePosition(newPostion);
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;

                yield return null;
            }
            canMove = true;
        }

        protected bool AttemptMove(int xDir, int yDir) {
            if (xDir == 0 && yDir == 0) {
                return true;
            }

            Vector2 start = transform.position;
            Vector2 end = start + new Vector2(xDir, yDir); ;
            UpdateAnimation(xDir, yDir);

            //Disable collider because linecast hit self
            boxCollider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);
            
            boxCollider.enabled = true;

            if (hit.transform == null) {
                StartCoroutine(SmoothMovement(end));
                return true;
            }

            return false;
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Bonus") {
                Destroy(other.gameObject);
            }
        }


        public void Destroy() {
            if (OnDie != null) {
                OnDie(this, null);
            }
            Destroy(gameObject);
        }
    }
}
