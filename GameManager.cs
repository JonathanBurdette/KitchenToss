using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip sfxGameOver;

    //in-game buttons
    public GameObject scoreObj;
    public GameObject timerObj;
    public GameObject playBtnObj;
    public GameObject pauseBtnObj;
    public GameObject restartBtnObj;

    //menus
    public GameObject gameOverObj;
    public GameObject pauseMenuObj;
    public GameObject lobbyObj;
    public GameObject countdownObj;

    public Text scoreText;
    public Text timerText;
    public Text finalScoreText;
    public Text fruitsTossedText;
    public Text ceilingHitsText;
    public Text countdownText;

    public int score = 0;
    public int fruitCount = 0;
    public int ceilingCount = 0;
    public int timeLeft = 60;
    public int countdown = 3;

    public bool playerActive = false;
    public bool gameOver = false;
    public bool gamePaused = false;
    public bool pauseCountdown = false;
    private bool timeOneSelected = false;
    private bool timeTwoSelected = false;
    private bool timeThreeSelected = false;
    private bool timeFourSelected = false;

    public List<GameObject> fruitList = new List<GameObject>();

    //if true, player is active
    public bool PlayerActive {
        get {
            return playerActive;
        }
    }

    //if false, player is inactive
    public bool PlayerInactive {
        get {
            return playerActive;
        }
    }

    //if true, game is paused
    public bool GamePaused {
        get {
            return gamePaused;
        }
    }

    //if false, game is unpaused
    public bool GameUnpaused {
        get {
            return gamePaused;
        }
    }

    //if true, game is over
    public bool GameOver {
        get {
            return gameOver;
        }
    }

    //if false, game is not over
    public bool GameNotOver {
        get {
            return gameOver;
        }
    }

    //if true countdown is paused
    public bool CountdownPaused {
        get {
            return pauseCountdown;
        }
    }

    //if false countdown is unpaused
    public bool CountdownUnpaused {
        get {
            return pauseCountdown;
        }
    }

    //keeps game manager intact when loading scene
    private void Awake() {

        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start() {
        audioSource = GetComponent<AudioSource>();
        scoreText.text = "Score: 0";
        timerText.text = "Time: " + timeLeft.ToString();
        countdownText.text = "3";
    }

    // Update is called once per frame
    void Update () {
        finalScoreText.text = score.ToString();
        ceilingHitsText.text = ceilingCount.ToString();
        fruitsTossedText.text = fruitCount.ToString();
    }

    //30 sec game option
    public void TimeOne() {
        timeOneSelected = true;
        timeTwoSelected = false;
        timeThreeSelected = false;
        timeFourSelected = false;
        LoadGame();
    }

    //1 min game
    public void TimeTwo() {
        timeOneSelected = false;
        timeTwoSelected = true;
        timeThreeSelected = false;
        timeFourSelected = false;
        LoadGame();
    }

    //1.5 min game
    public void TimeThree() {
        timeOneSelected = false;
        timeTwoSelected = false;
        timeThreeSelected = true;
        timeFourSelected = false;
        LoadGame();
    }

    //2 min game
    public void TimeFour() {
        timeOneSelected = false;
        timeTwoSelected = false;
        timeThreeSelected = false;
        timeFourSelected = true;
        LoadGame();
    }

    public void LoadGame() {
        StartCoroutine(LoadNewGame());
    }

    IEnumerator LoadNewGame() {
        yield return new WaitForSeconds(0.15f);
        lobbyObj.SetActive(false);
        TimeReset();
        ResetStats();

        ShowCountdownText();
        HideButtons();
        if(GameOver) {
            gameOverObj.SetActive(false);
        }
        if(GamePaused) {
            pauseMenuObj.SetActive(false);
        }
        Invoke("StartGameActivity", 3);
        Invoke("HideCountdownText", 4);
    }

    //sets time based on button pressed
    public void TimeReset() {
        if (timeOneSelected == true) {
            timeLeft = 30;
        }
        else if (timeTwoSelected == true) {
            timeLeft = 60;
        }
        else if (timeThreeSelected == true) {
            timeLeft = 90;
        }
        else if (timeFourSelected == true) {
            timeLeft = 120;
        }
    }

    //resets stats to default
    public void ResetStats() {
        scoreText.text = "Score: 0";
        score = 0;
        fruitCount = 0;
        ceilingCount = 0;
        countdownText.text = "3";
        countdown = 3;
    }

    //shows the countdown text
    public void ShowCountdownText() {
        countdownObj.SetActive(true);
        GetComponent<OpeningCountdown>().enabled = true;
    }
    
    //hides in-game buttons
    public void HideButtons() {
        scoreObj.SetActive(false);
        timerObj.SetActive(false);
        restartBtnObj.SetActive(false);
        pauseBtnObj.SetActive(false);
        playBtnObj.SetActive(false);
    }

    //makes player active and starts opening countdown
    public void StartGameActivity() {
        PlayerStartedGame();
        GetComponent<Countdown>().enabled = true;
        UnpauseGame();
        GameIsNotOver();
    }

    //hides the countdown text
    public void HideCountdownText() {
        countdownObj.SetActive(false);
    }

    //spawns a random fruit out of list
    public void SpawnFruit() {
        int prefabIndex = Random.Range(0, fruitList.Count);
        Instantiate(fruitList[prefabIndex]);
    }
    
    //determines score based on height
    public void UpdateScore(float newScore) {
        if (newScore >= 3 && newScore <= 3.17) {
            score = score + 1;
        } else if (newScore > 3.17 && newScore <= 3.34) {
            score = score + 2;
        } else if (newScore > 3.34 && newScore <= 3.51) {
            score = score + 3;
        } else if (newScore > 3.51 && newScore <= 3.68) {
            score = score + 4;
        } else if (newScore > 3.68 && newScore <= 3.85) {
            score = score + 5;
        } else if (newScore > 3.85 && newScore <= 4.02) {
            score = score + 6;
        } else if (newScore > 4.02 && newScore <= 4.19) {
            score = score + 12;
        } else if (newScore > 4.19 && newScore <= 4.36) {
            score = score + 24;
        } else if (newScore > 4.36 && newScore <= 4.53) {
            score = score + 48;
        } else if (newScore > 4.53 && newScore <= 5) {
            score = score + 96;
        }
        scoreText.text = "Score: " + score.ToString();
    }

    //increments count of fruits tossed
    public void UpdateFruitsTossed() {
        fruitCount++;
    }

    //increments count of fruit that hit the ceiling
    public void UpdateCeilingHits() {
        ceilingCount++;
    }

    //sets player active
    public void PlayerStartedGame() {
        playerActive = true;
    }

    //player is inactive
    public void PlayerEndedGame() {
        playerActive = false;
    }

    //stops countdown timer
    public void PauseCountdown() {
        pauseCountdown = true;
    }

    //starts countdown timer
    public void UnpauseCountdown() {
        pauseCountdown = false;
    }

    //game is paused
    public void PauseGame() {
        if (!GameOver) {
            gamePaused = true;
            PauseCountdown();
            playBtnObj.SetActive(true);
            pauseMenuObj.SetActive(true);
            PlayerEndedGame();
        }
    }
    
    //game is unpaused
    public void UnpauseGame() {
        StartCoroutine(PlayBtnDelay());
    }

    //allows for button sound
    IEnumerator PlayBtnDelay() {
        yield return new WaitForSeconds(0.15f);
        gamePaused = false;
        UnpauseCountdown();
        playBtnObj.SetActive(false);
        pauseMenuObj.SetActive(false);
        PlayerStartedGame();
    }
    
    //restarts game when restart button is pressed
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResetStats();
        TimeReset();
        if(GameOver) {
            GameIsNotOver();
        }
        if(GamePaused) {
            UnpauseGame();
        }
    }

    //displays game over screen when time runs out
    public void GameIsOver() {
        gameOver = true;
        gameOverObj.SetActive(true);
        audioSource.PlayOneShot(sfxGameOver);
        PlayerEndedGame();
        PauseCountdown();
    }

    //gets rid of game over actions
    public void GameIsNotOver() {
        gameOver = false;
        gameOverObj.SetActive(false);
        UnpauseGame();
    }
    
    //loads lobby from new game button
    public void LoadLobbyFromGame() {
        lobbyObj.SetActive(true);
    }

    //exits the app
    public void Exit() {
        StartCoroutine(ExitApp());
    }

    IEnumerator ExitApp() {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}

