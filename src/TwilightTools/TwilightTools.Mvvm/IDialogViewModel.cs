using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools
{
    public interface IDialogViewModel
    {
        string Title { get; }

        bool? DialogResult { get; set; }
    }
}
