using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerMovement : MonoBehaviour {



    [Header("Tweakables")]
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] [Range(0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -5;
    [SerializeField] float jumpPower = 20f;

    // Dash
    [SerializeField] float dashLerpDuration = 1f;
    [SerializeField] float dashStartValue = 2f;
    [SerializeField] float dashEndValue = 0.6f;
    [SerializeField] float DashingTimout = 2f;
    private float timeOfLastDash;
    private float DashMult = 1;

    // Movement
    public AudioSource dash;
    float velocityY = 0f;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    CharacterController controller = null;


    // Multipliers
    bool isJumping = false;
    bool isDashing = false;

    // Inputs 
    Vector2 targetDir = Vector2.zero;

    private void Start() { controller = GetComponent<CharacterController>(); }

    private void Update() { GrabInputs(); UpdateMovement(); }

    void GrabInputs() {
        targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space)) { isJumping = true; }
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - timeOfLastDash > DashingTimout) { isDashing = true; timeOfLastDash = Time.time; }
    }

    void UpdateMovement() {

        targetDir.Normalize(); // Makes moving diagnally speed the same

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
    
       // velocityY += gravity * Time.deltaTime;

        if (controller.isGrounded) {
            if (isJumping) { velocityY = jumpPower; isJumping = false; Debug.Log("Jump"); }  // Debug.Log("Jump");
            else { }// velocityY = gravity * Time.deltaTime; }// velocityY = 0f; }
        }
        else {

            if (isJumping) { isJumping = false; }
             velocityY += gravity * Time.deltaTime;
        }

        if (isDashing) { StartCoroutine(Dash()); }


        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * DashMult * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dash() {
        dash.Play();
        isDashing = false;
        float timeElapsed = 0;

        while (timeElapsed < dashLerpDuration) {
            DashMult = Mathf.Lerp(dashStartValue, dashEndValue, timeElapsed / dashLerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
