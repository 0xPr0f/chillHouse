using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float gravity = -9.9f;

    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    public Transform groundCheck;
    public float jumpHeight = 3f;
    public float flyheight;
    public float speed = 12f;
    
    PhotonView view;
    Animator animator;
    float targetTime = 3f;
    Vector3 velocity;
    bool isGrounded;
    private void Start()
    {
     
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
       if (view.IsMine)
        {
            targetTime -= Time.deltaTime;

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

           

            Vector3 move = transform.right * x + transform.forward * z;
            if(x != 0)
            {
              animator.SetBool("isWalking", true);
            }
            if(x == 0)
            {
               animator.SetBool("isWalking", false);
            }
            if (z != 0)
            {
               animator.SetBool("isWalking", true);
            }
            if (z == 0)
            {
              animator.SetBool("isWalking", false);
            }

            controller.Move(move * speed * Time.deltaTime);
            

        if (Input.GetButtonDown("Jump") && isGrounded)
            {
                if (targetTime <= 0.0f)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    animator.SetTrigger("Jump");
                    targetTime = 3f;
                }

            }
           


            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

   
        
}
