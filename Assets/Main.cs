using System;
using System.Collections.Generic;
using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

[RequireComponent(typeof(AudioSource))]
public class Main : MonoBehaviour
{
    public enum RoomSide {BottomSide = 1, RightSide = 2, TopSide = 3, LeftSide = 4};

    public GameObject LeftCube;
    public GameObject RightCube;
    public GameObject TopCube;
    public GameObject BottomCube;
    public GameObject Stage;
    public GameObject Fuel;
    public GameObject Player;
    public AudioClip RotationSound;
    public static string Score;
    public float RotateAngle;
    public static string Time = string.Empty;
    public static bool timeUp = false;
    public AudioClip VictoryMusic;
    public AudioSource mainLevelMusic;

    static readonly Random randomGenerator = new Random(DateTime.Now.Millisecond);
    readonly List<Square> _mazeGrid = new List<Square>();
   
    static int screenWidth = 10;
    static int screenHeight = 10;	
    static int roomWidth = 1;
    readonly int _maxRows = screenWidth/roomWidth;
    readonly int _maxColumns = screenHeight/roomWidth;
    bool removeSidesRandonly = true;
    private bool isVictory = false;
    
    public static int FuelLeft;

    void  OnGUI()
    {
        GUI.Box(new Rect(0,0,200,25), !timeUp ? "Remaining Items:"+Score : Score);
        GUI.Box(new Rect(0,30,200,25), "Time left:"+Time);
    }

    void LevelReset()
    {
        isVictory = false;
        Score = string.Empty;
        timeUp = false;
    }

    // Use this for initialization
	void Start ()
	{

	    LevelReset();
	    Square.Plane = Stage;
	    Square.TopCube = TopCube;
	    Square.BottomCube = BottomCube;
	    Square.RightCube = RightCube;
	    Square.LeftCube = LeftCube;
	    DrawLevel(_maxColumns, _maxRows, roomWidth);
	}

    // Update is called once per frame
	void Update ()
	{

	    if (Input.GetKeyDown(KeyCode.R))
	    {
	        Stage.transform.Rotate(0, RotateAngle, 0f);
            GetComponent<AudioSource>().PlayOneShot(RotationSound);
	    }

	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
            LevelReset();
            SceneManager.LoadScene("MenuScene");
	    }

	    // Update the main Score with the text
	    Score = FuelLeft.ToString();
	    if ((FuelLeft == 0 && !isVictory) || Input.GetKeyDown(KeyCode.W))
	    {
            // win
	        if ((!timeUp && !isVictory)|| Input.GetKeyDown(KeyCode.W))
	        {
	            isVictory = true;
	            mainLevelMusic.Stop();
                
	            GetComponent<AudioSource>().PlayOneShot(VictoryMusic);
                
	            
	            Score = "You Win!";
	        }
	    }
	    else
	    {
            if(timeUp)
                Score = "Sorry, you Lose!";
	    }

	    if (!GetComponent<AudioSource>().isPlaying && isVictory)
	    {
	        LevelReset();
	        SceneManager.LoadScene("MenuScene");
	        
	    }

	}

    private void DrawLevel(int maxColumns, int maxRows, int roomWidth)
    {
        for (int y = 0; y < maxColumns; y++)
        {
            for (int x = 0; x < maxRows; x++)
            {
                _mazeGrid.Add(new Square(x * roomWidth, y * roomWidth, roomWidth));
            }
        }

        var totalRooms = _mazeGrid.Count;

        
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

            int rInt = randomGenerator.Next(0, removableSides.Count-1);
            int randSideIndex = rInt;
            
            if (removeSidesRandonly)
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


        var playermazePosition = randomGenerator.Next(0, _mazeGrid.Count);
        Debug.Log("Size of grid is: " + _mazeGrid.Count);
        for (int i = 0; i < _mazeGrid.Count;i++)
        {
            var square = _mazeGrid[i];
            square.Draw();

            if (randomGenerator.Next(0, 3) == 1)
                square.PlaceFuel(Fuel);

            if (i == playermazePosition)
                square.PlacePlayer(Player);
        }
    }
}
