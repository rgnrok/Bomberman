using System.Collections;
using UnityEngine;

namespace Bomberman
{
    public class Speed : AbstractBonus
    {
        public float speedMultiplier = 2f;
        public float duration = 5;

        protected override void ApplyBonus(Player player) {
            StartCoroutine(IncreaseSpeed(player));
        }

        private IEnumerator IncreaseSpeed(Player player) {
           
            player.speed *= speedMultiplier;
            yield return new WaitForSeconds(duration);
            player.speed /= speedMultiplier;
            Destroy();
        }
    }
}