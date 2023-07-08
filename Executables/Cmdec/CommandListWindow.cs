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

                table.Rows.Add($"0x{Convert.ToString(command.Location, 16).PadLeft(Memory.Package.Metadata.AddressAlignment, '0')}", rawData, command.Description);
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

            var tips = new Label
            {
                Text = "Ctrl+Q : Quit application / M : View package metadata",
                X = 0,
                Y = Pos.Bottom(this) - 3
            };
            Add(tips);
        }

        public TableView Table { get; set; }

        public override bool OnKeyDown(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.M || keyEvent.Key == Key.m)
            {
                Dialog dialog = new()
                {
                    Title = "Package metadata",
                    Height = Dim.Percent(70),
                    Width = Dim.Percent(70),
                    VerticalTextAlignment = VerticalTextAlignment.Justified
                };

                dialog.Add(new Label(1, 1, $"Package type        :   {Memory.Package!.Metadata.PackageType}"));
                dialog.Add(new Label(1, 2, $"Data alignment      :   {Memory.Package!.Metadata.DataAlignment}"));
                dialog.Add(new Label(1, 3, $"Data slot alignment :   {Memory.Package!.Metadata.DataSlotAlignment}"));
                dialog.Add(new Label(1, 4, $"Address alignment   :   {Memory.Package!.Metadata.AddressAlignment}"));
                dialog.Add(new Label(1, 5, $"Entry function id   :   {Memory.Package!.Metadata.EntryFunctionId}"));
                dialog.Add(new Label(1, 6, $"Data section size   :   {Memory.Package!.Metadata.DataSectionSize}"));

                var closeButton = new Button("Close", true);
                closeButton.Clicked += () =>
                {
                    Application.RequestStop();
                };
                dialog.AddButton(closeButton);

                Application.Run(dialog);
            }

            return base.OnKeyDown(keyEvent);
        }
    }
}
