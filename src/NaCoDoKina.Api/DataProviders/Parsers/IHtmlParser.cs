namespace NaCoDoKina.Api.DataProviders.Parsers
{
    /// <summary>
    /// Parses html content to given entity class 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IHtmlParser<in TEntity>
    {
        /// <summary>
        /// Parses content and fills up provided object 
        /// </summary>
        /// <param name="content"> Parsed content </param>
        /// <param name="parsed"> Object to fill </param>
        void Parse(string content, TEntity parsed);
    }
}