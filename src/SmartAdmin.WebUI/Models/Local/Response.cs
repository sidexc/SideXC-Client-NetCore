using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideXC.WebUI.Models.Local
{
    public class Response
    {
        List<string> _lista = new List<string>();
        public List<string> Errors { get { return _lista; } }
        public bool AnyError { get { return _lista.Any(); } }
        public void AddError(string error) { _lista.Add(error); }
        public string HtmlError { get { return string.Join("<br>", _lista); } }
        public string Information { get; set; }
    }
}
