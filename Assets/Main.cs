using System;
using System.Collections.Generic;
using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

[RequireComponent(typeof(AudioSource))]
public class Main : MonoBehaviour
{
    // easy way to reason about which sides of the walls we are refering to
    public enum RoomSide {BottomSide = 1, RightSide = 2, TopSide = 3, LeftSide = 4};

    // The walls that we will be placing strategically on the playing area
    public GameObject LeftCube;
    public GameObject RightCube;
    public GameObject TopCube;
    public GameObject BottomCube;

    // So we can rotate it
    public GameObject Stage;

    // the fuel game object which is just a battery 
    public GameObject Fuel;
    
    // the player gme object
    public GameObject Player;
    
    
    // Where we store the string representation of our score
    public static string Score;

    // how much are we going to rotate to board?
    public float RotateAngle;

    // FX
    public AudioClip VictoryMusic;
    public AudioSource mainLevelMusic;
    public AudioClip RotationSound;
    public AudioClip failSound;
    
    // custom setting that represents how much time it costs to blow up a wall/remove it
    public static int digTimeDisadvantage;

    // our random number genreator to randonly remove walls, place fuel and the player
    static readonly Random randomGenerator = new Random(DateTime.Now.Millisecond);
    
    // The theoretical model of our playing board
    readonly List<Square> _mazeGrid = new List<Square>();
   
    // our level timer
    public static Timer LevelTimer = new Timer(mins, secs);
    
    // timer defaults
    public static int mins = 5;
    public static int secs = 0;
    
    // how much fual we have left to collect in the level
    public static int FuelLeft;


    // are we there yet?
    private bool isVictory = false;

    // misc settings that are self explanatory
    static int screenWidth = 10;
    static int screenHeight = 10;	
    static int roomWidth = 1;
    readonly int _maxRows = screenWidth/roomWidth;
    readonly int _maxColumns = screenHeight/roomWidth;
    bool removeSidesRandonly = true;
    public static bool endSceneStarted = false;
    
    void  OnGUI()
    {
        // draw our score
        GUI.Box(new Rect(0,0,200,25), Score);
        GUI.Box(new Rect(0,30,200,25), "Time left:" + LevelTimer.ToString());
    }

    void LevelReset()
    {
        isVictory = false;
        Score = string.Empty;
        LevelTimer.TimeUp = false;
        digTimeDisadvantage = 10;
        LevelTimer = new Timer(mins,secs);
    }

    // Use this for initialization
	void Start ()
	{
	    LevelReset();
	    // lets make the user provided prefabs globally accessible (just to make communication easier in this prototype)
	    Square.Plane = Stage;
	    Square.TopCube = TopCube;
	    Square.BottomCube = BottomCube;
	    Square.RightCube = RightCube;
	    Square.LeftCube = LeftCube;
	    
        // Auto-generate a level
	    DrawLevel(_maxColumns, _maxRows, roomWidth);
	}

    // Update is called once per frame
	void Update ()
	{
        // update the timer
	    LevelTimer.Update();
        
        // should we rotate the board?
	    if (Input.GetKeyDown(KeyCode.R))
	    {
	        Stage.transform.Rotate(0, RotateAngle, 0f);
            GetComponent<AudioSource>().PlayOneShot(RotationSound); // make woosh sound
	    }

        // lets get out of here!
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
            LevelReset();
            SceneManager.LoadScene("MenuScene");
	    }

	    
        // check for victory:
	    if (FuelLeft == 0 && !isVictory || Input.GetKeyDown(KeyCode.W)) // we can explicitly win by pressing 'W' - cheat
	    {
            // win condition
	        if ((!LevelTimer.TimeUp || Input.GetKeyDown(KeyCode.W)) && !endSceneStarted)
	        {
	            mainLevelMusic.Stop();
	            GetComponent<AudioSource>().PlayOneShot(VictoryMusic);
	            endSceneStarted = true;
	            Score = "YOU WIN!";
	            isVictory = true;
	        }
	    }
	    else
	    {
	        if ((LevelTimer.TimeUp && !isVictory || Input.GetKeyDown(KeyCode.F)) && !endSceneStarted)
	        {
	            mainLevelMusic.Stop();
	            GetComponent<AudioSource>().PlayOneShot(failSound);
	            endSceneStarted = true;
	            Score = "Sorry, you Lose!";
	            isVictory = false;
	        }
	    }

	    if (endSceneStarted)
	    {
	        LevelTimer.StopTimer();
	    }
	    else
	    {
	        Score = "Remaining Items:" + FuelLeft.ToString(); // Update the main Score with the text
	    }


