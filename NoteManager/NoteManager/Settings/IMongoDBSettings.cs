namespace NoteManager.Settings
{
    /// <summary>
    /// Interface for MongoDB settings required for connecting to a MongoDB database.
    /// </summary>
    public interface IMongoDBSettings
    {
        /// <summary>
        /// Gets or sets the name of the notes collection.
        /// </summary>
        string NoteCollectionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the categories collection.
        /// </summary>
        string CategoryCollectionName {  get; set; }

        /// <summary>
        /// Gets or sets the connection string for the MongoDB database.
        /// </summary
        string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the MongoDB database.
        /// </summary>
        string DatabaseName { get; set; }
    }
}
