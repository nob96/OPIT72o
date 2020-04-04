using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace OPIT72o.ViewModel.Session
{
    class Ubersicht : Ressources.Binder
    {
        private Model.Observable.Session _list;
        private string _dbStatus;


        public Model.Observable.Session List { get { return this._list; } set { this._list = value; this.OnPropertyChanged("List"); } }
        public Ressources.RC CmdBuchen { get; set; }
        public string DBStatus { get { return this._dbStatus; } set { this._dbStatus = value; this.OnPropertyChanged("DBStatus"); } }

        public Ubersicht() : base()
        {
            this.List = new Model.Observable.Session();
            this.CmdBuchen = new Ressources.RC(Buchen, param => true);
        }

        public void Buchen(object obj)
        {
            Model.Session model = (Model.Session)obj;
            MessageBoxResult confirmResult = System.Windows.MessageBox.Show($"Session {model.Session7} buchen?", "Buchen Bestätigen", System.Windows.MessageBoxButton.YesNo);

            if (confirmResult == MessageBoxResult.Yes)
            {
                if (model.Buchen())
                {
                    this.DBStatus = "Buchen erfolgreich.";
                    this.List.Remove(model);
                }
                else
                {
                    this.DBStatus = "Speichern fehlgeschlagen: " + model.DB.Error;
                }
            }
        }
    }
}
