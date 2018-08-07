using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Team
    {
        public int TeamID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Zoom { get; set; }

        public double Lattitude { get; set; }

        public double Longtidute { get; set; }

        public Team() { }

        public Team(int teamId, string name, string description, double zoom, double lattitude, double longtidute)
        {
            this.TeamID = teamId;
            this.Name = name;
            this.Description = description;
            this.Zoom = zoom;
            this.Lattitude = lattitude;
            this.Longtidute = longtidute;
        }
    }
}
