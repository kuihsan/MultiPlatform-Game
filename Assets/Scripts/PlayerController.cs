using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float jumpForce;
    bool canJump;
    public AudioSource jump;
    private Vector2 startTouchPosition, endTouchPosition;
    private Touch touch;
    private IEnumerator goCoroutine;
    private bool coroutineAllowed;




    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
        }

        if(touch.phase == TouchPhase.Began)
        {
            startTouchPosition = touch.position;
        }

        if (Input.touchCount > 0 && touch.phase == TouchPhase.Ended && coroutineAllowed)
        { 
            endTouchPosition = touch.position;
            
            if((endTouchPosition.x > startTouchPosition.x))
            {
                rb.AddForce(Vector3.right * 2, ForceMode.Impulse);
                Debug.Log("kanan");
            }

            else if ((endTouchPosition.x < startTouchPosition.x))
            {
                rb.AddForce(Vector3.left * 2, ForceMode.Impulse);
                Debug.Log("kiri");
            }




        }


        if (Input.GetMouseButtonDown(0) && canJump)
        {
            //jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jump.Play();
        }
    }

    private IEnumerator Go(Vector3 direction)
    {
        coroutineAllowed = false;

        for (int i = 0; i <= 2; i++)
        {
            transform.Translate(direction);
            yield return new WaitForSeconds(0.01f);
        }

        for (int i = 0; i <= 2; i++)
        {
            transform.Translate(direction);
            yield return new WaitForSeconds(0.01f);
        }
        transform.Translate(direction);
        coroutineAllowed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Obstacle")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
