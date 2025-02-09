using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using Scripts.Models;

public partial class Game
{
    /// <summary>
    /// Represents the game logic, including the management of rounds, ratios, and university proposals.
    /// </summary>

    private double _attractivenessRatio;
    private double _successRatio;
    private double _insertionRatio;
    private double _satisfactionRatio;

    private int _nbStudent;
    private int _nbGratuated;
    private int _nbJobs;

    private int _year;

    private University _university;

    private List<ITorringtonObserver> _observers;

    private ProposalGenerator _proposalGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/> class with optional observers.
    /// </summary>
    /// <param name="observers">An array of observers to be added to the game.</param>
    public Game(params ITorringtonObserver[] observers) {
        _proposalGenerator = new ProposalGenerator();
        _university = new University();
        _observers = new List<ITorringtonObserver>();

        if (observers != null) {
            foreach (ITorringtonObserver observer in observers) {
                _observers.Add(observer);
            }
        }
    }

    /// <summary>
    /// Starts the game, initializing the game state and notifying observers.
    /// </summary>
    public void StartGame() {
        setUpVariable();
        NotifyNextTurn(_year);
        NotifyRatioUpdated();
    }

    /// <summary>
    /// Initializes game variables to their starting values.
    /// </summary>
    private void setUpVariable() {
        _attractivenessRatio = 0;
        _successRatio = 0;
        _insertionRatio = 0;
        _satisfactionRatio = 0;
        _year = 2024;
    }

    /// <summary>
    /// Advances the game to the next round, notifying observers of the updates.
    /// </summary>
    public void NextTurn() {
        if(_university.GetAcceptedProposals().Count == 0) {
            NotifyError("Error", "Au moins une formation doit être acceptée !");
            return;
        }
        foreach (Proposal prop in _university.GetAcceptedProposals()) {
            if (prop.GetStudent() == 0) {
                NotifyError("Error", "Le nombre d'étudiant des formations ne peut être de zéro !");
                return;
            }
        }
        if(_proposalGenerator.GetProposalLeft() == 0) {
            EndGame();
            return;
        }

        _year++;
        NotifyNextTurn(_year);
        NotifyRatioUpdated();
    }

    private void NotifyError(string win, string error)
    {
        foreach (ITorringtonObserver observer in _observers)
        {
            observer.ReactToError(win, error);
        }
    }

    /// <summary>
    /// Ends the game, notifying observers of the end of the game.
    /// </summary>
    public void EndGame() {
        if(_university.GetAcceptedProposals().Count == 0) {
            NotifyError("Error", "Au moins une formation doit être acceptée !");
            return;
        }
        foreach (Proposal prop in _university.GetAcceptedProposals()) {
            if (prop.GetStudent() == 0) {
                NotifyError("Error", "Le nombre d'étudiant des formations ne peut être de zéro !");
                return;
            }
        }
        NotifyEndGame();
    }

    /// <summary>
    /// Gets the attractiveness ratio for the university's accepted proposals.
    /// </summary>
    /// <returns>The attractiveness ratio.</returns>
    public double GetAttractivenessRatio() {
        calculateAttractiveness();
        return _attractivenessRatio;
    }

    /// <summary>
    /// Gets the success ratio for the university's accepted proposals.
    /// </summary>
    /// <returns>The success ratio.</returns>
    public double GetSuccessRatio() {
        calculateSuccess();
        return _successRatio;
    }

    /// <summary>
    /// Gets the insertion ratio for the university's accepted proposals.
    /// </summary>
    /// <returns>The insertion ratio.</returns>
    public double GetInsertionRatio() {
        calculateInsertion();
        return _insertionRatio;
    }

    /// <summary>
    /// Gets the satisfaction ratio for the university's accepted proposals.
    /// </summary>
    /// <returns>The satisfaction ratio.</returns>
    public double GetSatisfactionRatio() {
        calculateSatisfaction();
        return _satisfactionRatio;
    }

