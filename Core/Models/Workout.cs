using Microsoft.AspNetCore.Mvc;

namespace Core.Models
{
    public class Workout
    {
        [FromQuery(Name = "NumberOfSets")]
        public int NumberOfSets { get; set; } = 8;
        [FromQuery(Name = "SetDuration")]
        public int SetDuration { get; set; } = 60;
        [FromQuery(Name = "RestDuration")]
        public int RestDuration { get; set; } = 0;

        public enum SetType
        {
            CoreSide = 1,
            CoreCenter = 2
        }
    }
}
