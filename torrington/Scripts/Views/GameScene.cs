using Godot;
using System;

/// <summary>
/// Represents the main game scene that acts as the central controller for managing interactions 
/// between the tablet, computer, and settings scenes.
/// Implements the <see cref="ITorringtonObserver"/> interface to handle observer-related updates.
/// </summary>
public partial class GameScene : Control, ITorringtonObserver
{
    private ComputerScene _computerScene; ///< Reference to the computer scene instance.
    private TabletScene _tabletScene; ///< Reference to the tablet scene instance.
    private SettingsScene _settingsScene; ///< Reference to the settings scene instance.
    private EndGameScene _endGameScene; ///< Reference to the end game scene instance.

    private TorringtonController _cont; ///< The main controller for game logic.

    /// <summary>Event emitted when the next round is called.</summary>
	[Signal] 
    public delegate void NextRoundCalledEventHandler();

    /// <summary>Event emitted when the tablet button is pressed.</summary>
    [Signal] 
    public delegate void TabletButtonPressedEventHandler();

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Initializes the controller and loads the tablet, computer, and settings scenes.
    /// </summary>
    public override void _Ready()
    {
        // Creates the controller and add GameScene as an observer
        _cont = new TorringtonController(this);

        // Load the Computer and Tablet scenes
        _tabletScene = (TabletScene)LoadScene("tabletScene", _cont);
        _computerScene = (ComputerScene)LoadScene("computerScene", _cont);
        _settingsScene = (SettingsScene)LoadScene("ingameSettingsScene", null);
        _endGameScene = (EndGameScene)LoadScene("endGameScene", null);
    }

    /// <summary>
    /// Loads a scene from a PackedScene and instantiates it.
    /// Optionally sets a controller for the scene if required.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    /// <param name="cont">The controller to set for the loaded scene.</param>
    /// <returns>The loaded scene as a <see cref="Control"/> instance.</returns>
    private Control LoadScene(string sceneName, TorringtonController cont)
    {
        try
        {
            var scn = GD.Load<PackedScene>($"res://Scenes/{sceneName}.tscn");
            var scene = scn.Instantiate<Control>();
            if (scene == null)
            {
                GD.PrintErr($"{sceneName} instance is null");
            }

            if (scene is TabletScene tabletScene)
            {
                tabletScene.SetController(cont);
            }
            else if (scene is ComputerScene computerScene)
            {
                computerScene.SetController(cont);
            }

            return scene;
        }
        catch (Exception e)
        {
            GD.PrintErr($"Failed to load {sceneName}: {e.Message}");
            return null;
        }
    }

	/// <summary>
    /// Called when the tablet button is pressed.
    /// Adds the tablet scene to the root and emits a signal.
    /// </summary>
    public void OnTabletButtonPressed()
    {
        GetNode<AudioStreamPlayer2D>("backgroundPanel/buttonSound").Play();
        if (_tabletScene != null) {
            GetTree().Root.AddChild(_tabletScene);
            EmitSignal(nameof(TabletButtonPressed));
        } else {
            GD.PrintErr("TabletScene is null");
        }
    }

    /// <summary>
    /// Called when the computer button is pressed.
    /// Adds the computer scene to the root.
    /// </summary>
    public void OnComputerButtonPressed()
    {
        GetNode<AudioStreamPlayer2D>("backgroundPanel/buttonSound").Play();
        if (_computerScene != null) {
            GetTree().Root.AddChild(_computerScene);
        } else {
            GD.PrintErr("ComputerScene is null");
        }
    }

    /// <summary>
    /// Called when the options button is pressed.
    /// Adds the settings scene to the root.
    /// </summary>
	public void OnOptionsButtonPressed() {
        GetNode<AudioStreamPlayer2D>("backgroundPanel/buttonSound").Play();
        if (_settingsScene != null) {
            GetTree().Root.AddChild(_settingsScene);
        } else {
            GD.PrintErr("settingsScene is null");
        }
	}