    /// <summary>
    /// Calculates the attractiveness ratio based on accepted proposals.
    /// </summary>
    private void calculateAttractiveness() {
        if (_university.GetAcceptedProposals().Count == 0) {
            _attractivenessRatio = 0;
            return;
        }

        double average = 0;
        foreach (Proposal prop in _university.GetAcceptedProposals()) {
            double tmp = prop.GetCandidates() / prop.GetStudent();
            average += tmp;
        }

        _attractivenessRatio = average / _university.GetAcceptedProposals().Count;
        _attractivenessRatio = Math.Round(_attractivenessRatio, 2);
    }

    /// <summary>
    /// Calculates the success ratio based on accepted proposals.
    /// </summary>
    private void calculateSuccess() {
        if (_university.GetAcceptedProposals().Count == 0) {
            _successRatio = 0;
            return;
        }

        double average = 0;
        foreach (Proposal prop in _university.GetAcceptedProposals()) {
            double tmp = prop.GetGratuated() / prop.GetStudent();
            average += tmp;
        }

        _successRatio = average / _university.GetAcceptedProposals().Count;
        _successRatio = Math.Round(_successRatio,2);
    }

    /// <summary>
    /// Calculates the insertion ratio based on accepted proposals.
    /// </summary>
    private void calculateInsertion() {
        if (_university.GetAcceptedProposals().Count == 0) {
            _insertionRatio = 0;
            return;
        }

        double average = 0;
        foreach (Proposal prop in _university.GetAcceptedProposals()) {
            double tmp = prop.GetJobs() / prop.GetGratuated();
            average += tmp;
        }

        _insertionRatio = average / _university.GetAcceptedProposals().Count;
        _insertionRatio = Math.Round(_insertionRatio,2);
    }

    /// <summary>
    /// Calculates the satisfaction ratio based on the other ratios.
    /// </summary>
    private void calculateSatisfaction() {
        _satisfactionRatio = Math.Min(_attractivenessRatio, Math.Min(_successRatio, _insertionRatio));
        _satisfactionRatio = Math.Round(_satisfactionRatio, 2);
    }

    /// <summary>
    /// Notifies all observers of the next round.
    /// </summary>
    public void NotifyNextTurn(int year) {
        foreach (ITorringtonObserver observer in _observers) {
            observer.ReactToNextRound(year);
        }
    }

    /// <summary>
    /// Notifies all observers of updated ratios (attractiveness, success, insertion).
    /// </summary>
    public void NotifyRatioUpdated() {
        foreach (ITorringtonObserver observer in _observers) {
            observer.ReactToRatioUpdated(new double[] { GetAttractivenessRatio(), GetSuccessRatio(), GetInsertionRatio(), GetSatisfactionRatio() });
        }
    }

    /// <summary>
    /// Notifies all observers that the game has ended.
    /// </summary>
    public void NotifyEndGame() {
        foreach (ITorringtonObserver observer in _observers) {
            observer.ReactToEndGame(new double[] { GetAttractivenessRatio(), GetSuccessRatio(), GetInsertionRatio(), GetSatisfactionRatio() } );
        }
    }

    /// <summary>
    /// Gets a list of the university's accepted proposals.
    /// </summary>
    /// <returns>A list of accepted proposals.</returns>
    public List<Proposal> GetAcceptedProposals() {
        return _university.GetAcceptedProposals();
    }

    /// <summary>
    /// Adds a proposal to the university.
    /// </summary>
    /// <param name="proposal">The proposal to add.</param>
    public void AddProposal(Proposal proposal) {
        _university.AddProposal(proposal);
    }

    /// <summary>
    /// Removes a proposal from the university.
    /// </summary>
    /// <param name="proposal">The proposal to remove.</param>
    public void RemoveProposal(Proposal proposal) {
        _university.RemoveProposal(proposal);
        _proposalGenerator.RemoveProposal(proposal);
    }

    /// <summary>
    /// Gets a new proposal from the proposal generator.
    /// </summary>
    /// <returns></returns>
    public Proposal GetProposals() {
        return _proposalGenerator.GenerateProposal();
    }

    /// <summary>
    /// Gets the number of proposals generated by the proposal generator.
    /// </summary>
    public int GetProposalCount() {
        return _proposalGenerator.GetProposalCount();
    }

    /// <summary>
    /// Gets the number of proposals remaining in the proposal generator.
    /// </summary>
    public int GetProposalLeft() {
        return _proposalGenerator.GetProposalLeft();
    }

}
