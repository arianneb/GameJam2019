// Tutorial script by Michael Long
// This script allows for the moving and controlling of a 2D player character using either a rigidbody2D or moving the transform directly
// Can move left, right, and jump
// Jumping is limited to touching a collider with the 'floor' tag (unless allow_infinite_jumping is set to true)

using System.Collections;		
using System.Collections.Generic;
using UnityEngine;
// The above lines import functionality and methods, which we can now use in our file

// Double slashes (//) are comments. Comments are not pieces of code, and are purely for the benefit of us meatbags, and not the computer
/*
 * The /* are extended comments. 
 * Multi-line comments, wooh!
 */
[RequireComponent(typeof(Rigidbody2D))]		// Make sure we have a rigidbody2D on this component
public class PlayerController : MonoBehaviour 
{
	// Public variables shown up in the Unity editor inspector when you click on the game object
	// Try changing the movement_speed and jump_height in the unity editor
	public bool use_transform_movement = true;
	public float horizontal_walking_speed = 3.0f;		
	public float jump_force = 100f;
	public Rigidbody2D player_physics;	
	public BoxCollider2D player_collider;

	public int allowed_jumps = 1;	// Normally we can only jump once after we touch the floor, but some games allow for 'double-jumping'
	int currently_allowed_jumps = 1;
	public bool allow_infinite_jumping = false;

	// Optional, may cover in tutorial
	Animator animation_manager;


	// This function is called right after the game has started.
	// Use it to intialize one-time features on your object
	void Start () 
	{
		currently_allowed_jumps = allowed_jumps;

		// Use GetComponent to get components located on this gameobject
		player_physics = this.GetComponent<Rigidbody2D> ();

		Animator anim = this.GetComponent<Animator> ();
		if (anim != null)
			animation_manager = anim;
		// The other way to assign components to variables is to drag components into the public fields of the Unity editor
	}
	
	// Update is called once per frame. The game runs many frames per second (fps).
	// Useful for checking player input (did they press SPACEBAR, or use WADS?)
	void Update () 
	{
		// The Horizontal input axis is A and D, left and right arrows
		float horizontal_input = Input.GetAxis ("Horizontal");
		// Use this to move your character left and right
		if (use_transform_movement) {
			// Movng the transform directly allows for instantaneous stopping and starting of movement. Tigher controls you'd likely find in platformers like mario

			// Multiply the incoming input by the walking speed
			// Time.deltaTime is the amount of time (in seconds) to compute the last frame. Multiply things in update by Time.deltaTime to ensure our framerate does not affect our movement speed
			//player_physics.MovePosition((Vector2)this.transform.position + new Vector2(horizontal_input * horizontal_walking_speed * Time.deltaTime, 0));
			this.transform.position += new Vector3 (horizontal_input * horizontal_walking_speed * Time.deltaTime, 0, 0);
		}
		else {
			// Use physics to move our character. Movement feels more sluggish. Moving with physics is necessary if you want your character to bounce around in your physics-based game.

			// Vector2's have both an X and a Y value. For horizontal movement we want to change its X component
			player_physics.AddForce(new Vector2(horizontal_input, 0) * horizontal_walking_speed * Time.deltaTime);
		}


		// Should we jump?
		if (Input.GetButtonDown("Jump") && (currently_allowed_jumps > 0 || allow_infinite_jumping)) {
			Debug.Log ("Jumping!");		// Use Debug.Log() to output information to the console
			currently_allowed_jumps--;
			player_physics.AddForce (new Vector2 (0, 1) * jump_force);	// We don't need to multiply this by Time.deltaTime since this is only occasionally being used
		}

		if (animation_manager != null) {
			animation_manager.SetFloat ("MovementSpeed", Mathf.Abs(horizontal_input));
		}
	}


	// This function is called whenever a collider attached to the gameobject this script is on touches another collider.
	// Ex: touching the ground/roof/another physical object
	// Can be used for things like killing the player if they touch spikes, or to reset the jumping flag (can only jump once after touching the floor)
	void OnCollisionEnter2D(Collision2D coll) 
	{
		// Check the tag of the object we collided with
		if (coll.gameObject.tag == "Floor") {
			Debug.Log ("Touched the floor");
			currently_allowed_jumps = allowed_jumps; 	// We've touched the floor, reset the number of jumps we can do
		} else if (coll.gameObject.tag == "Enemy") {
			Die ();
		}
	}
	// Called repeatedly while we're colliding/touching another collider
	void OnCollisionStay2D(Collision2D coll) 
	{

	}
	// Called when we exit a collision with another collider
	void OnCollisionExit2D(Collision2D coll)
	{

	}


	// This function is called whenever a collider attached to the gameobject this script is on touches a collider set as a 'trigger'.
	// Consider triggers as invisible regions, which can be used for things such as killing a player if they step on lava, or when a player enters a room, enemies are spawned
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Lava") {
			Die ();
		}
	}
	// Called repeatedly while we're inside a trigger collider
	void OnTriggerStay2D(Collider2D other)
	{

	}
	// Called when we leave a trigger collider
	void OnTriggerExit2D(Collider2D other)
	{

	}


	// Call this when you want to destroy the player character
	public void Die()
	{
		Debug.Log ("Character has died!");
		Destroy (this.gameObject);
	}
}
