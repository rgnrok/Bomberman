using UnityEngine;

namespace Bomberman
{

    public class MovingObject : MonoBehaviour
    {
        public LayerMask blockingLayer;
        public float speed;

        protected BoxCollider2D boxCollider;
        protected Rigidbody2D rb2D;
        protected Animator animator;
        protected float halfWidth;
        protected float halfHeight;


        protected virtual void Start() {
            boxCollider = GetComponent<BoxCollider2D>();
            halfWidth = boxCollider.size.x / 2;
            halfHeight = boxCollider.size.y / 2;

            rb2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

        }


        protected void UpdateAnimation(int x, int y) {
            if (x > 0) {
                animator.SetTrigger("Right");
            }
            if (x < 0) {
                animator.SetTrigger("Left");
            }
            if (y > 0) {
                animator.SetTrigger("Up");
            }
            if (y < 0) {
                animator.SetTrigger("Down");
            }
        }
    }
}