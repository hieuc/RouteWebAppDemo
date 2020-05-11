using RoutesWebAppDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoutesWebAppDemo.Controllers
{
    // DataPack acts like a struct to hold multiple data type.
    public class DataPack
    {
        public String url { get; set; } = "";
        public String json { get; set; } = "";
        public Route route { get; set; }
    }
}