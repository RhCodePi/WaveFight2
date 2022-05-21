using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator playerAnim;
    private GameManager gameManager;
    private float horizontal, xBound = 7.50f;
    [SerializeField] public float speed = 10f;
    private Quaternion playerRotation = new Quaternion(0, 1, 0, 0);
    [HideInInspector] public Vector2 moveDir;
    private void Awake() {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    private void FixedUpdate() {
        if(gameManager.getIsGameActive)
            PlayerMovement();
    }

    private void PlayerMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        moveDir = horizontal * Vector3.right;
        playerRb.velocity = moveDir * speed;
        //for left and right turn
        playerRb.transform.rotation = (horizontal > 0) ? playerRotation : playerRb.transform.rotation;
        playerRb.transform.rotation = (horizontal < 0) ? Quaternion.identity : playerRb.transform.rotation;

        //for left and right bound
        transform.position = (transform.position.x < -xBound) ? 
                            new Vector3(-xBound, transform.position.y) : transform.position;
        transform.position = (transform.position.x > xBound) ?
                             new Vector3(xBound, transform.position.y) : transform.position;

        playerAnim.SetFloat("Speed", Mathf.Abs(horizontal));
    }

}
