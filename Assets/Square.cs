using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Assets
{
    public class Square : MonoBehaviour
    {
        public static GameObject LeftCube;
        public static GameObject RightCube;
        public static GameObject TopCube;
        public static GameObject BottomCube;
        public static GameObject Plane;
        public static GameObject Fuel;
        public bool rotate;

        public int X { get; private set; }
        public int Y { get; private set; }
        public int W { get; private set; }
        public bool[] walls = {true, true, true, true};

        private GameObject[] objectWalls = new GameObject[3];
        
        private RectDetails rectDetails;
        public Square(int x, int y, int w)
        {
            X = x; Y = y; W = w;
            rectDetails = new RectDetails(x,y,w,w);
        }

        /// <summary>
        ///   <para>Returns the name of the game object.</para>
        /// </summary>
        public override string ToString()
        {
            return rectDetails.ToString();
        }

        public void RemoveWall(Main.RoomSide wall)
        {
            switch (wall)
            {
                case Main.RoomSide.BottomSide:
                    walls[0] = false;
                    break;
                case Main.RoomSide.RightSide:
                    walls[1] = false;
                    break;
                case Main.RoomSide.TopSide:
                    walls[2] = false;
                    break;
                case Main.RoomSide.LeftSide:
                    walls[3] = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("wall", wall, null);
            }
        }

        /// <summary>
        /// Place the prefab for each wall at the starting point and rotate it if needed (bottom and tops)
        /// </summary>
        /// <param name="starting"></param>
        /// <param name="name"></param>
        void DrawCubeLine(Vector3 starting, string name)
        {
            var start = starting;
            
            GameObject obj = null;
            switch (name)
            {
                case "Top":
                    start.x -= 0.46f;
                    obj = Instantiate(TopCube, start, Quaternion.Euler(0,90,0));
                    
                    break;
                case "Bottom":
                    start.x += 0.46f;
                    obj = Instantiate(BottomCube, start, Quaternion.Euler(0,90,0));
                    
                    break;
                case "Right":
                    start.z += 0.5f;
                    obj = Instantiate(RightCube, start, Quaternion.identity);
                    break;
                case "Left":
                    start.z -= 0.5f;
                    obj = Instantiate(LeftCube, start, Quaternion.identity);
                    break;
            }
            obj.name = name;
            // important bit, this alows us to rotate all the items with the stage/plane
            obj.transform.parent = Plane.transform;

        }

        /// <summary>
        /// This function will determine the corners of the square, place the prefabs on them and rotate them as nessesary to make up a wall
        /// 
        /// d---c
        /// |   |
        /// |   |
        /// a___b
        /// </summary>
        public void Draw()
        {
            float offset = 0.0f;
            float ax = rectDetails.getAx() + offset;
            float ay = rectDetails.getAy() + offset;
            float bx = rectDetails.getBx() + offset;
            float by = rectDetails.getBy() + offset;
            float cx = rectDetails.getCx() + offset;
            float cy = rectDetails.getCy() + offset; // but is it really the height
            float dx = rectDetails.getDx() + offset;
            float dy = rectDetails.getDy() + offset;
            
            //  walls are stored in an array of true/false that indicatres if that wall position is enabled or removed
            if (walls[0])
            {
                DrawCubeLine(new Vector3(ax,0,ay), "Bottom");
            }
            if (walls[1])
            {
                DrawCubeLine(new Vector3(bx,0,by), "Right");
            }
            if (walls[2])
            {
                DrawCubeLine(new Vector3(cx,0,cy), "Top");
            }
            if (walls[3])
            {
                DrawCubeLine(new Vector3(dx,0,dy), "Left");
            }
        }

        public bool IsWalled(Main.RoomSide wall)
        {
            switch (wall)
            {
                case Main.RoomSide.TopSide:
                    return walls[0];
                case Main.RoomSide.RightSide:
                    return walls[1];
                case Main.RoomSide.BottomSide:
                    return walls[2];
                case Main.RoomSide.LeftSide:
                    return walls[3];
                default:
                    throw new ArgumentOutOfRangeException("wall", wall, null);
            }
            
        }


        public void PlaceFuel(GameObject fuel)
        {
            if(fuel == null)
                Debug.Log("Fuel prefab is mull");
            const float offset = 0.5f;
            // lets place it relative to the position of the middle of the current square(rectdetails)
            var centre = new Vector3(rectDetails.getAx() + offset, 0 ,rectDetails.getAy() + offset);
            
            // Plonk some fuel down
            Instantiate(fuel, centre , Quaternion.Euler(0,0,0));
            fuel.transform.parent = Plane.transform; // rotate with stage pleases
            fuel.transform.localScale = new Vector3(0.5f,0.5f,0.5f); // Dyanimcally change the scale (should have really done this on import)

            // we've added more fual to the level
            Main.FuelLeft++;
            
        }

        public void PlacePlayer(GameObject player)
        {
            // just like placing fuel
            const float offset = 0.5f;
            var centre = new Vector3(rectDetails.getAx() + offset, 0 ,rectDetails.getAy() + offset);
            Instantiate(player, centre , Quaternion.Euler(0,0,0));
        }
    }
}
