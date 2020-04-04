using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace OPIT72o.ViewModel.Bankroll
{
    class Ubersicht : Ressources.Binder
    {
        private Model.Observable.Bankroll _list;
        private string _dbstatus;

        public Model.Bankroll Model { get; set; }
        public Model.Observable.Bankroll List { get { return this._list; } set { this._list = value; this.OnPropertyChanged("List"); } }
        public Ressources.RC CmdSave { get; set; }
        public Ressources.RC CmdDelete { get; set; }
        public string DBStatus { get { return this._dbstatus; } set { this._dbstatus = value; this.OnPropertyChanged("DBStatus"); } }

        public Ubersicht() : base()
        {
            this.Model = new Model.Bankroll();
            this.List = new Model.Observable.Bankroll();
            this.CmdDelete = new Ressources.RC(Delete, param => true);
            this.CmdSave = new Ressources.RC(Update, param => true);
        }

        public void Update(object obj)
        {
            OPIT72o.Model.Bankroll model = (OPIT72o.Model.Bankroll)obj;
            this.DBStatus = (model.Update()) ? "Update ok." : "Update fehlgeschlagen: " + model.DB.Error;
        }

        public void Delete(object obj)
        {
            OPIT72o.Model.Bankroll model = (OPIT72o.Model.Bankroll)obj;
            MessageBoxResult confirmResult = System.Windows.MessageBox.Show($"Bankroll {model.Bezeichnung} löschen?", "Löschen Bestätigen", System.Windows.MessageBoxButton.YesNo);

            if (confirmResult == MessageBoxResult.Yes)
            {
                this.List.Remove(model);
                this.DBStatus = (model.Delete()) ? "Löschen ok." : "Löschen fehlgeschlagen: " + model.DB.Error;
            }

        }
    }
}
