using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OPIT72o.Model
{
    class Bankroll : Ressources.Binder
    {
        private int _bankroll7;
        private string _bezeichnung;
        private decimal _startbetrag;
        private decimal _betrag;
        private bool _aktiv;
        private decimal _trend;
        private decimal _winrate;
        private decimal _netto;
        private DateTime _datum;

        public Ressources.Database DB { get; set; }
        public int Bankroll7 { get { return this._bankroll7; } set { this._bankroll7 = value; this.OnPropertyChanged("Bankroll7"); } }
        public string Bezeichnung { get { return this._bezeichnung; } set { this._bezeichnung = value; this.OnPropertyChanged("Bezeichnung"); } }
        public decimal Startbetrag { get { return this._startbetrag; } set { this._startbetrag = value; this.OnPropertyChanged("Startbetrag"); } }
        public decimal Betrag { get { return this._betrag; } set { this._betrag = value; this.OnPropertyChanged("Betrag"); } }
        public bool Aktiv { get { return this._aktiv; } set { this._aktiv = value; this.OnPropertyChanged("Aktiv"); } }
        public decimal Trend { get { return this._trend; } set { this._trend = value; this.OnPropertyChanged("Trend"); } }
        public decimal Winrate { get { return this._winrate; } set { this._winrate = value; this.OnPropertyChanged("Winrate"); } }
        public decimal Netto { get { return this._netto; } set { this._netto = value; this.OnPropertyChanged("Netto"); } }
        public DateTime Datum { get { return this._datum; } set { this._datum = value; this.OnPropertyChanged("Datum"); } }


        public Bankroll() : base()
        {
            this.DB = new Ressources.Database();
        }

        public bool Save()
        {
            string query = $"INSERT INTO Bankroll VALUES" +
                            $"(" +
                                $"0," +
                                $"'{this.Bezeichnung}'," +
                                $"{this.Betrag}," +
                                $"{this.Betrag},"+
                                $"{this.Aktiv}," +
                                $"0," +
                                $"0," +
                                $"0," +
                                $"NOW()" +
                            $")";

            return this.DB.SaveOrUpdate(query);
        }

        public bool Update()
        {
            string query = $"UPDATE Bankroll " +
                            $"SET Bezeichnung = '{this.Bezeichnung}'," +
                            $"Aktiv = {this.Aktiv} " +
                            $"WHERE Bankroll7 = {this.Bankroll7}";

            return this.DB.SaveOrUpdate(query);
        }

        public bool Delete()
        {
            string query = $"DELETE FROM Bankroll WHERE Bankroll7 = {this.Bankroll7}";

            return this.DB.SaveOrUpdate(query);
        }
    }
}
