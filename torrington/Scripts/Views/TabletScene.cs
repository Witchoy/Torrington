using Godot;
using System.Collections.Generic;

/// <summary>
/// Represents the tablet scene in the game, displaying accepted proposals
/// and enabling their manipulation via sliders and buttons.
/// </summary>
public partial class TabletScene : Control
{
    private List<Proposal> _acceptedProposals; ///< List of proposals accepted by the player.
    private VBoxContainer _vBoxAcceptedProposals; ///< Container for displaying accepted proposals.
    private Panel _topPanel; ///< Panel containing the accepted proposals.
    private TorringtonController _cont; ///< Reference to the main game controller.
    private ScrollContainer _scrollContainer; ///< Scrollable container for the accepted proposals.

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Connects to the <see cref="GameScene"/> and initializes UI components.
    /// </summary>
    public override void _Ready()
    {
        var gameScene = GetTree().Root.GetNode<GameScene>("GameScene");
        if (gameScene != null)
        {
            gameScene.Connect(nameof(GameScene.TabletButtonPressed), new Callable(this, nameof(OnTabletButtonPressed)));
        }

        SetUpVariables(); // Ensure variables are initialized
        DisplayAcceptedProposals(); // Safely call this after initialization
    }

    /// <summary>
    /// Initializes variables and retrieves the accepted proposals from the controller.
    /// </summary>
    private void SetUpVariables()
    {
        _vBoxAcceptedProposals = GetNode<VBoxContainer>("topPanel/ScrollContainer/VBoxAcceptedProposals");
        _vBoxAcceptedProposals.SetPosition(new Vector2(75, 150));
        _scrollContainer = GetNode<ScrollContainer>("topPanel/ScrollContainer");
        _topPanel = GetNode<Panel>("topPanel");
        _acceptedProposals = _cont.GetAcceptedProposals();
    }

    /// <summary>
    /// Sets the main controller for the tablet scene.
    /// </summary>
    /// <param name="controller">The main game controller.</param>
    public void SetController(TorringtonController controller)
    {
        _cont = controller;
    }

    /// <summary>
    /// Reacts to the tablet button press signal by displaying the accepted proposals.
    /// </summary>
    private void OnTabletButtonPressed()
    {
        DisplayAcceptedProposals();
    }

    /// <summary>
    /// Displays the list of accepted proposals in the UI.
    /// Adds sliders and buttons for each proposal.
    /// </summary>
    private void DisplayAcceptedProposals()
    {
        ClearChildren(_vBoxAcceptedProposals);

        _acceptedProposals = _cont.GetAcceptedProposals();
        if (_acceptedProposals == null || _acceptedProposals.Count == 0)
        {
            GD.Print("No accepted proposals to display");
            return;
        }

        foreach (var proposal in _acceptedProposals)
        {
            var gBox = new GridContainer { Columns = 2 };

            var labelName = new Label { Text = proposal.GetName() };

            var labelStu = new Label { Text = $"Nombre d'étudiants  : {proposal.GetStudent()}" };
            var labelGra = new Label { Text = $"Pourcentage de diplômés : {proposal.GetGratuated()}" };

            var hsliderStu = CreateSlider(proposal.GetStudent(), proposal.GetStudentMax(), labelStu, proposal, true);
            var hsliderGra = CreateSlider(proposal.GetGratuated(), proposal.GetGratuatedMax(), labelGra, proposal, false);

            var removeButton = new Button { Text = "Supprimer" };
            removeButton.Pressed += () => OnRemoveProposalPressed(proposal);

            gBox.AddChild(labelName);
            gBox.AddChild(removeButton);
            gBox.AddChild(labelStu);
            gBox.AddChild(hsliderStu);
            gBox.AddChild(labelGra);
            gBox.AddChild(hsliderGra);

            _vBoxAcceptedProposals.AddChild(gBox);
            _vBoxAcceptedProposals.AddChild(new HSeparator());
        }
    }

    /// <summary>
    /// Creates a slider for modifying proposal attributes.
    /// </summary>
    /// <param name="initialValue">Initial value of the slider.</param>
    /// <param name="maxValue">Maximum value of the slider.</param>
    /// <param name="label">The label to update with the slider's value.</param>
    /// <param name="proposal">The proposal to modify.</param>
    /// <param name="isStudentSlider">Whether the slider modifies students or graduates.</param>
    /// <returns>The configured slider.</returns>
    private HSlider CreateSlider(int initialValue, int maxValue, Label label, Proposal proposal, bool isStudentSlider)
    {
        var slider = new HSlider
        {
            MinValue = 3,
            MaxValue = maxValue,
            Value = initialValue,
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            CustomMinimumSize = new Vector2(150, 0)
        };

        slider.ValueChanged += (value) => OnSliderChanged(label, isStudentSlider ? "Nombre d'étudiants" : "Taux de diplômés", (int)value, !isStudentSlider);
        slider.ValueChanged += (value) =>
        {
            if (isStudentSlider) proposal.SetStudent((int)value);
            else proposal.SetGratuated((int)value);
        };

        return slider;
    }

    /// <summary>
    /// Updates a label when the slider value changes.
    /// </summary>
    /// <param name="lbl">The label to update.</param>
    /// <param name="txt">The text prefix for the label.</param>
    /// <param name="newValue">The new value to display.</param>
    private void OnSliderChanged(Label lbl, string txt, int newValue, bool isTablet)
    {
        if(isTablet) {
            lbl.Text = $"{txt} : {newValue} %";
        } else {
            lbl.Text = $"{txt} : {newValue}  ";
        }
    }

    /// <summary>
    /// Clears all children from the specified container.
    /// </summary>
    /// <param name="cont">The container to clear.</param>
    private void ClearChildren(Container cont)
    {
        if (cont == null)
        {
            GD.Print("Container is null");
            return;
        }

        foreach (Node child in cont.GetChildren())
        {
            cont.RemoveChild(child);
            child.QueueFree();
        }
    }

    /// <summary>
    /// Handles the removal of a proposal when the remove button is pressed.
    /// Refreshes the displayed proposals.
    /// </summary>
    /// <param name="proposal">The proposal to remove.</param>
    private void OnRemoveProposalPressed(Proposal proposal)
    {
        GetNode<AudioStreamPlayer2D>("buttonSound").Play();
        _cont.RemoveProposal(proposal);
        DisplayAcceptedProposals();
    }

    /// <summary>
    /// Hides the tablet scene when the off button is pressed.
    /// </summary>
    public void OnOffButtonPressed()
    {
        GetNode<AudioStreamPlayer2D>("buttonSound").Play();
        GetTree().Root.RemoveChild(this);
    }
}
