using System;
using System.Collections.Generic;

namespace NaCoDoKina.Api.Models
{
    /// <summary>
    /// Movie show times for cinema 
    /// </summary>
    public class MovieShowtime
    {
        /// <summary>
        /// Movie show language and presentation type 
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Type of the show, for example 2D, 3D, etc. 
        /// </summary>
        public string ShowType { get; set; }

        /// <summary>
        /// Cinema name 
        /// </summary>
        public string CinemaName { get; set; }

        /// <summary>
        /// Show times 
        /// </summary>
        public List<DateTime> ShowTimes { get; set; }

        public override string ToString()
        {
            return $"{nameof(Language)}: {Language}, {nameof(ShowType)}: {ShowType}, {nameof(CinemaName)}: {CinemaName}";
        }
    }
}