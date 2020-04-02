using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicMixer : MonoBehaviour {

	public GameObject player;			//A reference to the player Game Object speed float variable
	public AudioMixer aMixer;			//A reference to the Audio Mixer of the project to control the fader volumes

	public AudioSource stemMix1;			//A reference to the Game Object with stem mix 1 
	public AudioSource stemMix2;			//A reference to the Game Object with stem mix 2
	public AudioSource colMusic;			//A reference to the Game Object with the collsion music 
	public AudioSource lapComp;				//A reference to the Game Object with a lap complete sound
	public AudioSource gameComp;			//A reference to the Game Object with a game complete sound

	// Use this for initialization
	void Start () 
	{
	


		aMixer.SetFloat ("MM_Stem1", 0f);		//Set Stem mix 1 volume fader in the Audio Mixer to 0db
		aMixer.SetFloat ("MM_Stem2", -80f);		//Set Stem Mix 2 volume fader in the Audio Mixer to -80db so not playing at the start
		aMixer.SetFloat("SFX",0f);				//Set SFX volume fader in the Audio Mixer to 0db, at end of the game set to -80db.
		aMixer.SetFloat("HitMusic_SFX",0f);		//Set HitMusic_SFX volume fader in the Audio Mixer to 0db, at end of the game set to -80db.

		Invoke ("StartStemMixes", 11f);			//Wait 11 secounds after start of game to play Stem Mixes after CutScene with whole mix has finished
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player.GetComponent<VehicleMovement> ().speed > 20f) //if players speed increases above 20f
		{
			//...set the fader volume of Stem Mix 2 to 0db we are moving...
			aMixer.SetFloat ("MM_Stem2", 0f); 
		} 
		else 
		{
			//...otherwise set it to -80db, we not moving...
			aMixer.SetFloat ("MM_Stem2", -80f);
		}
	}

	//This function is called after the cut scene and while mix has finished playing at the start of the level
	void StartStemMixes()
	{
		//.. if the stem is not playing, start playing them together so in sync
		if(!stemMix1.isPlaying)
		{
		stemMix1.Play (); //...Play Stem Mix 1
		stemMix2.Play (); //...Play  Stem Mix 2
		}
	}

	// this function is called by the players VehicleMovement script when it collides with a wall
	public void PlayWallCollisionMusic()
	{
		//...check is the audio source is not playing already
		if (!colMusic.isPlaying) 
		{
			//...play the audio source of the referenced Game Object in the Music Manager
			colMusic.Play ();
		}
	}

	//This function is called by the game manager script when a lap complete
	public void CompleteLap()
	{
		//...play lap complete sound...
		lapComp.Play();
	}

	//This function is called by the game manager script when the game is complete
	public void CompleteGame()
	{
		//...play game complete sound
		gameComp.Play ();

		//Set HitMusic_SFX and SFX fader in Audio Mixer to -80db so we cant hear engine, collision or HitMusic sound at end of game
		aMixer.SetFloat("SFX",-80f);
		aMixer.SetFloat("HitMusic_SFX",-80f);

	}
		
}
