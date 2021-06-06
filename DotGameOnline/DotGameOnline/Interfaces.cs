using DotGameOnline.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotGameOnline
{
    public interface IGrid
    {
        void SetPixel(int x, int y, string playerID);
        public Dictionary<Point, FinalPointClass> GetPoints { get; }
        public Dictionary<string, FinalPlayer> GetPlayers { get; }
        Dictionary<SendPoint, string> GetPointsInZone(int x, int y, int width, int height);
    }
    public interface IPoint
    {
        Point GetPoint();
        string PlayerID { get; }
    }
    public interface IPlayer
    {
        Color GetColor();
        public string ID { get; }
    }
    public struct Color
    {
        float R { get; set; }
        float G { get; set; }
        float B { get; set; }
        float A { get; set; }
        public Color(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        public Color(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
            A = 1;
        }
        public override string ToString()
        {
            return $"rgb({(int)(R * 255)}, {(int)(G * 255)}, {(int)(B * 255)})";
        }
    }
    public struct Point
    {
        public int x, y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Point Moved(int x, int y)
        {
            return new Point(this.x + x, this.y + y);
        }
        public static bool operator ==(Point pointA, Point pointB) => pointA.x == pointB.x && pointA.y == pointB.y;
        public static bool operator !=(Point pointA, Point pointB) => pointA.x != pointB.x || pointA.y != pointB.y;
        public static Point operator +(Point pointA, Point pointB) => new Point(pointA.x + pointB.x, pointA.y + pointB.y);
        public static Point operator -(Point pointA, Point pointB) => new Point(pointA.x - pointB.x, pointA.y - pointB.y);
    }

    /// <summary>
    /// What will be sent to users
    /// </summary>
    public class SendPoint
    {
        public int x { get; set; }
        public int y { get; set; }
        public string playerID { get; set; }
        public SendPoint()
        {

        }
        public SendPoint(int x, int y, string playerID)
        {
            this.x = x;
            this.y = y;
            this.playerID = playerID;
        }
        public SendPoint(FinalPointClass point)
        {
            this.x = point.GetPoint().x;
            this.y = point.GetPoint().y;
            this.playerID = point.PlayerID;
        }
    }

}
