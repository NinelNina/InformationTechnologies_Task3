
using System;

namespace Task2.Model;

public class Dog : Animal, IVoicalizable
{
    public event EventHandler<VoiceEventArgs> Voice;

    public override bool Move()
    {
        if (Speed < maxSpeed)
        {
            Speed += 10;

            return true;
        }
        return false;
    }

    public string GetVoiceMessage()
    {
        return "Собака лает!";
    }

    public void OnVocalize()
    {
        Voice?.Invoke(this, new VoiceEventArgs(GetVoiceMessage()));

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
