using UnityEngine;

namespace Bomberman
{
    public class AdditionalBomb : AbstractBonus
    {
        protected override void ApplyBonus(Player player) {
            player.MaxBombs++;
            Destroy();
        }
    }
}