using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPIT72o.ViewModel.Session
{
    class Neu : Ressources.Binder
    {
        private string _dbStatus;

        public int Bankroll8 { get; set; }
        public Model.Session Model { get; set; }
        public Ressources.RC CmdSave { get; set; }
        public string DBStatus { get { return this._dbStatus; } set { this._dbStatus = value; this.OnPropertyChanged("DBStatus"); } }


        public Neu() : base()
        {
            this.CmdSave = new Ressources.RC(Save, param => true);
            this.Model = new Model.Session();
            Model.Observable.Bankroll bList = new Model.Observable.Bankroll();

            try
            {
                this.Bankroll8 = bList.First(model => model.Aktiv == true).Bankroll7;
                this.Model.Bankroll8 = this.Bankroll8;
            }
            catch (System.InvalidOperationException e)
            {
                this.Bankroll8 = -1;
                this.Model.Bankroll8 = this.Bankroll8;
            }

        }

        public void Save(object obj)
        {
            this.DBStatus = (this.Model.Save() == true) ? "Speichern erfolgreich." : "Speichern fehlgeschlagen: " + this.Model.DB.Error;
        }
    }
}
