using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUntil.DTO
{
    public class Menu
    {

public class MenuDto
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool Hidden { get; set; }
        public string Redirect { get; set; }
        public string Component { get; set; }
        public bool AlwaysShow { get; set; }
        public MenuMeta Meta { get; set; }
        public List<MenuDto> Children { get; set; }
    }

    public class MenuMeta
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool NoCache { get; set; }
        public string Link { get; set; }
    }
}
}
