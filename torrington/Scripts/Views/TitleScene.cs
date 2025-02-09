using Godot;

public partial class TitleScene : Control
{
	/// <summary>
	/// Signal emitted when the start button is pressed.
	/// </summary>
	[Signal]
	public delegate void StartButtonPressedEventHandler();

	public override void _Ready() {

		AnimatedSprite2D logoAnimation = GetNode<AnimatedSprite2D>("backgroundPanel/LogoAnimation");
		logoAnimation.Play();
	}

	/// <summary>
	/// Handles the event when the start button is pressed.
	/// Loads the game scene and adds it to the scene tree.
	/// </summary>
	public void OnStartButtonPressed() {
		GetNode<AudioStreamPlayer2D>("backgroundPanel/buttonSound").Play();
		GD.Print("Le jeu démarre.");
		var scene = GD.Load<PackedScene>("res://Scenes/gameScene.tscn");
		var instance = scene.Instantiate<Control>();
		GetTree().Root.AddChild(instance);
	}

	/// <summary>
	/// Handles the event when the options button is pressed.
	/// Loads the settings scene and adds it to the scene tree.
	/// </summary>
	public void OnOptionsButtonPressed() {
		GetNode<AudioStreamPlayer2D>("backgroundPanel/buttonSound").Play();
		var scene = GD.Load<PackedScene>("res://Scenes/settingsScene.tscn");
		var instance = scene.Instantiate<Control>();
		GetTree().Root.AddChild(instance);
	}

	/// <summary>
	/// Handles the event when the exit button is pressed.
	/// Quits the application.
	/// </summary>
	public void OnExitButtonPressed() {
		GetNode<AudioStreamPlayer2D>("backgroundPanel/buttonSound").Play();
		GetTree().Quit();
	}
}
