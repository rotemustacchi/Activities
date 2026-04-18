using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Profiles.DTO
{
    public class PhotoUploadResult
    {
        public required string PublicId { get; set; }
        public required string Url { get; set; }
    }
}
