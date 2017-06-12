using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangEffect : MonoBehaviour {

    public float effectLifeTime;

    [SerializeField]
    private ParticleSystem horizontalEffect;
    [SerializeField]
    private ParticleSystem verticalEffect;
    
    void Awake () {
        horizontalEffect.gameObject.SetActive(false);
        verticalEffect.gameObject.SetActive(false);
    }

    public IEnumerator Show(int power) {

        horizontalEffect.startLifetime = power*0.5f;
        verticalEffect.startLifetime = power* 0.5f;

        horizontalEffect.gameObject.SetActive(true);
        verticalEffect.gameObject.SetActive(true);

        yield return new WaitForSeconds(effectLifeTime * power);

        horizontalEffect.gameObject.SetActive(false);
        verticalEffect.gameObject.SetActive(false);
    }
}
