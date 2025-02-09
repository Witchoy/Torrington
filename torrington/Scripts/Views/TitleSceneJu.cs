using Godot;
using System;

/*
public partial class TitleScene : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		new TorringtonController();
		var animatedLogo = GetNode<AnimatedSprite2D>("backgroundPanel/AnimatedSprite2D");
		animatedLogo.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void onStartButtonPressed() {
		GD.Print("Le jeu démarre.");
		GetTree().ChangeSceneToFile("res://Scenes/gameScene.tscn");
	}

	public void onOptionsButtonPressed() {
		var scene = GD.Load<PackedScene>("res://Scenes/settingsScene.tscn");
		var instance = scene.Instantiate<Control>();
		GetTree().Root.AddChild(instance);
	}

	public void onScoresButtonPressed() {
		GetTree().ChangeSceneToFile("res://Scenes/scoresScene.tscn");
	}

	public void onExitButtonPressed() {
		GetTree().Quit();
	}

	public void onCreditsButtonPressed() {
		
	}



}
*/