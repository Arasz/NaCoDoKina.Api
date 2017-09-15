using System;
using System.Collections.Generic;

namespace NaCoDoKina.Api.DataContracts
{
    /// <summary>
    /// Movie show times for cinema 
    /// </summary>
    public class MovieShowtime
    {
        public string CinemaName { get; set; }

        public List<DateTime> ShowTimes { get; set; }
    }
}