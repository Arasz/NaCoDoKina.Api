namespace NaCoDoKina.Api.Infrastructure.Settings
{
    /// <summary>
    /// Connects C# class property name to HTML element on web page with CSS selector 
    /// </summary>
    public class PropertySelector
    {
        /// <summary>
        /// C# class property name - data target 
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// CSS selector for element with property value - data source 
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        /// Html element attribute 
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// Should map value from attribute provided in attribute property 
        /// </summary>
        public bool FromAttribute { get; set; }
    }
}