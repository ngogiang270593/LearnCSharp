using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace App.Security
{
    public class GenZrequirement:IAuthorizationRequirement
    {
        public int MinYear { get; }

        public int MaxYear { get; }

        public GenZrequirement(int _minYear = 1997, int _maxYear = 2012)
        {
            MinYear = _minYear;
            MaxYear = _maxYear;
        }
    }
}