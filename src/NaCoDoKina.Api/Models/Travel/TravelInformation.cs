using System;
using System.Collections.Generic;

namespace NaCoDoKina.Api.Models.Travel
{
    /// <summary>
    /// All information about travel 
    /// </summary>
    public partial class TravelInformation : IComparable<TravelInformation>, IComparable
    {
        public TravelPlan TravelPlan { get; }

        public double Distance { get; }

        public TimeSpan Duration { get; }

        public TravelInformation(TravelPlan travelPlan, double distance, TimeSpan duration)
        {
            TravelPlan = travelPlan;
            Distance = distance;
            Duration = duration;
        }
    }

    public partial class TravelInformation
    {
        protected bool Equals(TravelInformation other)
        {
            return Equals(TravelPlan, other.TravelPlan) && Distance.Equals(other.Distance) &&
                   Duration.Equals(other.Duration);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TravelInformation)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (TravelPlan != null ? TravelPlan.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Distance.GetHashCode();
                hashCode = (hashCode * 397) ^ Duration.GetHashCode();
                return hashCode;
            }
        }

        public int CompareTo(TravelInformation other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var distanceComparison = Distance.CompareTo(other.Distance);
            if (distanceComparison != 0) return distanceComparison;
            return Duration.CompareTo(other.Duration);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is TravelInformation))
                throw new ArgumentException($"Object must be of type {nameof(TravelInformation)}");
            return CompareTo((TravelInformation)obj);
        }

        public static bool operator <(TravelInformation left, TravelInformation right)
        {
            return Comparer<TravelInformation>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(TravelInformation left, TravelInformation right)
        {
            return Comparer<TravelInformation>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(TravelInformation left, TravelInformation right)
        {
            return Comparer<TravelInformation>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(TravelInformation left, TravelInformation right)
        {
            return Comparer<TravelInformation>.Default.Compare(left, right) >= 0;
        }
    }
}