using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Rtl.TVMazeService.Domain.Models.Dtos
{
    public class CastMemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public DateTime? Birthdate { get; set; }

        public string Birthday
        {
            get => Birthdate?.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            set { }
        }
    }
}