	    if (!GetComponent<AudioSource>().isPlaying && endSceneStarted)
	    {
	        LevelReset();
	        SceneManager.LoadScene("MenuScene");
	        endSceneStarted = false;
	    }

	}

    // Draws a maxRows x maxCols board of roomWidth(which we only ever set to 1)
    private void DrawLevel(int maxColumns, int maxRows, int roomWidth)
    {
        // make a whole bunch of squares (these will represnt each room/tunnel)
        for (int y = 0; y < maxColumns; y++)
        {
            for (int x = 0; x < maxRows; x++)
            {
                _mazeGrid.Add(new Square(x * roomWidth, y * roomWidth, roomWidth));
            }
        }

        var totalRooms = _mazeGrid.Count;

        // determine which sides can be removed and then randonly remove a number of them (using only the square objects - no drawing yet)
        for (int i = 0; i < totalRooms; i++)
        {
            var nextIndex = i + 1;
            var prevIndex = i - 1;

            if (nextIndex >= totalRooms)
                break;

            var thisRow = Math.Abs(i / maxColumns);
            var lastColumn = (thisRow + 1 * maxColumns) - 1;
            var thisColumn = maxColumns - (lastColumn - i);

            int roomAboveIndex = i - maxColumns;
            int roomBelowIndex = i + maxColumns;
            int roomLeftIndex = i - 1;
            int roomRightIndex = i + 1;

            bool canRemoveAbove = roomAboveIndex > 0;
            bool canRemoveBelow = roomBelowIndex < totalRooms;
            bool canRemoveLeft = thisColumn - 1 >= 1;
            bool canRemoveRight = thisColumn + 1 <= maxColumns;

            var removableSides = new List<RoomSide>();
            var currentRoom = _mazeGrid[i];
            var nextRoom = _mazeGrid[nextIndex];

            if (canRemoveAbove && currentRoom.IsWalled(RoomSide.TopSide) && _mazeGrid[roomAboveIndex].IsWalled(RoomSide.BottomSide))
                removableSides.Add(RoomSide.TopSide);
            if (canRemoveBelow && currentRoom.IsWalled(RoomSide.BottomSide) && _mazeGrid[roomBelowIndex].IsWalled(RoomSide.TopSide))
                removableSides.Add(RoomSide.BottomSide);
            if (canRemoveLeft && currentRoom.IsWalled(RoomSide.LeftSide) && _mazeGrid[roomLeftIndex].IsWalled(RoomSide.RightSide))
                removableSides.Add(RoomSide.LeftSide);
            if (canRemoveRight && currentRoom.IsWalled(RoomSide.RightSide) && _mazeGrid[roomRightIndex].IsWalled(RoomSide.LeftSide))
                removableSides.Add(RoomSide.RightSide);

            // which of the sides should we remove for this square?

            int rInt = randomGenerator.Next(0, removableSides.Count-1);
            int randSideIndex = rInt;
            
            if (removeSidesRandonly) // this should always be set
            {
                switch (removableSides[randSideIndex])
                {
                    case RoomSide.TopSide:
                        currentRoom.RemoveWall(RoomSide.TopSide);
                        nextRoom.RemoveWall(RoomSide.BottomSide);
                        continue;
                    case RoomSide.RightSide:
                        currentRoom.RemoveWall(RoomSide.RightSide);
                        nextRoom.RemoveWall(RoomSide.LeftSide);
                        continue;
                    case RoomSide.BottomSide:
                        currentRoom.RemoveWall(RoomSide.BottomSide);
                        nextRoom.RemoveWall(RoomSide.TopSide);
                        continue;
                    case RoomSide.LeftSide:
                        currentRoom.RemoveWall(RoomSide.LeftSide);
                        var prev = _mazeGrid[prevIndex];
                        prev.RemoveWall(RoomSide.RightSide);
                        continue;
                }
            }
        }



        // In which square/room should we place the player?
        var playermazePosition = randomGenerator.Next(0, _mazeGrid.Count);
        for (int i = 0; i < _mazeGrid.Count;i++)
        {
            var square = _mazeGrid[i];

            // this is where all the action happens: draw the square using the prefabs
            square.Draw();

            // place the fual here
            if (randomGenerator.Next(0, 3) == 1)
            {
                Debug.Log("placing fuel...");
                square.PlaceFuel(Fuel);
                Debug.Log("placing fuel...done");
            }


            // place the player here
            if (i == playermazePosition)
                square.PlacePlayer(Player);
        }
    }
}
