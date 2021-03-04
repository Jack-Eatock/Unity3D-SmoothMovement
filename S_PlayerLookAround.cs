using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerLookAround : MonoBehaviour {
    [Header("Tweakables")]
    [SerializeField] Transform Playercamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] [Range(0f, 0.5f)] float mouseSmoothTime = 0.03f;

    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0.0f;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseVelocity = Vector2.zero;

    void Start() {
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

      // Update is called once per frame
    void Update() {
        UpdateMouseLook();
    }

     void UpdateMouseLook() {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseVelocity, mouseSmoothTime);

        // Moving cam vertically
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);
        Playercamera.localEulerAngles = Vector3.right * cameraPitch;

        // Moving cam horizontal
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

}
