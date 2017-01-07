using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TextController : MonoBehaviour {

	public Text text;
	private enum States {start, cell, cell_unlocked, cell_open, cell_death, investigate, hair_0, hair_1, objects_0, objects_1, nose_0, nose_1, nose_2, nose_death, lock_0, lock_1, lock_locked, lock_unlocked, lock_hair, lock_objects, cell_objects, plant_look, freedom};
	private States myState;

	bool objects_collected = false;
	bool hair_collected = false;
	bool lock_seeded = false;
	bool plant_looked = true;

	int stall = 0;
	int stall_max = 4;
	int digger = 0;
	int digger_max = 5;

	// Use this for initialization
	void Start () {
		myState = States.start;
	}

	// Update is called once per frame
	void Update () {

		print (myState);

		if (myState == States.cell) 				{cell ();}
		else if (myState == States.start) 			{start ();}
		else if (myState == States.investigate) 	{investigate ();}
		else if (myState == States.hair_0) 			{hair_0 ();}
		else if (myState == States.hair_1) 			{hair_1 ();} 
		else if (myState == States.objects_0) 		{objects_0 ();}
		else if (myState == States.objects_1) 		{objects_1 ();} 
		else if (myState == States.nose_0) 			{nose_0 ();}
		else if (myState == States.nose_1) 			{nose_1 ();}
		else if (myState == States.nose_2) 			{nose_2 ();} 
		else if (myState == States.lock_0) 			{lock_0 ();}
		else if (myState == States.lock_1) 			{lock_1 ();}
		else if (myState == States.lock_locked) 	{lock_locked ();}
		else if (myState == States.lock_unlocked) 	{lock_unlocked ();}
		else if (myState == States.lock_hair) 		{lock_hair ();}
		else if (myState == States.lock_objects) 	{lock_objects ();}
		else if (myState == States.plant_look) 		{plant_look ();}
		else if (myState == States.cell_unlocked) 	{cell_unlocked ();}
		else if (myState == States.cell_open) 		{cell_open ();}
		else if (myState == States.cell_death) 		{cell_death ();}
		else if (myState == States.nose_death) 		{nose_death ();}
	}

	void start () { //state only when first starting 

		hair_collected = false;
		objects_collected = false;
		lock_seeded = false;
		plant_looked = false;

		stall = 0;
		digger = 0;

		text.color = Color.white;

		text.text = "You awake. Groggily.\n\n" +
					"It is dark. It is hard to see - you know this is not right. " +
					"As you begin to panic, you scramble with your hands to get a sense of the place. " +
					"You are in a confined cell.  It is dirty but solid. \n\n" +
							
					"[Press I to Investiate your surroundings]\n" +
					"[Press P to Pick your nose]";
					
		if (Input.GetKeyDown (KeyCode.I)) {myState = States.investigate;}

		// NOSE PICKER
		if (Input.GetKeyDown (KeyCode.P)) {

			if (digger > digger_max) 		{myState = States.nose_death;}
			else if (digger <= digger_max) 	{myState = States.nose_0; ++digger;}

		}

	}

	void cell () { //state when returning to cell after investigating 

		text.text = "You are still in the cell.\n\n" +
					"As the seconds tick away, your desire and need to escape escalates with the panic. \n\n" +

					"[Press I to Investiate your surroundings]\n" +
					"[Press P to Pick your nose]";

		if (Input.GetKeyDown (KeyCode.I)) {myState = States.investigate;}


		// NOSE PICKER
		if (Input.GetKeyDown (KeyCode.P)) {


			if (hair_collected)				{myState = States.nose_1;}

			else {
				
			if (digger <= digger_max) 		{myState = States.nose_0; ++digger;}
			if (digger > digger_max) 		{myState = States.nose_death;}
			
			}

		}


	 
	}

	void investigate() { //state when investigating 

		string base_text = "Fumbling around the small confines of the cell, you find a few items. \n" + "You can make out:\n";
		string addendum_hair = null;
		string addendum_objects = null;
		string press_objects = null;
		string press_hair = null;

		if (!objects_collected && !lock_seeded) {

			addendum_objects = "+ a few round objects \n";
			press_objects = "[Press O to examine the Objects]\n";
		}
				
		if (!hair_collected) {
			addendum_hair = "+ a tuft of what seems like hair \n";
			press_hair = "[Press H to examine the Hair]\n";
		}

		string end_text = 	"+ a simple lock on the cell door \n\n" + 
					
					press_hair +
					press_objects + 
					"[Press L to examine the Lock] \n" + 
					"[Press R to roam the cell]";

		text.text = base_text + addendum_hair + addendum_objects + end_text;

		if (Input.GetKeyDown (KeyCode.H) && !hair_collected) 	{myState = States.hair_0;} 

		if (Input.GetKeyDown (KeyCode.O) && !objects_collected) {myState = States.objects_0;}

		if (Input.GetKeyDown (KeyCode.L)) {

			if (hair_collected || objects_collected) 	{myState = States.lock_1;}
			else {myState = States.lock_0;}

		}
			
		else if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}


	}

	void hair_0 () {

		text.text = "You rub the tuft of hair against your palm.  It tickles. \n\n" +
					"The hair is coarse and rough.  It is bound together by a small leather cord. " +
					"You stroke your own hair instinctively, but your head has been shaven. \n\n" +

					"[Press T to Take the hair]\n\n"+
					"[Press R to Return to investigating your cell.]";

		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}
		else if (Input.GetKeyDown (KeyCode.T)) {myState = States.hair_1;}

	}

	void hair_1 () {

		hair_collected = true;
		objects_collected = false;

		text.text = "You clutch the tuft of hair.\n\n" +
					"Maybe you will talk to it like a doll when you go crazy.\n\n" +

					"[Press R to return to roaming your cell]";  
			
		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}

	}

	void objects_0 () {

		text.text = "You pass your fingers over the round objects.  They seem to be some sort of seed. \n\n" +
					"They are small and roughly textured. They have no distinct smell or taste - you cannot identify them. \n\n" + 

					"[Press T to Take the seeds]\n" +
					"[Press R to Return to roaming your cell]";

		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}
		else if (Input.GetKeyDown (KeyCode.T)) {myState = States.objects_1;}
	}

	void objects_1 () {
		
		hair_collected = false;
		objects_collected = true;

		text.text = "You keep hold of the mysterious seeds.\n\n" +
					"Perhaps they will serve as a meager meal.  Perhaps. \n\n" +

					"[Press R to return to roaming your cell]"; 

		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}

	}

	//END OF OBJECTS

	void nose_0 () {

		text.text = "You dig for treasure in your nose caverns. \n\n" +
					"What else is there to do anyways, right? \n\n" +
		
					"[Press R to return to roaming your cell]"; 
		
		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}
	}

	void nose_1 () {

		hair_collected = false;
		digger = 0;

		string base_text;
		string end_text;
		string option_text = null;

		base_text = "You dig for treasure in your nose caverns. \n\n" +
					"The tuft of hair in your hand tickles deep into your sinus.  You fling it away, but... \n\n" +
					"AAAAAAAAAAAACHHHHOOOOOO!!!!!\n\n" +
					"You sneeze a mess into your confined cell.  Great.\n\n";

		if (lock_seeded) {
		
			option_text = "[Press I to Investigate the lock]\n";
		}

			end_text =	"[Press R to return to roaming your cell]";

		text.text = base_text + option_text + end_text;

		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}
		if (Input.GetKeyDown (KeyCode.I) && lock_seeded) {myState = States.lock_unlocked;}

	}


	void lock_0 () {

		//concatenate to make easier to read (does not affect the layout) 
		text.text = "The lock is a simple lock with a keyhole on the front. \n\n" +
			
					"There are no distinguishing marks, as it is fairly unremarkable, but it is definitely sturdy enough to keep you confined in the cell.\n\n" +

					"[Press O to Open the lock]\n"+
					"[Press R to Return to investigating your cell]";

		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}
		if (Input.GetKeyDown (KeyCode.O)) {myState = States.lock_locked;}
	}

	void lock_locked () {

		text.text = "The lock is stronger than you thought.  There is no way to force it open. \n\n" +

					"[Press R to Return to investigating your cell]";

		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}

	}

	void lock_1 () {

		//concatenate to make easier to read (does not affect the layout) 

		string base_text = "The lock is a simple lock with a keyhole on the front. \n\n" +
			
							"There are no distinguishing marks, as it is fairly unremarkable, but it is definitely sturdy enough to keep you confined in the cell.\n\n"; 

		string addendum = null;

		if (objects_collected) {

			addendum = "[Press S to try using the Seeds on the lock]\n[Press R to Return to investigating your cell]";



			if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}
			if (Input.GetKeyDown (KeyCode.S)) {myState = States.lock_objects;}

		}
			
		if (hair_collected) {

			addendum = "[Press H to try using the Hair on the lock]\n[Press R to Return to investigating your cell]";

			if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}
			if (Input.GetKeyDown (KeyCode.H)) {myState = States.lock_hair;}
		}

		text.text = base_text + addendum;
	}

	void lock_hair () {

		text.text = "You brush the tuft of hair against the lock. \n\n" +
					"You are sure the lock, if it were alive, would be ticklish, but besides that sentiment, nothing happens. \n\n" +

					"[Press R to Return to investigating your cell]";

		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}
	}

	void lock_objects () {

		lock_seeded = true;
		objects_collected = false;

		text.text = "You place the seeds into the keyhole of the lock. \n\n" +
					
					"They fall into the lock just right, and no matter how you shake it, they tumble around without falling back out. \nBesides that, nothing happens. \n\n" +

					"[Press R to Return to investigating your cell]";

		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell;}
	}

	void lock_unlocked () {

		text.text = "You take a closer look at the lock.\n\n" +

					"Your disgusting mucus has a coated the lock and seeped into the keyhole. " +
					"As you peer into the hole, you see that the seeds have been drowned in your phlegm. " +
					"Surprisingly, the shell of the seeds begin to give way to rapidly sprouting plant tendrils.\n\n" +
					"The lock creaks and shatters as the cluster of plants grows beyond its limits. \n\n" +

					"Your sneeze has activated the seeds.  Cool.\n\n" +
					
					"[Press O to open the cell door]\n"+
					"[Press R to Return to investigating your cell]";

		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell_unlocked;}
		if (Input.GetKeyDown (KeyCode.O)) {myState = States.cell_open;}

	}

	void cell_unlocked () { //state when returning to cell after unlocking

		text.text = "You are still in the cell.\n\n" +
					"It is the same as it ever was, though your few meager possessions are now gone and the lock has been broken.\n\n" +

					"The strange writhing plant mass has tumbled into your room, and continues to pulsate and grow.\n\n" +


					"[Press I to Investigate the plant]\n" +
					"[Press P to Pick your nose]\n" +
					"[Press O to open the cell door]\n";


		// NOSE PICKER
		if (Input.GetKeyDown (KeyCode.P)) {
			
			if (digger > digger_max) 		{myState = States.nose_death;}
			if (stall > stall_max)			{myState = States.cell_death;}
			else if (digger <= digger_max) 	{myState = States.nose_2; ++digger; ++stall;}

		}

		if (Input.GetKeyDown (KeyCode.I)) {
			
			if (!plant_looked)				{myState = States.plant_look; plant_looked = true;}

			else if (plant_looked) {
				if (stall > stall_max)		{myState = States.cell_death;}
				if (stall <= stall_max)		{myState = States.plant_look; ++stall;}
			}
		}
			
		if (Input.GetKeyDown (KeyCode.O)) {myState = States.cell_open;}

	}



	void nose_2 () { //state when returning to cell after unlocking

		text.text = "You dig for treasure in your nose caverns. \n\n" +

					"You have previously vacated the entirety of your nose - it was satisfying. \n\n" +

					"It is empty now. \n\n" +

					"[Press R to roam the cell]\n";


		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell_unlocked;} 

	}


	void plant_look () { //state when returning to cell after unlocking

		text.text = "The plant is a quickly growing mass. \n\n" +

			"You are, however, not a botanist, so besides the exponential growth, you can draw no conclusions from this odd specimen. \n\n" +

			"[Press R to roam the cell]\n";


		if (Input.GetKeyDown (KeyCode.R)) {myState = States.cell_unlocked;} 

	}

	void cell_death () { //state when stall too long

		text.color = Color.red;

		text.text = "As you fiddle around, you notice that the growing plant mass grows faster and faster.\n\n" +

					"In fact, it has overgrown the cell dooor and begun to press against the cell walls. " +
					"The realization that your already confined space is now a constricting deathtrap comes to you seconds too late. \n\n" +

					"The last thoughts that form in your mind as the plant mass presses you against the cell walls," +
					"compressing the air out of your lungs and crushing your bones to paste is: \n\n" +

					"\"I hate vegetables...\" \n\n" +

					"[Press P to Play again]\n";

		if (Input.GetKeyDown (KeyCode.P)) {myState = States.start;}


	}

	void nose_death () { //state when picking nose too long

		text.color = Color.red;

		text.text = "As you reach into your nostril once again, you suddenly feel a rush of warmth. \n\n" +

					"A torrent of hot blood pours from your nose. You've gone too far this time." + 
					"Dizziness washes over you as you attempt to scoop the blood back into your nose. \n\n" +

					"It is clear you are not a doctor.\n\n" + 

					"[Press P to Play again]";

		if (Input.GetKeyDown (KeyCode.P)) {myState = States.start;}


	}

	void cell_open () { //state when returning to cell after unlocking

		text.color = Color.green;

		text.text = "YOU HAVE ESCAPED!!!! \n\n" +
					"YOU ARE A MASTERMIND!!!! \n\n" +

					"[Press P to Play again]";
		
		if (Input.GetKeyDown (KeyCode.P)) {myState = States.start;}

	}

}