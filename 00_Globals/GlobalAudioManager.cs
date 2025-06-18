using Godot;
using Godot.Collections;

public partial class GlobalAudioManager : Node
{
    public static GlobalAudioManager Instance;

    public int MusicAudioPlayerCount = 2;
    public int CurrentMusicPlayer = 0;
    public Array<AudioStreamPlayer> MusicPlayers = new Array<AudioStreamPlayer>();
    public string MusicBus = "Music";

    public float MusicFadeDuration = 1f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        ProcessMode = Node.ProcessModeEnum.Always;

        for (int i = 0; i < MusicAudioPlayerCount; i++)
        {
            var audioPlayer = new AudioStreamPlayer();
            AddChild(audioPlayer);
            audioPlayer.Bus = MusicBus;
            audioPlayer.VolumeDb = -40f;
            MusicPlayers.Add(audioPlayer);
        }
    }

    public void PlayMusic(AudioStream audio)
    {
        if (audio != MusicPlayers[CurrentMusicPlayer].Stream)
        {
            var oldPlayer = MusicPlayers[CurrentMusicPlayer];
            FadeOutAndStop(oldPlayer);

            CurrentMusicPlayer++;
            if (CurrentMusicPlayer > 1)
            {
                CurrentMusicPlayer = 0;
            }

            var currentPlayer = MusicPlayers[CurrentMusicPlayer];
            currentPlayer.Stream = audio;
            PlayAndFadeIn(currentPlayer);
        }
    }

    public void PlayAndFadeIn(AudioStreamPlayer audioStreamPlayer)
    {
        audioStreamPlayer.Play(0);
        var tween = CreateTween();
        tween.TweenProperty(audioStreamPlayer, "volume_db", 0, MusicFadeDuration);
    }

    public void FadeOutAndStop(AudioStreamPlayer audioStreamPlayer)
    {
        var tween = CreateTween();
        tween.TweenProperty(audioStreamPlayer, "volume_db", -40, MusicFadeDuration);
        tween.Finished += audioStreamPlayer.Stop;
    }
}
