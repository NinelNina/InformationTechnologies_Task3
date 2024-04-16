namespace Task2.Model;

public interface IVoicalizable
{
    event EventHandler<VoiceEventArgs> Voice;
    void OnVocalize();
    string GetVoiceMessage();
}

public class VoiceEventArgs : EventArgs
{
    public string VoiceMessage { get; }

    public VoiceEventArgs(string voiceMessage)
    {
        VoiceMessage = voiceMessage;
    }
}
