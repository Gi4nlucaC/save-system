namespace SaveSystem
{
    /// <summary>
    /// Interface for objects that contribute metadata to save files
    /// </summary>
    public interface IHeaderSavable
    {
        /// <summary>
        /// Get metadata that should be included in the save file header
        /// </summary>
        /// <returns>Metadata to include in the save header</returns>
        MetaData GetMetaDataPart();
    }
}