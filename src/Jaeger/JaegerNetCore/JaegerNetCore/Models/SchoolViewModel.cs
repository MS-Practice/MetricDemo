using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JaegerNetCore.Models
{
    public class SchoolViewModel
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int AreaId { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public List<ClassViewModel> Classes { get; set; } = new List<ClassViewModel>();
    }
}
