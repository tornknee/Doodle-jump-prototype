using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    Rigidbody2D myRigi;

    [SerializeField]
    float jumpHeight;

    [SerializeField]
    float movementSpeed;

    [SerializeField]
    Transform gameCam;

    [SerializeField]
    Text currentScore;

    [SerializeField]
    Text highScore;

    [SerializeField]
    GameObject character;

    [SerializeField]
    GameObject platform;

    [SerializeField]
    GameObject gameOver;

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    GameObject buttonToSelect;

    [SerializeField]
    AudioSource dashSound;

    ParticleSystem particles;

    public bool alive = true;

    float score;

    float x;
    float y;

    float tap1;
    float tap2;
    bool tappedLeft = false;
    bool tappedRight = false;

    float dashTimer = 0.1f;
    int direction;

    public Animator animator;

    List<GameObject> platforms = new List<GameObject>();

    void Start()
    {
        y = gameCam.position.y - 2;

        myRigi = GetComponent<Rigidbody2D>();

        particles = GetComponentInChildren<ParticleSystem>();

        currentScore.text = "0m";
 
    }

    void Update()
    {
        Cursor.visible = false;
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));

        Spawner();
        Dash();
        CamStuff();

        character.SetActive(alive);
        highScore.text = "Highscore: " + (PlayerPrefs.GetFloat("highscore", 0));

        /*/Solution 1, left in here for future reference
        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
        }
        /*/

        //Solution 2
        if (alive)
        {
            
            //Movement
            transform.Translate(Vector3.right * movementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);

            //Score
            //Rounds up, removing decimals, *100 to reintroduce 2 decimal places but won't have the ., then /100 to get the number to 2 decimal places
            score = Mathf.Round(gameCam.position.y * 100) / 100;
            //adding a string to the end automatically converts to string without using .ToString()
            currentScore.text = score + "m";

        }
    }

    //Jumps when hits platform
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (myRigi.velocity.y <= 0)
        {
            myRigi.velocity = Vector2.up * jumpHeight;
        }
    }

    /// <summary>
    /// Moves the camera up as character moves up, ends game when character falls out of camera, and destroys platforms as they fall out of camera
    /// </summary>
    private void CamStuff()
    {
        //moves camera up as character moves up
        if (transform.position.y > gameCam.position.y)
        {
            Vector3 newCamPos = gameCam.position;
            newCamPos.y = transform.position.y;
            gameCam.position = newCamPos;
        }
        //Ends game if character falls below edge of camera
        if (transform.position.y < gameCam.position.y - 6)
        {
            alive = false;
            gameOver.SetActive(true);
            eventSystem.SetSelectedGameObject(buttonToSelect);
            if (score > PlayerPrefs.GetFloat("highscore", 0))
            {
                PlayerPrefs.SetFloat("highscore", score);
            }
        }
        //Destroys lowest platform as they move below the edge of camera
        if (platforms[0].transform.position.y < gameCam.position.y - 6)
        {
            GameObject platformDie = platforms[0];
            platforms.Remove(platforms[0]);
            Destroy(platformDie);
        }
    }

    /// <summary>
    /// Maintains a constant 5 platforms
    /// </summary>
    private void Spawner()
    {
        if (platforms.Count < 5)
        {
            x = Random.Range(-6, 6);
            platforms.Add(Instantiate(platform, new Vector2(x, y), Quaternion.identity));
            y = y + Random.Range(2, 4);
        }
    }

    /// <summary>
    /// Checks for double tap, does a dash
    /// </summary>
    private void Dash()
    {
        //Checks for double tap left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (tappedLeft == true)
            {
                tap2 = Time.time;
                if (tap2 - tap1 < 0.25f)
                {
                    direction = 1;
                    tappedLeft = false;
                    Debug.Log("dashing");
                    dashTimer = 0;
                    particles.Play();
                    dashSound.Play();
                }
                else
                {
                    tap1 = Time.time;
                }
            }
            else
            {
                tap1 = Time.time;
                tappedLeft = true;
                tappedRight = false;
            }

        }
        //Checks for double tap right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (tappedRight == true)
            {
                tap2 = Time.time;

                if (tap2 - tap1 < 0.25f)
                {
                    direction = 2;
                    tappedRight = false;
                    Debug.Log("dashing");
                    dashTimer = 0;
                    particles.Play();
                    dashSound.Play();
                }
                else
                {
                    tap1 = Time.time;
                }
            }
            else
            {
                tap1 = Time.time;
                tappedRight = true;
                tappedLeft = false;
            }
        }
        //Creates a timer for the dash move
        if (dashTimer >= 0.1)
        {
            direction = 0;
            particles.Stop();
        }
        else
        {
            dashTimer = dashTimer + Time.deltaTime;
            if (direction == 1)
            {
                transform.Translate(Vector3.left * 50 * Time.deltaTime);
            }
            else if (direction == 2)
            {
                transform.Translate(Vector3.right * 50 * Time.deltaTime);
            }
        }
    }
}