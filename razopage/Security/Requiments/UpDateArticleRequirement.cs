using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace App.Security
{
    public class UpDateArticleRequirement:IAuthorizationRequirement
    {
        private readonly int _year;
        private readonly int _month;
        private readonly int _day;

        public UpDateArticleRequirement(int year = 2023, int month =3 , int day = 10)
        {
            this._year = year;
            this._month = month;
            this._day = day;
        }

        public DateTime? GetNgayCheck()
        {
            var rs = new DateTime(_year,_month,_day);
            return rs;
        }
    }
}