using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OPIT72o.ViewModel.Bankroll
{
    class Neu : Ressources.Binder
    {
        private string _dbStatus;

        public Model.Bankroll Model { get; set; }
        public Ressources.RC CmdSave { get; set; }
        public string DBStatus { get { return this._dbStatus; } set { this._dbStatus = value; this.OnPropertyChanged("DBStatus"); } }

        public Neu() : base()
        {
            this.Model = new Model.Bankroll();
            this.CmdSave = new Ressources.RC(Save, param => true);
        }

        public void Save(object obj)
        {
            this.DBStatus = (this.Model.Save()) ? "Speichern ok." : "Speichern fehlgeschlagen: " + this.Model.DB.Error;
        }
    }
}
