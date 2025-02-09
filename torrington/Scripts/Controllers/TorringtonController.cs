using System.Collections.Generic;
using Godot;

public partial class TorringtonController : Node
{
    /// <summary>
    /// Controller class that manages the game flow and interacts with the <see cref="Game"/> class.
    /// It provides methods for starting the game, advancing to the next round, 
    /// and managing proposals in the game.
    /// </summary>

    private Game _game;

    /// <summary>
    /// Initializes a new instance of the <see cref="TorringtonController"/> class.
    /// It creates a new <see cref="Game"/> instance and starts the game with the provided observers.
    /// </summary>
    /// <param name="observers">An array of <see cref="ITorringtonObserver"/> instances to be notified about game events.</param>
    public TorringtonController(params ITorringtonObserver[] observers)
    {
        _game = new Game(observers);
        StartGame();
    }

    /// <summary>
    /// Starts the game by invoking the <see cref="Game.startGame"/> method.
    /// </summary>
    public void StartGame()
    {
        _game.StartGame();
    }

    /// <summary>
    /// Advances the game to the next round by invoking the <see cref="Game.NextRound"/> method.
    /// </summary>
    public void NextTurn()
    {
        _game.NextTurn();
    }

    public void EndGame() {
        _game.EndGame();
    }

    /// <summary>
    /// Gets the list of accepted proposals from the game.
    /// </summary>
    /// <returns>A list of <see cref="Proposal"/> objects that have been accepted in the game.</returns>
    public List<Proposal> GetAcceptedProposals()
    {
        return _game.GetAcceptedProposals();
    }

    /// <summary>
    /// Adds a proposal to the game by invoking the <see cref="Game.AddProposal"/> method.
    /// </summary>
    /// <param name="proposal">The <see cref="Proposal"/> to add to the game.</param>
    public void AddProposal(Proposal proposal)
    {
        _game.AddProposal(proposal);
    }

    /// <summary>
    /// Removes a proposal from the game by invoking the <see cref="Game.RemoveProposal"/> method.
    /// </summary>
    /// <param name="proposal">The <see cref="Proposal"/> to remove from the game.</param>
    public void RemoveProposal(Proposal proposal)
    {
        _game.RemoveProposal(proposal);
    }

    /// <summary>
    /// Gets the list of proposals from the game.
    /// </summary>
    /// <returns></returns>
    public Proposal GetProposals()
    {
        return _game.GetProposals();
    }

    /// <summary>
    /// Gets the number of proposals in the game.
    /// </summary>
    public int GetProposalCount() {
        return _game.GetProposalCount();
    }

    /// <summary>
    /// Gets the number of proposals left in the game.
    /// </summary>
    public int GetProposalLeft() {
        return _game.GetProposalLeft();
    }
}
