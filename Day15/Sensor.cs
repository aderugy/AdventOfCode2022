namespace Day15;

public class Sensor
{
    public int X;
    public int Y;
    public int BeaconX;
    public int BeaconY;
    public int Range;

    public Sensor(int[] coordinates)
    {
        X = coordinates[0];
        Y = coordinates[1];
        BeaconX = coordinates[2];
        BeaconY = coordinates[3];
        Range = Math.Abs(BeaconX - X) + Math.Abs(BeaconY - Y);
    }

    /**
     * Returns the bounds of the area without beacons at a given row
     * Both bounds are included
     */
    public (int, int) GetBounds(int y)
    {
        int dY = Math.Abs(Y - y);
        int dX = Range - dY;

        if (dX < 0)
            return (0, 0);

        int lowXBound = X - dX;
        int highXBound = X + dX;
        
        if (y == BeaconY)
        {
            if (X - dX == BeaconX)
                lowXBound++;
            else
                highXBound--;
        }
        return (lowXBound, highXBound);
    }

    /**
     * Calculating the equation of the increasing lines of the beacon search area
     */
    public (int, int) PositiveLines()
    {
        return (X - Y - Range, X - Y + Range);
    }
    
    /**
     * Calculating the equation of the decreasing lines of the beacon search area
     */
    public (int, int) NegativeLines()
    {
        return (X + Y - Range, X + Y + Range);
    }
}
