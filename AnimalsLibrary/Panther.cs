namespace Task2.Model;

public class Panther : Animal, IVoicalizable
{
    public event EventHandler<VoiceEventArgs> Voice;
    public event EventHandler<ClimbTreeEventArgs> ClimbTree;

    public override bool Move()
    {
        if (Speed < maxSpeed)
        {
            Speed += 20;

            return true;
        }
        return false;
    }

    public string GetVoiceMessage()
    {
        return "Пантера рычит!";
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
        ClimbTree?.Invoke(this, new ClimbTreeEventArgs("Пантера залезла на дерево."));
    }

    public void OnVocalize()
    {
        Voice?.Invoke(this, new VoiceEventArgs(GetVoiceMessage()));
    }
}

public class ClimbTreeEventArgs : EventArgs
{
    public string ClimbTreeMessage { get; }

    public ClimbTreeEventArgs(string climbTreeMessage)
    {
        ClimbTreeMessage = climbTreeMessage;
    }
}