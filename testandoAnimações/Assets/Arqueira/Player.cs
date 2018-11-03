using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {
   
    SpriteRenderer sr;
    Rigidbody2D rb2d;
    Animator animator;
	Animation animation;

    public float jumpSpeed;
	public bool grounded = true;
	public float speed;
	bool jumping;
	bool jumpCheckpoint;
	bool attacking;

	RuntimeAnimatorController anC;

    //---------------------------------------------------------------------------------
    void Start () {
        speed = 3f;
        jumpSpeed = 10f;
		jumping = false;
		jumpCheckpoint = false;

        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

		rb2d.gravityScale = 3.0f;

		anC = animator.runtimeAnimatorController;    //Get Animator controller
	}
	//---------------------------------------------------------------------------------
	bool isGrounded(){
		return (Mathf.Abs(rb2d.velocity.y) <= 0.05f)? true : false;
	}
	//---------------------------------------------------------------------------------
	bool isWalking(){
		return (Mathf.Abs(rb2d.velocity.x) > 0.7)? true: false;
	}
	//---------------------------------------------------------------------------------
	bool isGoingDown(){
		Debug.Log(rb2d.velocity.y);
		return (rb2d.velocity.y < 0f)? true : false;
	}

	//---------------------------------------------------------------------------------
	void Jump(){
		rb2d.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
	}
	//---------------------------------------------------------------------------------
	float getClipTime(string animationName){
		float time = 0f;
			for(int i = 0; i< anC.animationClips.Length; i++){
				if(animationName.Equals(anC.animationClips[i].name)){
					time = anC.animationClips[i].length;
					break;
				}
			}
		return time;	
	}
	//---------------------------------------------------------------------------------
	void moveCharacter(){
		//RIGHT
		if(Input.GetKey(KeyCode.D)){
			//transform.position += Vector3.right * speed *Time.deltaTime;
			rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
			sr.flipX = true;
		}	
		//LEFT
		else if(Input.GetKey(KeyCode.A)){
			//transform.position += Vector3.right * -speed * Time.deltaTime;
			rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
			sr.flipX = false;
		}
		//JUMP
		if(Input.GetKeyDown(KeyCode.W)){
			jumping = true;          //player is in jumping process
			animator.SetBool("goingDown", false); // << --- GAMBIARRA, teve q ser aqui pra funcionar
			Invoke("Jump", 0.1f);    //apply jump force after x seconds because of impulse animaiton
		}
	}
	//---------------------------------------------------------------------------------
	void updateAnimation(){

		if(isGoingDown()) {
			animator.SetBool("goingDown", true);
			jumpCheckpoint = true;
		}

		if(isGrounded()) {
			if(!jumping){
				animator.SetBool("grounded", true);                                          

				if(isWalking()) 
					animator.SetBool("walking", true);
				else 
					animator.SetBool("walking",false);
			}
			else{
				animator.SetBool("walking",false);
            	animator.SetBool("grounded", false);
				if(jumpCheckpoint) 
					jumping = jumpCheckpoint =  false; //so that player completes the jump before set bool as false 
			}
		}
		else {
			animator.SetBool("grounded", false);
			animator.SetBool("walking",false);
		}

	}
	//---------------------------------------------------------------------------------
	void stopShootingAnimation(){
		animator.SetBool("attacking", false);
		attacking = false;
	}
	//---------------------------------------------------------------------------------
	void updateAttack(){
		if(Input.GetKey(KeyCode.T) && !attacking){
			attacking = true;
			animator.SetBool("attacking", true);
			Invoke("stopShootingAnimation", getClipTime("Archer_Shooting"));
		}
	}
	//---------------------------------------------------------------------------------
    void Update () {
		moveCharacter();
		updateAnimation();
		updateAttack();
	} 
}
















