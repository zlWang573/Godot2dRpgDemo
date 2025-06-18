using Godot;
using System.Threading.Tasks;

public partial class SceneTransition : CanvasLayer
{
	public static SceneTransition Instance;
    public AnimationPlayer animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Instance = this;
        animationPlayer = GetNode<AnimationPlayer>("Control/AnimationPlayer");
    }

	public async Task<bool> FadeOut()
	{
		animationPlayer.Play("fade_out");
		await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		return true;
	}

    public async Task<bool> FadeIn()
    {
        animationPlayer.Play("fade_in");
        await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
        return true;
    }
}
