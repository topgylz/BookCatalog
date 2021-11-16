using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.RequestFeatures
{
    public class AuthorParameters : RequestParameters
    {
        public AuthorParameters()
        {
            OrderBy = "surname";
        }
        public string SearchTerm { get; set; }
    }
}
