using System.Collections;
using UnityEngine;
using System;

namespace Bomberman
{
    public class Bomb : MonoBehaviour
    {
        public float lifeTime;
        public int power;
        public LayerMask destroyingLayer;
        public LayerMask contactLayer;

        public event EventHandler<EventArgs> OnBang;

        [SerializeField]
        private BangEffect bangEffect;
        private SpriteRenderer spriteRender;

        void Awake() {
            spriteRender = GetComponent<SpriteRenderer>();
        }

        void Start() {
            StartCoroutine(WaitBang());
        }

        public IEnumerator WaitBang() {
            while (lifeTime > 0) {
                lifeTime -= Time.deltaTime;
                yield return null;
            }

            if (OnBang != null) {
                OnBang(this, null);
            }
            
            spriteRender.enabled = false;

            yield return StartCoroutine(bangEffect.Show(power));

            //Sends rays in all directions and destroys objects
            Vector2[] directions = new Vector2[] {
                new Vector2(-power, 0),
                new Vector2(power, 0),
                new Vector2(0, -power),
                new Vector2(0, power),
            };
            RaycastHit2D[] hits;
            for (int i = 0; i < directions.Length; i++) {
                hits = Physics2D.LinecastAll(transform.position, (Vector2)transform.position + directions[i], contactLayer);
                for (int j = 0; j < hits.Length; j++) {
                    IDestroyable obj = hits[j].collider.GetComponent<IDestroyable>();
                    //Checks the location of objects behind an impenetrable wall
                    if (obj == null) {
                        break;
                    }
                    obj.Destroy();
                }

            }
        }
    }

}
