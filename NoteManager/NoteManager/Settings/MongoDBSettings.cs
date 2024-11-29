namespace NoteManager.Settings
{
    public class MongoDBSettings : IMongoDBSettings
    {
        /// <summary>
        /// Gets or sets the name of the notes collection.
        /// </summary>
        public string NoteCollectionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the categories colection
        /// </summary>
        public string CategoryCollectionName {  get; set; }

        /// <summary>
        /// Gets or sets the connection string for the MongoDB database.
        /// </summary>
        public string ConnectionString { get; set ; }

        /// <summary>
        /// Gets or sets the name of the MongoDB database.
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
