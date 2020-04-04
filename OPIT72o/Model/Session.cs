using System;
using System.Collections.Generic;
using System.Text;

namespace OPIT72o.Model
{
    class Session : Ressources.Binder
    {
        private int _session7;
        private int _bankroll8;
        private decimal _buyIn;
        private decimal _cashOut;
        private string _location;
        private string _start = DateTime.Now.ToString();
        private string _ende;
        private decimal _bigBlind;
        private string _dauer;
        private decimal _snowieScore;
        private bool _gebucht;

        public Ressources.Database DB { get; set; }
        public int Session7 { get { return this._session7; } set { this._session7 = value; this.OnPropertyChanged("Session7"); } }
        public int Bankroll8 { get { return this._bankroll8; } set { this._bankroll8 = value; this.OnPropertyChanged("Bankroll8"); } }
        public decimal BuyIn { get { return this._buyIn; } set { this._buyIn = value; this.OnPropertyChanged("BuyIn"); } }
        public decimal CashOut { get { return this._cashOut; } set { this._cashOut = value; this.OnPropertyChanged("CashOut"); } }
        public string Location { get { return this._location; } set { this._location = value; this.OnPropertyChanged("Location"); } }
        public string Start { get { return this._start; } set { this._start = value; this.OnPropertyChanged("Start"); } }
        public string Ende { get { return this._ende; } set { this._ende = value; this.OnPropertyChanged("Dauer"); } }
        public decimal BigBlind { get { return this._bigBlind; } set { this._bigBlind = value; this.OnPropertyChanged("BigBlind"); } }
        public decimal SnowieScore { get { return this._snowieScore; } set { this._snowieScore = value; this.OnPropertyChanged("SnowieScore"); } }
        public bool Gebucht { get { return this._gebucht; } set { this._gebucht = value; } }
        public string Dauer
        { 
            get 
            {
                DateTime s = DateTime.Parse(this.Start);
                DateTime e = DateTime.Parse(this.Ende);
                TimeSpan d = e - s;
                return String.Format("{0:0.00}", d.TotalMinutes);
            }

            set
            {
                this._dauer = value;
                this.OnPropertyChanged("Dauer");
            }
        }

        public Session() : base()
        {
            this.DB = new Ressources.Database();
        }

        public bool Save()
        {
            string query = $"INSERT INTO Session VALUES" +
                           $"(" +
                                $"0," +
                                $"{this.Bankroll8}," +
                                $"{this.BuyIn}," +
                                $"NULL," +
                                $"'{this.Location}'," +
                                $"'{DateTime.Parse(this.Start).ToString("yyyy-MM-dd HH:mm:ss")}'," +
                                $"NULL," +
                                $"{this.BigBlind}," +
                                $"FALSE," +
                                $"NULL" +
                           $")";

            return this.DB.SaveOrUpdate(query);
        }

        public bool Buchen()
        {
            string query = $"UPDATE Session " +
                            $"SET CashOut={this.CashOut}, " +
                            $"Ende='{DateTime.Parse(this.Ende).ToString("yyyy-MM-dd HH:mm:ss")}', " +
                            $"SnowieScore={this.SnowieScore}, " +
                            $"Buchen = 1 " +
                            $"WHERE Session7={this.Session7}";

            return this.DB.SaveOrUpdate(query);
        }
    }
}
