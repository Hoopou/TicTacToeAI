using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchAPI.Utils.Menu
{
    public class ChoiceSelectionArgs: EventArgs
    {
        public int SelectedID { get; set; }
        public string SelectedValue { get; set; }
    }
}
