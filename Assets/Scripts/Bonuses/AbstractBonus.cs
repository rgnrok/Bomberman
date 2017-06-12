using UnityEngine;

namespace Bomberman
{
    public abstract class AbstractBonus : MonoBehaviour, IDestroyable
    {
        protected BoxCollider2D boxCollider;
        protected SpriteRenderer spriteRender;

        abstract protected void ApplyBonus(Player player);

        protected void Awake() {
            boxCollider = GetComponent<BoxCollider2D>();
            spriteRender = GetComponent<SpriteRenderer>();
        }

        public void Destroy() {
            Destroy(gameObject);
        }

        //Hide object and destroy after done coroutines
        public void Use(Player player) {
            boxCollider.enabled = false;
            spriteRender.enabled = false;
            ApplyBonus(player);
        }
    }
}
