using System.Data;
using Terminal.Gui;

namespace Arc.Cmdec
{
    internal class CommandListWindow : Window
    {
        public CommandListWindow()
        {
            Title = "Command list";

            // Add table
            var table = new DataTable("Table");
            table.Columns.Add("Position");
            table.Columns.Add("Commands");
            table.Columns.Add("Description");

            // Add rows
            foreach (var command in Memory.Package!.Commands)
            {
                table.Rows.Add(command.Location, command.RawData, command.Description);
            }

            // Create table
            Table = new TableView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(1)
            };

            Table.Data = table;
            Table.Update();

            // Add table to window
            Add(Table);
        }

        public TableView Table { get; set; }
    }
}
