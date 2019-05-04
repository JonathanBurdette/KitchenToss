using UnityEngine;

public class FruitBehavior : MonoBehaviour {

    private Rigidbody rigidBody;
    private AudioSource audioSource;
    
    [SerializeField] private AudioClip sfxToss;
    [SerializeField] private AudioClip sfxSplat;
    [SerializeField] private AudioClip sfxScore;

    private float height1;
    private float height2;
    private float swipeDistance;
    private float startTime;
    private float endTime;
    private float timeDifference;
    private float forceAmount;

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;

    private bool toss = false;

    //prevents crash from fruit rotation
    private float rotateBuffer = 2.692f;
    
    // Use this for initialization
    private void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update() {

        if (GameManager.instance.PlayerActive) {
            //rotates fruit
            transform.Rotate(new Vector3(5, 100, -15) * Time.deltaTime);
        }

        //height check
        height1 = transform.position.y;

        //compare current height and height at end of update function to determine score
        if (height1 < height2 && rigidBody.detectCollisions == true && height2 > rotateBuffer) {
            GameManager.instance.UpdateScore(height2);
            if (height1 >= 3) {
                audioSource.PlayOneShot(sfxScore);
                GetComponent<FruitBehavior>().enabled = false;
            }
        }

        //if fruit is at start position and no game over, pause, or before beginning countdown
        if (height1 == height2 && GameManager.instance.PlayerActive) {

            //detects swipe
            if (Input.touches.Length > 0) {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) {
                    firstPressPos = new Vector2(touch.position.x, touch.position.y);
                    startTime = Time.time;
                }

                if (touch.phase == TouchPhase.Ended) {
                    secondPressPos = new Vector2(touch.position.x, touch.position.y);
                    swipeDistance = secondPressPos.y - firstPressPos.y;
                    endTime = Time.time;
                    timeDifference = endTime - startTime;

                    //swipe power is determined by length and time span of swipe                    
                    forceAmount = 0.00150f * (swipeDistance / timeDifference);
                    audioSource.PlayOneShot(sfxToss);
                    rigidBody.useGravity = true;
                    toss = true;
                }
            }
        }

        height2 = height1;
    }

    private void FixedUpdate() {
        //launches fruit 
        if (toss == true) {
            toss = false;
            rigidBody.velocity = new Vector2(0, 0);
            rigidBody.AddForce(new Vector2(0, forceAmount), ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ceiling") {
            rigidBody.AddForce(new Vector3(1, 0, -1.75f), ForceMode.Impulse);
            audioSource.PlayOneShot(sfxSplat);
            GameManager.instance.UpdateCeilingHits();
            GameManager.instance.UpdateFruitsTossed();
            rigidBody.detectCollisions = false;
            GameManager.instance.SpawnFruit();
            Destroy(gameObject, 2);
        }

        if (collision.gameObject.tag == "Floor") {
            rigidBody.AddForce(new Vector3(1, -1000, -1.75f), ForceMode.Impulse);
            audioSource.PlayOneShot(sfxSplat);
            GameManager.instance.UpdateFruitsTossed();
            rigidBody.detectCollisions = false;
            GameManager.instance.SpawnFruit();
            Destroy(gameObject, 2);
        }
    }
}
