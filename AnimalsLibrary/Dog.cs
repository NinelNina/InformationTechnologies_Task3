namespace Task2.Model;

public class Dog : Animal, IVoicalizable
{
    public event IVoicalizable.VoiceHandler Voice;

    public override bool Move()
    {
        if (Speed < maxSpeed)
        {
            Speed += 10;

            return true;
        }
        return false;
    }

    public void OnVocalize()
    {
        Voice?.Invoke($"Гав!");
    }

    public override bool Stand()
    {
        if (Speed > minSpeed)
        {
            Speed -= 10;

            return true;
        }
        return false;
    }
}
