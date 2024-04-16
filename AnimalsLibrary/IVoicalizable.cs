namespace Task2.Model;

public interface IVoicalizable
{
    delegate void VoiceHandler(string message);
    event VoiceHandler Voice;
    void OnVocalize();
}
