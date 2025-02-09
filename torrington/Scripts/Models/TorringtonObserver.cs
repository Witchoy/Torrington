/// <summary>
/// The <see cref="ITorringtonObserver"/> interface defines methods for observing 
/// various events in the game. Implementing classes must define how to react to updates 
/// such as changes in ratios, the progression to the next round, the end of the game, 
/// and when there are no students left.
/// </summary>
public interface ITorringtonObserver {

    /// <summary>
    /// Called when the ratios (attractiveness, success, and insertion) are updated.
    /// Implementing classes must define how to react to these updates.
    /// </summary>
    /// <param name="ratios">An array of integers representing the updated ratios. 
    /// The array includes the attractiveness, success, and insertion ratios in that order.</param>
    public void ReactToRatioUpdated(double[] ratios);

    /// <summary>
    /// Called when the game progresses to the next round. Implementing classes must 
    /// define how to react to this event.
    /// </summary>
    public void ReactToNextRound(int year);

    /// <summary>
    /// Called when the game ends. Implementing classes must define how to react 
    /// to the end of the game.
    /// </summary>
    public void ReactToEndGame(double[] ratios);

    /// <summary>
    /// Called when an error occurs. Implementing classes must define how to react to errors.
    /// </summary>
    public void ReactToError(string win, string error);
}
