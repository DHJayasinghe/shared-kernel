using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SharedKernel.Helpers
{
    public class PaginationResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecords { get; set; }

        [JsonIgnore]
        public byte[] Content { get; set; }

    }
}
