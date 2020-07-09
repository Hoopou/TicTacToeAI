using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchAPI.Utils.Menu
{
    public class CustomConsoleMenu
    {
        public event EventHandler<ChoiceSelectionArgs> ChoiceSelection;

        private Dictionary<int, string> choices = new Dictionary<int, string>();
        public char SelectedIcon { get; set; } = '#';

        private static int SelectedIndex = 0;

        public string MenuMessage { get; set; } = "";

        public void AddChoice(int id, string choice)
        {
            choices.Add(id, choice);
        }

        public void DisplayMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(GetMenu());
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case (ConsoleKey.UpArrow):
                        if (SelectedIndex > 0 && choices.Count() > 1)
                            SelectedIndex--;
                        break;
                    case (ConsoleKey.DownArrow):
                        if (SelectedIndex < choices.Count() - 1 && choices.Count() > 1)
                            SelectedIndex++;
                        break;
                    case (ConsoleKey.Enter):
                        ChoiceSelection?.Invoke(this, new ChoiceSelectionArgs()
                        {
                            SelectedValue = choices.ElementAt(SelectedIndex).Value,
                            SelectedID = choices.ElementAt(SelectedIndex).Key
                        });
                        return;
                }
            }

        }

        private string GetMenu()
        {
            StringBuilder menu = new StringBuilder();
            menu.AppendLine(MenuMessage);
            for (int i = 0; i < choices.Count(); i++)
            {
                menu.Append(((SelectedIndex == i) ? " " + SelectedIcon + " " : "  "));
                menu.AppendLine(choices.ElementAt(i).Value);
            }

            return menu.ToString();
        }

        public string GetMenuStringFromId(int id)
        {
            return choices.Where(v =>v.Key == id)?.FirstOrDefault().Value;
        }

        public override string ToString()
        {
            return GetMenu();
        }
    }
}
