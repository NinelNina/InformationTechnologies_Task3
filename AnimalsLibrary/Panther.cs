namespace Task2.Model;

public class Panther : Animal, IVoicalizable
{
    public event IVoicalizable.VoiceHandler Voice;
    public event EventHandler ClimbTree;

    public override bool Move()
    {
        if (Speed < maxSpeed)
        {
            Speed += 20;

            return true;
        }
        return false;
    }

    public override bool Stand()
    {
        if (Speed > minSpeed)
        {
            Speed -= 20;

            return true;
        }
        return false;
    }

    public void OnClimbTree()
    {
        ClimbTree?.Invoke(this, EventArgs.Empty);
    }

    public void OnVocalize()
    {
        Voice?.Invoke($"Ррр!");
    }
}
