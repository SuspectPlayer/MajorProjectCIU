using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ThirdPersonMovement : MonoBehaviourPunCallbacks
{
    [Header("Components")]
    [SerializeField]
    private PhotonView photonView;
    public CharacterController controller;
    public Animator animator;
    public Transform playerCamera;

    [Header("Movement")]
    // general movement
    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    // jump movement
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private Vector3 velocity;
    private bool isGrounded;
    // camera movement
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;


    private void Awake()
    {
        if (photonView == null) photonView = GetComponent<PhotonView>();
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }
    }
}
