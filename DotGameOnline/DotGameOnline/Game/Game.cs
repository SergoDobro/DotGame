using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGameOnline.Game
{

    public class FinalPointClass : IPoint
    {
        Point localPoint;
        public string PlayerID { get { return playerID; } }
        public string MassID { get; set; } = "";
        public bool IsChecked { get; set; }
        public bool IsActive { get; set; }
        string playerID;
        public float it = 0;
        public FinalPointClass(Point point, string playerID)
        {
            localPoint = point;
            this.playerID = playerID;
            IsActive = true;
        }
        public Point GetPoint()
        {
            return localPoint;
        }
        public void SetActive(bool isActive) => this.IsActive = isActive;
    }
    public class FinalPlayer : IPlayer
    {
        public Color GetColor() => color;
        Color color;
        public string ID { get { return id; } }
        string id;
        static Random random = new Random(890);
        public FinalPlayer(string newID)
        {
            id = newID;
            color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
        }
    }
    class FinalGrid : IGrid
    {
        public Dictionary<Point, FinalPointClass> GetPoints { get { return points; } }
        public Dictionary<string, FinalPlayer> GetPlayers { get { return players; } }

        Dictionary<Point, FinalPointClass> points = new Dictionary<Point, FinalPointClass>();
        Dictionary<string, FinalPlayer> players = new Dictionary<string, FinalPlayer>();

        public void SetPixel(int x, int y, string playerID)
        {
            //this point will neverChange
            Point point = new Point(x, y);
            if (points.ContainsKey(point))
            {
                return;
            }
            if (!players.ContainsKey(playerID)) players.Add(playerID, new FinalPlayer(playerID));
            FinalPointClass pointClass = new FinalPointClass(point, playerID);
            points.Add(point, pointClass);


            // setactivity



            DoToAll(new Action<FinalPointClass>(p =>
            {
                p.it = 0;
                p.IsChecked = false;
            }));
            Iterate(point, new List<Point>());
        }
        public void DoToAll(Action<FinalPointClass> action)
        {
            foreach (var p in points)
            {
                action(p.Value);
            }
        }
        public bool IsUsed(Point point) => points.ContainsKey(point);
        public bool IsUsedAndActive(Point point) => points.ContainsKey(point) && points[point].IsActive;
        public Dictionary<Point, FinalPointClass> NearPoints(Point point)
        {
            Dictionary<Point, FinalPointClass> nearpoints = new Dictionary<Point, FinalPointClass>();
            if (IsUsed(point.Moved(-1, -1))) nearpoints.Add((point.Moved(-1, -1)), points[point.Moved(-1, -1)]);
            if (IsUsed(point.Moved(0, -1))) nearpoints.Add((point.Moved(0, -1)), points[point.Moved(0, -1)]);
            if (IsUsed(point.Moved(1, -1))) nearpoints.Add((point.Moved(1, -1)), points[point.Moved(1, -1)]);
            if (IsUsed(point.Moved(1, 0))) nearpoints.Add((point.Moved(1, 0)), points[point.Moved(1, 0)]);
            if (IsUsed(point.Moved(1, 1))) nearpoints.Add((point.Moved(1, 1)), points[point.Moved(1, 1)]);
            if (IsUsed(point.Moved(0, 1))) nearpoints.Add((point.Moved(0, 1)), points[point.Moved(0, 1)]);
            if (IsUsed(point.Moved(-1, 1))) nearpoints.Add((point.Moved(-1, 1)), points[point.Moved(-1, 1)]);
            if (IsUsed(point.Moved(-1, 0))) nearpoints.Add((point.Moved(-1, 0)), points[point.Moved(-1, 0)]);
            return nearpoints;
        }
        public Dictionary<Point, FinalPointClass> NearActivePoints(Point point)
        {
            Dictionary<Point, FinalPointClass> nearpoints = new Dictionary<Point, FinalPointClass>();
            if (IsUsedAndActive(point.Moved(-1, -1))) nearpoints.Add((point.Moved(-1, -1)), points[point.Moved(-1, -1)]);
            if (IsUsedAndActive(point.Moved(0, -1))) nearpoints.Add((point.Moved(0, -1)), points[point.Moved(0, -1)]);
            if (IsUsedAndActive(point.Moved(1, -1))) nearpoints.Add((point.Moved(1, -1)), points[point.Moved(1, -1)]);
            if (IsUsedAndActive(point.Moved(1, 0))) nearpoints.Add((point.Moved(1, 0)), points[point.Moved(1, 0)]);
            if (IsUsedAndActive(point.Moved(1, 1))) nearpoints.Add((point.Moved(1, 1)), points[point.Moved(1, 1)]);
            if (IsUsedAndActive(point.Moved(0, 1))) nearpoints.Add((point.Moved(0, 1)), points[point.Moved(0, 1)]);
            if (IsUsedAndActive(point.Moved(-1, 1))) nearpoints.Add((point.Moved(-1, 1)), points[point.Moved(-1, 1)]);
            if (IsUsedAndActive(point.Moved(-1, 0))) nearpoints.Add((point.Moved(-1, 0)), points[point.Moved(-1, 0)]);
            return nearpoints;
        }

        public void Iterate(Point currentPoint, List<Point> pointsInRow)
        {
            List<Point> localPointsInRow = new List<Point>();
            pointsInRow.ForEach(x => localPointsInRow.Add(x));
            localPointsInRow.Add(currentPoint);
            points[currentPoint].IsChecked = true;
            Dictionary<Point, FinalPointClass> neighbors = NearActivePoints(currentPoint);
            foreach (var n in neighbors)
            {
                if (n.Value.PlayerID != points[currentPoint].PlayerID) continue;
                if (!n.Value.IsChecked)
                {
                    Iterate(n.Key, localPointsInRow);
                }
                if (n.Key == localPointsInRow[0] && localPointsInRow.Count > 3)
                {
                    Circle(localPointsInRow);
                }
            }
        }
        public void Circle(List<Point> circle)
        {
            circle.ForEach(x => points[x].it += 1);
            Circle_FindDots(circle);
            points[circle[0]].it = 0;
        }
        void Circle_FindDots(List<Point> circle)
        {
            bool success = false;
            string playerID;
            Point point;
            Point upperPoint;
            Point lowerPoint;
            bool miniFlag = true;
            for (int i = 0; i < circle.Count; i++)
            {
                point = circle[i];
                lowerPoint = point.Moved(0, 1);
                if (points.ContainsKey(lowerPoint)) miniFlag = false;
                upperPoint = point.Moved(0, -1);
                if (miniFlag && points.ContainsKey(upperPoint)) miniFlag = false;
                if (!miniFlag)
                {
                    lowerPoint = point.Moved(1, 0);
                    if (points.ContainsKey(lowerPoint)) continue;
                    upperPoint = point.Moved(-1, 0);
                    if (points.ContainsKey(upperPoint)) continue;
                }
                playerID = points[point].PlayerID;

                List<Point> checkedForCirclingUpper = new List<Point>();
                List<Point> checkedForCirclingLower = new List<Point>();
                List<Point> toCheckPointsUpper = new List<Point>() { upperPoint };
                List<Point> toCheckPointsLower = new List<Point>() { lowerPoint };
                List<string> checkedMassesUpper = new List<string>() { "" };
                List<string> checkedMassesLower = new List<string>() { "" };
                bool flag = true;
                while (toCheckPointsUpper.Count != 0 && toCheckPointsLower.Count != 0)
                {
                    //Upper
                    List<Point> newToCheckUpper = new List<Point>();
                    foreach (var circledPoint in toCheckPointsUpper)
                    {
                        List<Point> circleNeighbors = GetCircleNeighbors(circledPoint, checkedForCirclingUpper, toCheckPointsUpper);
                        for (int ii = 0; ii < circleNeighbors.Count; ii++)
                        {
                            if (circle.Contains(circleNeighbors[ii]) || newToCheckUpper.Contains(circleNeighbors[ii]))
                            {
                                circleNeighbors.RemoveAt(ii);
                                ii--;
                            }
                            else if (points.ContainsKey(circleNeighbors[ii]) && points[circleNeighbors[ii]].MassID != "")
                            {
                                string id = points[circleNeighbors[ii]].MassID;
                                if (!checkedMassesUpper.Contains(id))
                                {
                                    checkedMassesUpper.Add(id);
                                    checkedForCirclingUpper.AddRange(masses[id]);
                                }
                            }
                        }
                        newToCheckUpper.AddRange(circleNeighbors);
                    }
                    checkedForCirclingUpper.AddRange(toCheckPointsUpper);
                    toCheckPointsUpper = newToCheckUpper;
                    //Lower
                    List<Point> newToCheckLower = new List<Point>();
                    foreach (var circledPoint in toCheckPointsLower)
                    {
                        List<Point> circleNeighbors = GetCircleNeighbors(circledPoint, checkedForCirclingLower, toCheckPointsLower);
                        for (int ii = 0; ii < circleNeighbors.Count; ii++)
                        {
                            if (circle.Contains(circleNeighbors[ii]) || newToCheckLower.Contains(circleNeighbors[ii]))
                            {
                                circleNeighbors.RemoveAt(ii);
                                ii--;
                            }
                            else if (points.ContainsKey(circleNeighbors[ii]) && points[circleNeighbors[ii]].MassID != "")
                            {
                                string id = points[circleNeighbors[ii]].MassID;
                                if (!checkedMassesLower.Contains(id))
                                {
                                    checkedMassesLower.Add(id);
                                    checkedForCirclingLower.AddRange(masses[id]);
                                }
                            }
                        }
                        newToCheckLower.AddRange(circleNeighbors);
                    }
                    checkedForCirclingLower.AddRange(toCheckPointsLower);
                    toCheckPointsLower = newToCheckLower;
                    foreach (var itemLow in toCheckPointsLower)
                    {
                        foreach (var itemUp in newToCheckUpper)
                        {
                            if (itemUp == itemLow)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (!flag) break;
                    }
                    if (!flag) break;
                }
                if (!flag) continue;
                success = true;

                massesCounter++;
                string newMassID = (massesCounter).ToString();
                if (toCheckPointsUpper.Count == 0)
                {
                    checkedMassesUpper.Remove("");
                    foreach (var mID in checkedMassesUpper)
                    {
                        foreach (var d in masses[mID])
                        {
                            points[d].MassID = newMassID;
                        }
                        masses.Remove(mID);
                    }
                    masses.Add(newMassID, checkedForCirclingUpper);
                    foreach (var p in checkedForCirclingUpper)
                    {
                        if (!points.ContainsKey(p))
                        {
                            points.Add(p, new FinalPointClass(p, playerID) { IsActive = false, MassID = newMassID });
                        }
                        else
                        {
                            points[p].MassID = newMassID;
                        }
                    }
                }
                else
                {
                    checkedMassesLower.Remove("");
                    foreach (var mID in checkedMassesLower)
                    {
                        foreach (var d in masses[mID])
                        {
                            points[d].MassID = newMassID;
                        }
                        masses.Remove(mID);
                    }
                    masses.Add(newMassID, checkedForCirclingLower);
                    foreach (var p in checkedForCirclingLower)
                    {
                        if (!points.ContainsKey(p))
                        {
                            points.Add(p, new FinalPointClass(p, playerID) { IsActive = false, MassID = newMassID });
                        }
                    }
                }

                break;
            }
            if (!success) return;

            Console.WriteLine("Success!");


        }
        public List<Point> GetCircleNeighbors(Point point, List<Point> checkedForCircling, List<Point> circled)
        {
            List<Point> circleNeighbors = new List<Point>();
            if (!checkedForCircling.Contains(point.Moved(-1, 0)) && !circled.Contains(point.Moved(-1, 0))) circleNeighbors.Add(point.Moved(-1, 0));
            if (!checkedForCircling.Contains(point.Moved(0, -1)) && !circled.Contains(point.Moved(0, -1))) circleNeighbors.Add(point.Moved(0, -1));
            if (!checkedForCircling.Contains(point.Moved(1, 0)) && !circled.Contains(point.Moved(1, 0))) circleNeighbors.Add(point.Moved(1, 0));
            if (!checkedForCircling.Contains(point.Moved(0, 1)) && !circled.Contains(point.Moved(0, 1))) circleNeighbors.Add(point.Moved(0, 1));
            return circleNeighbors;
        }

        public void SetActivities() // IN PROGRESS
        {
            foreach (var p in points)
            {
                if (p.Value.IsActive)
                {
                    string owner = p.Value.PlayerID;
                    Dictionary<Point, FinalPointClass> neighbors = NearPoints(p.Key);
                    int myNeighbors = 0;
                    int notmyNeighbors = 0;
                    foreach (var n in neighbors)
                    {
                        if (n.Value.PlayerID == owner) myNeighbors++;
                        else notmyNeighbors++;
                    }
                    if (myNeighbors == 8)
                    {
                        //p.Value.IsActive = false;
                    }
                }
            }
        }

        static int massesCounter = 0; // IN PROGRESS
        Dictionary<string, List<Point>> masses = new Dictionary<string, List<Point>>(); // only inactive dots // IN PROGRESS


        public Dictionary<SendPoint, string> GetPointsInZone(int x, int y, int width, int height)
        {
            Dictionary<SendPoint, string> pointPlayers = new Dictionary<SendPoint, string>();
            foreach (var p in points)
            {
                if (p.Key.x > x && p.Key.x < x + width && p.Key.y > y && p.Key.y < y + height)
                {
                    pointPlayers.Add(new SendPoint(p.Value), p.Value.PlayerID);
                }
            }
            return pointPlayers;
        }
    }
}
