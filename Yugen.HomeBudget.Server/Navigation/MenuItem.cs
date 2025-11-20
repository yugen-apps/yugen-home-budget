using MudBlazor;
using System.Collections.Generic;

namespace Yugen.HomeBudget.Server.Navigation
{
    public class MenuItem
    {
        public string Icon { get; set; } = Icons.Material.Filled.Dashboard;

        public bool IsParent => MenuSubItems.Count > 0;

        public List<MenuItem> MenuSubItems { get; set; } = [];

        public string Path { get; set; } = string.Empty;

        public string Policy { get; set; } = string.Empty;

        public string[] Roles { get; set; } = [];

        public string Subtitle { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
    }
}