﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Model.DTO
{
    public class AddImageDTO
    {
        public IFormFile Image { get; set; }
    }
}
