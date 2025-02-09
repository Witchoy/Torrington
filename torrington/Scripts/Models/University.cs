using System;
using System.Collections.Generic;
using Godot;

public partial class University : Node
{
    /// <summary>
    /// Represents a university in the game. It manages the student capacity 
    /// and a list of accepted proposals. Proposals can be added or removed, 
    /// and the list of accepted proposals can be retrieved.
    /// </summary>

    private int _studentCapacity;
    private List<Proposal> _acceptedProposals;

    /// <summary>
    /// Initializes a new instance of the <see cref="University"/> class.
    /// The student capacity is set to a random value between 500 and 1000,
    /// and the list of accepted proposals is initialized as an empty list.
    /// </summary>
    public University()
    {
        Random aleatoire = new Random();
        _studentCapacity = aleatoire.Next(1000, 1500);
        _acceptedProposals = new List<Proposal>();
    }

    /// <summary>
    /// Adds a proposal to the list of accepted proposals in the university.
    /// </summary>
    /// <param name="proposal">The <see cref="Proposal"/> to add to the list.</param>
    public void AddProposal(Proposal proposal)
    {
        if (proposal == null) GD.Print("Proposal is null");
        _acceptedProposals.Add(proposal);
    }

    /// <summary>
    /// Removes a proposal from the list of accepted proposals in the university.
    /// </summary>
    /// <param name="proposal">The <see cref="Proposal"/> to remove from the list.</param>
    public void RemoveProposal(Proposal proposal)
    {
        if (_acceptedProposals.Contains(proposal))
        {
            _acceptedProposals.Remove(proposal);
        }
    }

    /// <summary>
    /// Gets the list of accepted proposals in the university.
    /// </summary>
    /// <returns>A list of <see cref="Proposal"/> objects that have been accepted by the university.</returns>
    public List<Proposal> GetAcceptedProposals()
    {
        return _acceptedProposals;
    }
}
