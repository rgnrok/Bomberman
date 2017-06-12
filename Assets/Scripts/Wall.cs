using UnityEngine;

namespace Bomberman
{
    public class Wall : MonoBehaviour, IDestroyable
    {
        public GameObject[] bonuses;

        [Range(0, 100)]
        public int spawnBonusAfterDestroyPercentage = 10;


        public void Destroy() {
            if (Random.Range(0, 100) < spawnBonusAfterDestroyPercentage) {
                Instantiate(bonuses[Random.Range(0, bonuses.Length)], transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
