using Microsoft.WindowsAPICodePack.Dialogs;

namespace OPIT72o.ViewModel.Tracker
{
    class Ubersicht : Ressources.Binder
    {
        private string _watcherStatus;

        public Ressources.RC CmdBrowse { get; set; }
        public Ressources.RC CmdWatch { get; set; }
        public Model.Tracker Model { get; set; }
        public string WatcherStatus { get { return this._watcherStatus; } set { this._watcherStatus = value; this.OnPropertyChanged("WatcherStatus"); } }

        public Ubersicht()
        {
            this.WatcherStatus = "Inaktiv";
            this.Model = new Model.Tracker();
            this.CmdBrowse = new Ressources.RC(Browse, param => true);
            this.CmdWatch = new Ressources.RC(Watch, param => true);
        }

        public void Browse(object obj)
        {
            CommonOpenFileDialog folderDialog = this.GetFolderDialog();

            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.Model.PfadLokal = folderDialog.FileName;
            }
        }

        public void Watch(object obj)
        {
            this.WatcherStatus = "Aktiv";
            this.Model.Watch();
        }

        private CommonOpenFileDialog GetFolderDialog()
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.IsFolderPicker = true;
            folderDialog.InitialDirectory = @"C:\";

            return folderDialog;
        }
    }
}
