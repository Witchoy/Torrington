using Godot;
using System.Collections.Generic;

/// <summary>
/// Represents the scene where proposals are displayed and managed on a computer interface.
/// </summary>
public partial class ComputerScene : Control
{
    private TorringtonController _cont;
    private List<Proposal> _currentProposals;

    private Panel _leftPanel;
    private Label _contentLabel;

    private Label _labelProposal1;
    private Label _labelProposal2;
    private Label _labelProposal3;

    private Button _acceptButton1;
    private Button _acceptButton2;
    private Button _acceptButton3;

    private Button _declineButton1;
    private Button _declineButton2;
    private Button _declineButton3;

    private VBoxContainer _vBoxContainer1;
    private VBoxContainer _vBoxContainer2;
    private VBoxContainer _vBoxContainer3;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Sets up variables and displays random proposals.
    /// </summary>
    public override void _Ready()
    {
        var gameScene = GetTree().Root.GetNode<GameScene>("GameScene");
        if (gameScene != null)
        {
            gameScene.Connect(nameof(GameScene.NextRoundCalled), Callable.From(OnNextRoundCalled));
        }
        SetUpVariables();
        DisplayRandomProposal();
    }

    /// <summary>
    /// Initializes UI components and variables.
    /// </summary>
    private void SetUpVariables()
    {
        _currentProposals = new List<Proposal>();

        _leftPanel = GetNode<Panel>("leftPanel");

        _vBoxContainer1 = GetNode<VBoxContainer>("leftPanel/VBoxProposal1");
        _vBoxContainer2 = GetNode<VBoxContainer>("leftPanel/VBoxProposal2");
        _vBoxContainer3 = GetNode<VBoxContainer>("leftPanel/VBoxProposal3");

        _labelProposal1 = GetNode<Label>("leftPanel/VBoxProposal1/labelProposal1");
        _labelProposal2 = GetNode<Label>("leftPanel/VBoxProposal2/labelProposal2");
        _labelProposal3 = GetNode<Label>("leftPanel/VBoxProposal3/labelProposal3");

        _acceptButton1 = GetNode<Button>("leftPanel/VBoxProposal1/hboxButtons1/acceptButton1");
        _acceptButton2 = GetNode<Button>("leftPanel/VBoxProposal2/hboxButtons2/acceptButton2");
        _acceptButton3 = GetNode<Button>("leftPanel/VBoxProposal3/hboxButtons3/acceptButton3");

        _declineButton1 = GetNode<Button>("leftPanel/VBoxProposal1/hboxButtons1/declineButton1");
        _declineButton2 = GetNode<Button>("leftPanel/VBoxProposal2/hboxButtons2/declineButton2");
        _declineButton3 = GetNode<Button>("leftPanel/VBoxProposal3/hboxButtons3/declineButton3");

        _contentLabel = GetNode<Label>("rightPanel/contentLabel");
    }

    /// <summary>
    /// Sets the controller for this scene.
    /// </summary>
    /// <param name="controller">The TorringtonController instance to set.</param>
    public void SetController(TorringtonController controller)
    {
        _cont = controller;
    }

    /// <summary>
    /// Handles the button to close the scene.
    /// </summary>
    public void OnOffButtonPressed()
    {
        GetNode<AudioStreamPlayer2D>("buttonSound").Play();
        _contentLabel.Text = "Veuillez cliquer sur une formation";
        GetTree().Root.RemoveChild(this);
    }

    /// <summary>
    /// Displays random proposals in the UI.
    /// </summary>
    public void DisplayRandomProposal()
    {
        GD.Print("DisplayRandomProposal");
        _currentProposals.Clear();

        if(_cont.GetProposalLeft() > 2){
            GD.Print("DisplayRandomProposal: More than 2 proposals left");
            for (int i = 0; i < 3; i++)
            {
                Proposal proposal = _cont.GetProposals();
                if (proposal == null) GD.Print("DisplayRandomProposal: Proposal is null");
                _currentProposals.Add(proposal);
            }

            BindProposalToUI(_labelProposal1, _acceptButton1, _declineButton1, _currentProposals[0]);
            BindProposalToUI(_labelProposal2, _acceptButton2, _declineButton2, _currentProposals[1]);
            BindProposalToUI(_labelProposal3, _acceptButton3, _declineButton3, _currentProposals[2]);
        }else if(_cont.GetProposalLeft() > 1){
            GD.Print("DisplayRandomProposal: More than 1 proposal left");
            for (int i = 0; i < 2; i++)
            {
                Proposal proposal = _cont.GetProposals();
                if (proposal == null) GD.Print("DisplayRandomProposal: Proposal is null");
                _currentProposals.Add(proposal);
            }

            BindProposalToUI(_labelProposal1, _acceptButton1, _declineButton1, _currentProposals[0]);
            BindProposalToUI(_labelProposal2, _acceptButton2, _declineButton2, _currentProposals[1]);
            _vBoxContainer3.Visible = false;
        }else{
            GD.Print("DisplayRandomProposal: 1 proposal left");
            Proposal proposal = _cont.GetProposals();
            if (proposal == null) GD.Print("DisplayRandomProposal: Proposal is null");
            _currentProposals.Add(proposal);

            BindProposalToUI(_labelProposal1, _acceptButton1, _declineButton1, _currentProposals[0]);
            _vBoxContainer2.Visible = false;
            _vBoxContainer3.Visible = false;
        }
    }

