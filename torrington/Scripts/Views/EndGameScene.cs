using Godot;
using System;

public partial class EndGameScene : Control
{
	TorringtonController _cont;

	public void SetRatios(double[] ratios)
	{
		GD.Print("Ratios1 : " + ratios[0]);
		GD.Print("Ratios2 : " + ratios[1]);
		GD.Print("Ratios3 : " + ratios[2]);
		GD.Print("Ratios4 : " + ratios[3]);

		var attractivenessLabel = GetNode<Label>("Panel/Panel/attractivenessLabel");
		var successLabel = GetNode<Label>("Panel/Panel/successLabel");
		var insertionLabel = GetNode<Label>("Panel/Panel/insertionLabel");
		var satisfactionLabel = GetNode<Label>("Panel/Panel/satisfactionLabel");

		attractivenessLabel.Text = "Taux d'attractivité :  "+ ratios[0];
		successLabel.Text = "Taux de réussite : " + ratios[1];
		insertionLabel.Text = "Taux d'insertion : " + ratios[2];
		satisfactionLabel.Text = "Taux de satisfaction : " + ratios[3];
	}

	public void OnMenuPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/titleScene.tscn");
	}
}