    /// <summary>
    /// Called when the next round button is pressed.
    /// Advances the game to the next round.
    /// </summary>
    public void OnNextTurnButtonPressed() {
        GetNode<AudioStreamPlayer2D>("backgroundPanel/buttonSound").Play();
        foreach (Node child in _computerScene.GetNode<Panel>("leftPanel").GetChildren()) {
            if (child is VBoxContainer vb && vb.Visible) {
                ReactToError("Error", "Vous devez accepter ou refuser toutes les propositions !");
                return;
            }
        }
        Window window = GetNode<Window>("backgroundPanel/popupNext");
        window.Visible = true;
        window.CloseRequested += () => {
            window.Visible = false;
            GetNode<ColorRect>("backgroundPanel/ColorRect").Visible = false;
        };
    }

    public void OnNextTurnValButtonPressed() {
        GetNode<Window>("backgroundPanel/popupNext").Visible = false;
        GetNode<AudioStreamPlayer2D>("backgroundPanel/buttonSound").Play();
        _cont.NextTurn();
    }

    public void OnEndGameButtonPressed() {
        GetNode<Window>("backgroundPanel/popupNext").Visible = false;
        GetNode<AudioStreamPlayer2D>("backgroundPanel/buttonSound").Play();
        _cont.EndGame();
    }

    /// <summary>
    /// Reacts to the next round being triggered by emitting the relevant signal.
    /// </summary>
    public void ReactToNextRound(int year)
    {
        var yearLabel = GetNode<Label>("backgroundPanel/yearLabel");
        yearLabel.Text = $"Année : {year}";
        EmitSignal(nameof(NextRoundCalled));
    }

    /// <summary>
    /// Reacts to the end of the game by transitioning to the end game scene.
    /// </summary>
    public void ReactToEndGame (double[] ratios)
    {
        GD.Print("Game ended");
        if (_endGameScene != null) {
            _endGameScene.SetRatios(ratios);
            GetTree().Root.AddChild(_endGameScene);
        } else {
            GD.PrintErr("endGameScene is null");
        }
    }

    /// <summary>
    /// Reacts to the ratio updates by updating the UI labels with the new values.
    /// </summary>
    /// <param name="ratios">An array containing the updated ratios for attractiveness, success, and insertion.</param>
    public void ReactToRatioUpdated(double[] ratios)
    {
		// Load the Labels in variables
    	var attractivenesslabel = GetNode<Label>("backgroundPanel/infoVBoxContainer/ratioVBoxContainer/attractivenessLabel");
    	var successlabel = GetNode<Label>("backgroundPanel/infoVBoxContainer/ratioVBoxContainer/successLabel");
    	var insertionlabel = GetNode<Label>("backgroundPanel/infoVBoxContainer/ratioVBoxContainer/insertionLabel");
        var satisfactionlabel = GetNode<Label>("backgroundPanel/infoVBoxContainer/ratioVBoxContainer/satisfactionLabel");

		// Update the printed ratios
		attractivenesslabel.Text =  $"Taux d'attractivité : {ratios[0]}";
		successlabel.Text = $"Taux de réussite : {ratios[1]}";
		insertionlabel.Text = $"Taux d'insertion : {ratios[2]}";
        satisfactionlabel.Text = $"Taux de satisfaction : {ratios[3]}";
    }

    /// <summary>
    /// Returns a string representation of the game scene.
    /// </summary>
    /// <returns>The name of the scene.</returns>
    public override string ToString() {
        return "GameScene";
    }

    /// <summary>
    /// Reacts to an error by displaying a popup with the error message.
    /// </summary>
    /// <param name="error"></param>
    public void ReactToError(string win, string error)
    {
        Window popup = GetNode<Window>($"backgroundPanel/popup{win}");
        RichTextLabel rtlabel = GetNode<RichTextLabel>("backgroundPanel/popupError/RichTextLabel");
        rtlabel.Text = error;
        popup.CloseRequested += () => popup.Visible = false;
        popup.Visible = true;
    }
}
