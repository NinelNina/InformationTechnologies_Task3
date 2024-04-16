namespace Task2.Model;

public class Turtle : Animal
{
    public override bool Move()
    {
        if (Speed < maxSpeed)
        {
            Speed += 5;

            return true;
        }
        return false;
    }

    public override bool Stand()
    {
        if (Speed > minSpeed)
        {
            Speed -= 5;

            return true;
        }
        return false;
    }
}
