using System.Collections;
using UnityEngine;

public class Countdown : MonoBehaviour {

    private Coroutine co;

    // Use this for initialization
    void Start () {
        StartCountdown();
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {

        GameManager.instance.timerText.text = ("Time: " + GameManager.instance.timeLeft);

        if(GameManager.instance.GamePaused || GameManager.instance.CountdownPaused) {
            StopCoroutine(co);
        }

        if(GameManager.instance.GameUnpaused || GameManager.instance.CountdownUnpaused) {
            StartCountdown();
        }
		
	}

    //game timer
    public void StartCountdown() {
        co = StartCoroutine(LoseTime());
    }

    //game timer coroutine
    IEnumerator LoseTime() {
        while (true) {
            yield return new WaitForSeconds(1);
            if (GameManager.instance.timeLeft >= 1) {
                GameManager.instance.timeLeft--;
            }
            if (GameManager.instance.timeLeft == 0) {
                GameManager.instance.GameIsOver();
            }
        }
    }
}
