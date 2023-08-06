using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinalMovil2.Views
{
    public class NavAdminFlyOutMenuItem
    {
        public NavAdminFlyOutMenuItem() { 
            TargetType = typeof(NavAdminFlyOutMenuItem);
        }

        public int Id { get; set; } 
        public string Title { get; set; }
        public Type TargetType { get; set; }
        public string IconSource { get; set; }
    }
}
