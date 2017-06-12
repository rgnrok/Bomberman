using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberman
{
    public class Stroller : Enemy
    {
        protected int xDir;
        protected int yDir;

        [Range(0, 100)]
        public int randomDirectionPercentage = 10;

        protected override void Start() {
            base.Start();
            ChangeDirection();
        }

        protected override void MoveEnemy() {
            if (!canMove) {
                return;
            }

            if (Random.Range(1,100) < randomDirectionPercentage) {
                ChangeDirection();
            }

            //Attempt to move in the selected direction
            if (!AttemptMove(xDir, yDir)) {
                ChangeDirection();
            }
        }

        protected void ChangeDirection() {
            xDir = Random.Range(-1, 2);
            yDir = Random.Range(-1, 2);

            if (xDir != 0) {
                yDir = 0;
            }

            if (xDir == 0 && yDir == 0) {
                xDir = 1;
            }
        }
    }
}