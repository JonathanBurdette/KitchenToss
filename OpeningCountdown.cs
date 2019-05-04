using System.Collections;
using UnityEngine;

public class OpeningCountdown : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
        StartCoroutine(WaitForGameStart());
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update () {

    }

    //3 second countdown before game begins
    IEnumerator WaitForGameStart() {
        while (true) {
            yield return new WaitForSeconds(1);

            if (GameManager.instance.countdown >= 1) {
                GameManager.instance.countdown--;
                GameManager.instance.countdownText.text = GameManager.instance.countdown.ToString();
            }

            if (GameManager.instance.countdown == 0) {
                GameManager.instance.countdownText.text = "GO!";
                GameManager.instance.scoreObj.SetActive(true);
                GameManager.instance.timerObj.SetActive(true);
                GameManager.instance.pauseBtnObj.SetActive(true);
                GameManager.instance.restartBtnObj.SetActive(true);
            }

        }
    }
}
