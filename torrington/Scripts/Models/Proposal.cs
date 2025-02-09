using System;
using Scripts.Models;

public class Proposal 
{
    private int _id;
    private string _name;
    private Field _field;
    private int _candidates;
    private int _jobs;
    private int _nbStudent;
    private int _pcGratuated; // Percentage of graduated students

    private const int MAX_STUDENT = 100;
    private const int MAX_GRATUATED = 100;

    /// <summary>
    /// Initializes a new instance of the <see cref="Proposal"/> class with the specified name, field, candidates, and jobs.
    /// </summary>
    /// <param name="name">The name of the proposal.</param>
    /// <param name="field">The field of study associated with the proposal, represented as a string.</param>
    /// <param name="candidates">The number of candidates for the proposal.</param>
    /// <param name="jobs">The number of jobs available in the proposal.</param>
    public Proposal(int id, string name, string field, int candidates, int jobs) {
        _id = id;
        Random rnd = new Random();
        _name = name;
        _field = (Field)Enum.Parse(typeof(Field), field, true);        // Conversion of string to enum
        _candidates = candidates;
        _jobs = jobs;
        _nbStudent = 0;
        _pcGratuated = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Proposal"/> class with default values.
    /// </summary>
    public Proposal() {
        _name = null;
        _field = 0;        // Default enum value
        _candidates = 0;
        _jobs = 0;
    }

    /// <summary>
    /// Returns a string representation of the proposal, including its name, field, number of candidates, and number of jobs.
    /// </summary>
    /// <returns>A string containing the proposal's details.</returns>
    public override string ToString()
    {
        return $"{_name} \n" +
               $"Field: {_field}\n" +
               $"Number of candidates: {_candidates}\n" +
               $"Number of jobs: {_jobs}\n";
    }

    /// <summary>
    /// Gets the ID of the proposal.
    /// </summary>
    public int GetId() {
        return _id;
    }

    /// <summary>
    /// Gets the name of the proposal.
    /// </summary>
    /// <returns>The name of the proposal.</returns>
    public string GetName() {
        return _name;
    }

    /// <summary>
    /// Gets the number of candidates for the proposal.
    /// </summary>
    /// <returns>The number of candidates.</returns>
    public int GetCandidates() {
        return _candidates;
    }

    /// <summary>
    /// Gets the number of jobs available for the proposal.
    /// </summary>
    /// <returns>The number of jobs.</returns>
    public int GetJobs() {
        return _jobs;
    }

    /// <summary>
    /// Gets the field associated with the proposal.
    /// </summary>
    /// <returns>The field of study as an enum value.</returns>
    public Field GetField() {
        return _field;
    }

    /// <summary>
    /// Sets the number of students in the proposal.
    /// </summary>
    /// <param name="nbStudent">The number of students to set.</param>
    public void SetStudent(int nbStudent) {
        _nbStudent = nbStudent;
    }

    /// <summary>
    /// Gets the number of students in the proposal.
    /// </summary>
    /// <returns>The number of students.</returns>
    public int GetStudent() {
        return _nbStudent;
    }

    /// <summary>
    /// Gets the maximum number of students that can be accepted for the proposal.
    /// </summary>
    /// <returns>The maximum number of students.</returns>
    public int GetStudentMax() {
        return MAX_STUDENT;
    }

    /// <summary>
    /// Sets the number of graduates for the proposal.
    /// </summary>
    /// <param name="nbGratuated">The number of graduates to set.</param>
    public void SetGratuated(int pcGratuated) {
        _pcGratuated = pcGratuated;
    }

    /// <summary>
    /// Gets the number of graduates for the proposal.
    /// </summary>
    /// <returns>The number of graduates.</returns>
    public int GetGratuated() {
        return _pcGratuated;
    }

    /// <summary>
    /// Gets the maximum number of graduates that can be achieved for the proposal.
    /// </summary>
    /// <returns>The maximum number of graduates.</returns>
    public int GetGratuatedMax() {
        return MAX_GRATUATED;
    }
}
