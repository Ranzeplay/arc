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
            table.Columns.Add("Location");
            table.Columns.Add("Commands");
            table.Columns.Add("Description");

            // Add rows
            foreach (var command in Memory.Package!.Commands)
            {
                string rawData = "";
                foreach (var item in command.RawData)
                {
                    var str = Convert.ToString(item, 16).PadLeft(2, '0');

                    rawData += str + ' ';
                }

                table.Rows.Add($"0x{Convert.ToString(command.Location, 16).PadLeft(8, '0')}", rawData, command.Description);
            }

            // Create table
            Table = new TableView
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(-1),
                Table = table,
                ColumnOffset = 0,
            };

            
            Table.Style.GetOrCreateColumnStyle(table.Columns["Location"])
                .RepresentationGetter = (v) => v.ToString();
            Table.Style.GetOrCreateColumnStyle(table.Columns["Commands"])
                .RepresentationGetter = (v) => v.ToString();
            Table.Style.GetOrCreateColumnStyle(table.Columns["Description"])
                .RepresentationGetter = (v) => v.ToString();


            // Add table to window
            Add(Table);
        }

        public TableView Table { get; set; }
    }
}
