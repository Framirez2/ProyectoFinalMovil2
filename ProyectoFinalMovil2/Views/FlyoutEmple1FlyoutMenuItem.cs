using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalMovil2.Views
{
    public class FlyoutEmple1FlyoutMenuItem
    {
        public FlyoutEmple1FlyoutMenuItem()
        {
            TargetType = typeof(FlyoutEmple1FlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }

        public string IconSource { get; set; }
    }
}