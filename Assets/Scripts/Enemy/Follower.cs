using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberman
{
    public class Follower : Stroller
    {
        protected Transform target;

        protected override void Start() {
            base.Start();
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        //MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
        protected override void MoveEnemy() {
            if (!canMove || target == null) {
                return;
            }

            if (!VisibleTarget()) {
                base.MoveEnemy();
                return;
            }

            yDir = 0;
            xDir = 0;
            //Move to Player in one direction
            if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) {
                yDir = target.position.y > transform.position.y ? 1 : -1;
            }
            else {
                xDir = target.position.x > transform.position.x ? 1 : -1;
            }

            if (xDir != 0) {
                yDir = 0;
            }

            AttemptMove(xDir, yDir);
        }

        protected bool VisibleTarget() {
            if (Mathf.Abs(target.position.x - transform.position.x) <= 3 || Mathf.Abs(target.position.y - transform.position.y) <= 3) {
                //If ray nothing found
                RaycastHit2D hit = Physics2D.Linecast(transform.position, target.transform.position, blockingLayer);
                if (hit.transform == null) {
                    return true;
                }
            }
            return false;
        }
    }
}