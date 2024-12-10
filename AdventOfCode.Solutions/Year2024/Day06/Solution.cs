using System.Drawing;
using System.Security.AccessControl;

namespace AdventOfCode.Solutions.Year2024.Day06;

class Solution : SolutionBase
{
    public Solution() : base(06, 2024, "") { }

    public int MIN { get; } = 0;
    public int MAX { get { return Debug ? 9 : 129; } }  //Input is 130x130, Debug is 10x10

    protected override string SolvePartOne()
    {
        List<string> theGrid = Input.SplitByNewline().ToList();

        //This should only return one item
        List<Point> guardLocation = FindItem(theGrid, '^');
        List<Point> obstacles = FindItem(theGrid, '#');
        List<Point> traversed = TraverseTheGrid_P1(guardLocation.First(), obstacles);

        //Attempt 1: 4559 - CORRECT
        return traversed.Distinct().Count().ToString();
    }

    protected override string SolvePartTwo()
    {
        List<string> theGrid = Input.SplitByNewline().ToList();

        //This should only return one item
        List<Point> guardLocation = FindItem(theGrid, '^');
        List<Point> obstacles = FindItem(theGrid, '#');

        List<Point> turningPoints = new();
        List<Point> traversed = new();
        TraverseTheGrid_P2(guardLocation.First(), obstacles, out traversed, out turningPoints);

        turningPoints = turningPoints.OrderBy(x => x.X).ToList();
        var groupedX = turningPoints.OrderBy(x => x.X).GroupBy(x => x.X).ToList();
        var groupedY = turningPoints.OrderBy(x => x.Y).GroupBy(x => x.Y).ToList();

        return traversed.Distinct().Count().ToString();
    }

    private List<Point> FindItem(List<string> theGrid, char locatable)
    {
        List<Point> itemsFound = new List<Point>();

        //Rows - Y
        for (int y = 0; y < theGrid.Count; y++)
        {
            //Columns - X
            for (int x = 0; x < theGrid[y].Length; x++)
            {
                char c = theGrid[y][x];
                if (c == locatable)
                    itemsFound.Add(new Point(x, y));
            }
        }
        return itemsFound;
    }

    private List<Point> TraverseTheGrid_P1(Point theGuard, List<Point> obstacles)
    {
        bool guardOutside = false;
        DIRECTION directionOfTravel = DIRECTION.UP;
        List<Point> traversedPoints = new List<Point>();

        while (guardOutside == false)
        {
            //And add the guards new location to the list of traversed points so long as they are inside
            traversedPoints.Add(theGuard);

            Point guardNextStep = GuardStep(theGuard, directionOfTravel);
            //Check if the guard hit an obstacle
            if (ObstacleEncountered(guardNextStep, obstacles))
            {
                //If an obstacle was encountered, change the direction of travel
                directionOfTravel = ChangeDirection(directionOfTravel);
            }

            //Then have the guard step in the direction of travel
            theGuard = GuardStep(theGuard, directionOfTravel);

            //Check if the guard is outside
            guardOutside = IsTheGuardOutside(theGuard);

            if (traversedPoints.Distinct().Count() > 16900)
            {
                //We've traversed all points - something is wrong
                return traversedPoints;
            }
        }

        return traversedPoints;
    }

    private List<Point> TraverseTheGrid_P2(Point theGuard, List<Point> obstacles, out List<Point> traversedPoints, out List<Point> turningPoints)
    {
        bool guardOutside = false;
        DIRECTION directionOfTravel = DIRECTION.UP;
        traversedPoints = new();
        turningPoints = new();

        while (guardOutside == false)
        {
            //And add the guards new location to the list of traversed points so long as they are inside
            traversedPoints.Add(theGuard);

            Point guardNextStep = GuardStep(theGuard, directionOfTravel);
            //Check if the guard hit an obstacle
            if (ObstacleEncountered(guardNextStep, obstacles))
            {
                //If an obstacle was encountered, change the direction of travel
                directionOfTravel = ChangeDirection(directionOfTravel);

                //Because the guard encountered an obstacle,
                //record the guards current location as a turning point
                //if the guard has been to this spot before, increment the counter
                turningPoints.Add(theGuard);
            }

            //Then have the guard step in the direction of travel
            theGuard = GuardStep(theGuard, directionOfTravel);

            //Check if the guard is outside
            guardOutside = IsTheGuardOutside(theGuard);

            if (traversedPoints.Distinct().Count() > 16900)
            {
                //We've traversed all points - something is wrong
                return traversedPoints;
            }
        }

        return traversedPoints;
    }

    private DIRECTION ChangeDirection(DIRECTION current)
    {
        DIRECTION newDirection = DIRECTION.NONE;
        switch (current)
        {
            case DIRECTION.UP:
                newDirection = DIRECTION.RIGHT;
                break;
            case DIRECTION.RIGHT:
                newDirection = DIRECTION.DOWN;
                break;
            case DIRECTION.DOWN:
                newDirection = DIRECTION.LEFT;
                break;
            case DIRECTION.LEFT:
                newDirection = DIRECTION.UP;
                break;
        }
        return newDirection;
    }

    private Point GuardStep(Point currentLocation, DIRECTION direction)
    {
        Point newLocation = new Point();
        // 0 = UP       (y-1)
        // 1 = DOWN     (y+1)
        // 2 = LEFT     (x-1)
        // 3 = RIGHT    (x+1)
        switch (direction)
        {
            case DIRECTION.UP:
                newLocation.X = currentLocation.X;
                newLocation.Y = currentLocation.Y - 1;
                break;
            case DIRECTION.DOWN:
                newLocation.X = currentLocation.X;
                newLocation.Y = currentLocation.Y + 1;
                break;
            case DIRECTION.LEFT:
                newLocation.X = currentLocation.X - 1;
                newLocation.Y = currentLocation.Y;
                break;
            case DIRECTION.RIGHT:
                newLocation.X = currentLocation.X + 1;
                newLocation.Y = currentLocation.Y;
                break;
        }
        return newLocation;
    }

    private bool ObstacleEncountered(Point point, List<Point> obstacles)
    {
        return obstacles.Where(x => x.X == point.X && x.Y == point.Y).Count() > 0;
    }

    private bool IsTheGuardOutside(Point theGuard)
    {
        return theGuard.X > MAX || theGuard.Y > MAX || theGuard.X < MIN || theGuard.Y < MIN;
    }

    enum DIRECTION
    {
        NONE = -1,
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3
    };
}
