using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using Godot;

namespace Scripts.Models {
    /// <summary>
    /// The ProposalGenerator class is responsible for generating random proposals 
    /// by fetching data from a database.
    /// </summary>
    public class ProposalGenerator
    {
        private SqliteConnection cx;
        private Random _rnd;
        private List<int> _valuesToExclude;
        private const int TOTAL_PROPOSALS = 17;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProposalGenerator"/> class.
        /// This constructor sets up the random number generator and initializes 
        /// the list to keep track of excluded values. It also establishes a connection
        /// to the database.
        /// </summary>
        public ProposalGenerator() {
            _rnd = new Random();
            _valuesToExclude = new List<int>();
            string dbPath = ProjectSettings.GlobalizePath("res://resources/DataBase/formationB.db"); // Gets the path to the Database
            string connectionString = $"Data Source={dbPath}";
            GD.Print($"Database path: {dbPath}");
            try 
            {
                cx = new SqliteConnection(connectionString); // Create a new connection
                cx.Open(); // Open the connection
            } 
            catch (Exception ex) 
            {
                GD.Print($"Error opening database connection: {ex.Message}"); // Log any connection errors
            }
        }

        /// <summary>
        /// Generates a random <see cref="Proposal"/> by fetching data from the database.
        /// It retrieves data for a randomly selected proposal, ensuring that already 
        /// selected proposals are excluded.
        /// </summary>
        /// <returns>A <see cref="Proposal"/> object containing the randomly selected proposal data.</returns>
        public Proposal GenerateProposal() 
        {
            int id;
            do {
                id = _rnd.Next(1, TOTAL_PROPOSALS + 1);  // Generate a random ID between 1 and 17
            } while (_valuesToExclude.Contains(id));  // Ensure the ID is not already selected
            _valuesToExclude.Add(id);

            Proposal proposal = null; // Create a new Proposal object
            string query = $"SELECT name, field, candidates, jobs FROM formations WHERE id = {id}"; // SQL query to retrieve proposal data

            try 
            {
                using (var cmd = new SqliteCommand(query, cx)) // Create a command to execute the query
                {
                    if (cx.State != System.Data.ConnectionState.Open) 
                    {
                        cx.Open(); // Ensure the connection is open
                    }

                    SqliteDataReader rd = cmd.ExecuteReader(); // Execute the query and get a data reader
                    if (rd.Read()) // Check if any data was returned
                    {
                        // Initialize the Proposal object with the retrieved data
                        proposal = new Proposal(
                            id,                   // id
                            rd.GetString(0),      // name
                            rd.GetString(1),      // field
                            rd.GetInt32(2),       // candidates
                            rd.GetInt32(3)        // jobs
                        );
                    } 
                    else 
                    {
                        GD.Print("No proposal found."); // Log if no data was found
                    }
                }
            } 
            catch (SqliteException ex) 
            {
                GD.Print($"Database error: {ex.Message}"); // Log database errors
            } 
            catch (Exception ex) 
            {
                GD.Print($"An error occurred: {ex.Message}"); // Log any other errors
            }

            return proposal; // Return the generated Proposal
        }


        /// <summary>
        /// Closes the connection to the database.
        /// <summary>
        public int GetProposalCount() {
            return _valuesToExclude.Count;
        }

        /// <summary>
        /// Returns the number of proposals left to generate.
        /// </summary>
        public int GetProposalLeft() {
            return TOTAL_PROPOSALS - _valuesToExclude.Count;
        }

        public int GetTotalProposal() {
            return TOTAL_PROPOSALS;
        }

        /// <summary>
        /// Closes the connection to the database.
        /// <summary>
        public void RemoveProposal(Proposal proposal) {
            _valuesToExclude.Remove(proposal.GetId());

        }
    }

}
