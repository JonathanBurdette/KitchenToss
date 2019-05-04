using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLobby : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    //loads lobby from menu
    public void LoadLobbyFromMenu() {
        StartCoroutine(LoadFromMenu());
    }

    //coroutine for loading lobby from menu
    IEnumerator LoadFromMenu() {
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene("Game");
    }

}
