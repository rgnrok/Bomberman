using System.Collections;
using UnityEngine;

namespace Bomberman
{
    public class Power : AbstractBonus
    {
        public int powerMultiplier = 2;
        public float duration = 5;

        protected override void ApplyBonus(Player player) {
            StartCoroutine(IncreasePower(player));
        }

        private IEnumerator IncreasePower(Player player) {
            player.bombPower *= powerMultiplier;
            yield return new WaitForSeconds(duration);
            player.bombPower /= powerMultiplier;
            Destroy();
        }
    }
}