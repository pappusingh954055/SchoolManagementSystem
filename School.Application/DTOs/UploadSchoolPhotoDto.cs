using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs
{
    public class UploadSchoolPhotoDto
    {
        public IFormFile File { get; set; } = default!;
    }
}
