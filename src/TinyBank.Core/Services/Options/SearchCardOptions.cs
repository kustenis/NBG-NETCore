using System;
using System.Collections.Generic;

namespace TinyBank.Core.Services.Options
{
    public class SearchCardOptions
    {
        public Guid? CardId { get; set; }
        public string CardNumber { get; set; }

        public int? MaxResults { get; set; }
        public bool? TrackResults { get; set; }       
        public int? Skip { get; set; }
    }
}
