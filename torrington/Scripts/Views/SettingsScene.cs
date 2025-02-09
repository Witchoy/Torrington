using Godot;

/// <summary>
/// Represents the settings menu scene, providing controls for volume adjustment, game quitting, 
/// resuming, and restarting.
/// </summary>
public partial class SettingsScene : Control
{
    private Node _previousScene; ///< Reference to the previous scene for potential navigation.

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Initializes the music gauge based on the current audio settings.
    /// </summary>
    public override void _Ready()
    {
        AudioServer.SetBusVolumeDb(0, AudioServer.GetBusVolumeDb(0) + 10);
        UpdateMusicGauge();
    }

    /// <summary>
    /// Quits the game when the Quit button is pressed.
    /// </summary>
    public void OnQuitGameButtonPressed()
    {
        GetTree().Quit();
    }

    /// <summary>
    /// Resumes the game by removing the settings scene from the root.
    /// </summary>
    public void OnResumeButtonPressed()
    {
        GetTree().Root.RemoveChild(this);
    }

    /// <summary>
    /// Restarts the game by transitioning to the title scene.
    /// </summary>
    public void OnRestartButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/titleScene.tscn");
    }

    /// <summary>
    /// Increases the music volume by 10 decibels, up to a maximum of 100 dB.
    /// Updates the music gauge to reflect the new volume.
    /// </summary>
    public void OnIncreaseButtonPressed()
    {
        if (AudioServer.GetBusVolumeDb(0) != 100)
        {
            AudioServer.SetBusVolumeDb(0, AudioServer.GetBusVolumeDb(0) + 10);
        }
        UpdateMusicGauge();
    }

    /// <summary>
    /// Decreases the music volume by 10 decibels, down to a minimum of 0 dB.
    /// Updates the music gauge to reflect the new volume.
    /// </summary>
    public void OnDecreaseButtonPressed()
    {
        if (AudioServer.GetBusVolumeDb(0) != 0)
        {
            AudioServer.SetBusVolumeDb(0, AudioServer.GetBusVolumeDb(0) - 10);
        }
        UpdateMusicGauge();
    }

    /// <summary>
    /// Updates the music gauge progress bar to reflect the current volume level.
    /// </summary>
    private void UpdateMusicGauge()
    {
        var gauge = GetNode<ProgressBar>("mainPanel/musicPanel/musicGauge");
        gauge.Value = AudioServer.GetBusVolumeDb(0);
    }
}