    /// <summary>
    /// Binds a proposal to a set of UI components.
    /// </summary>
    /// <param name="label">The label to display the proposal name.</param>
    /// <param name="acceptButton">The button to accept the proposal.</param>
    /// <param name="declineButton">The button to decline the proposal.</param>
    /// <param name="proposal">The proposal to bind.</param>
    private void BindProposalToUI(Label label, Button acceptButton, Button declineButton, Proposal proposal)
    {
        if (proposal == null) GD.Print("BindProposalToUI: Proposal is null");

        label.Text = proposal.GetName();

        ResetButtonConnections(acceptButton);
        ResetButtonConnections(declineButton);

        label.MouseFilter = MouseFilterEnum.Pass;
        label.GuiInput += (InputEvent @event) => OnLabelClicked(@event, proposal);

        acceptButton.Pressed += () => OnProposalAccepted(proposal, acceptButton.GetParent<HBoxContainer>().GetParent<VBoxContainer>());
        declineButton.Pressed += () => OnProposalRefused(proposal, declineButton.GetParent<HBoxContainer>().GetParent<VBoxContainer>());
    }

    /// <summary>
    /// Resets signal connections for a button.
    /// </summary>
    /// <param name="button">The button to reset.</param>
    private void ResetButtonConnections(Button button)
    {
        foreach (var connection in button.GetSignalConnectionList("pressed"))
        {
            button.Disconnect("pressed", (Callable)connection["callable"]);
        }
    }

    /// <summary>
    /// Handles mouse click events on a label.
    /// </summary>
    /// <param name="event">The input event.</param>
    /// <param name="proposal">The proposal associated with the label.</param>
    private void OnLabelClicked(InputEvent @event, Proposal proposal)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            ShowProposalDetails(proposal);
        }
    }

    /// <summary>
    /// Handles accepting a proposal.
    /// </summary>
    /// <param name="proposal">The proposal to accept.</param>
    /// <param name="vbox">The VBoxContainer associated with the proposal UI.</param>
    private void OnProposalAccepted(Proposal proposal, VBoxContainer vbox)
    {
        GetNode<AudioStreamPlayer2D>("buttonSound").Play();
        _cont.AddProposal(proposal);
        _currentProposals.Remove(proposal);
        vbox.Visible = false;
    }

    /// <summary>
    /// Handles declining a proposal.
    /// </summary>
    /// <param name="proposal">The proposal to decline.</param>
    /// <param name="vbox">The VBoxContainer associated with the proposal UI.</param>
    private void OnProposalRefused(Proposal proposal, VBoxContainer vbox)
    {
        GetNode<AudioStreamPlayer2D>("buttonSound").Play();
        _currentProposals.Remove(proposal);
        _cont.RemoveProposal(proposal);
        vbox.Visible = false;
    }

    /// <summary>
    /// Handles the start of a new round.
    /// </summary>
    private void OnNextRoundCalled()
    {
        _contentLabel.Text = "Veuillez cliquer sur une formation";
        foreach (Node child in _leftPanel.GetChildren())
        {
            if (child is VBoxContainer vbox)
            {
                vbox.Visible = true;
            }
        }
        DisplayRandomProposal();
    }

    /// <summary>
    /// Displays detailed information about a proposal.
    /// </summary>
    /// <param name="proposal">The proposal to display details for.</param>
    private void ShowProposalDetails(Proposal proposal)
    {
        _contentLabel.Text = "";
        string proposalContent = "Nom de la formation : ";
        proposalContent += $"{proposal.GetName()}\n";
        proposalContent += $"Domaine : {proposal.GetField()}\n";
        proposalContent += $"Nombre de candidats : {proposal.GetCandidates()}\n";
        proposalContent += $"Nombre d'emplois : {proposal.GetJobs()}\n";
        _contentLabel.Text = proposalContent;
    }
}
