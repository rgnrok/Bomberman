using UnityEngine;
using UnityEngine.UI;

public class LevelImage : MonoBehaviour {
    [SerializeField]
    private Text levelText;

    public void Show(int level) {
        levelText.text = "Level " + level;
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void GameOver() {
        levelText.text = "Game Over";
        gameObject.SetActive(true);
    }
}
