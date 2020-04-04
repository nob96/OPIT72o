using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using OPIT72o.Model.DataObject;

namespace OPIT72o.Model
{
    class Hand : Ressources.Binder
    {
        private string _dbStatus;

        public Card Holecard1 { get; set; }
        public Card Holecard2 { get; set; }
        public Card Flop1 { get; set; }
        public Card Flop2 { get; set; }
        public Card Flop3 { get; set; }
        public Card Turn { get; set; }
        public Card River { get; set; }
        public decimal Pot { get; set; }
        public decimal Rake { get; set; }
        public string ErgebnisDetail { get; set; }
        public string Ergebnis { get; set; }
        public string PokerKingID { get; set; }
        public bool Remember { get; set; }
        public string Notiz { get; set; }
        public string Nickname { get; set; }
        public Ressources.RC CmdUpdate { get; set; }
        public Ressources.Database DB { get; set; }
        public string DBStatus { get { return this._dbStatus; } set { this._dbStatus = value; this.OnPropertyChanged("DBStatus"); } }

        public Hand()
        {
            this.DB = new Ressources.Database();
            this.CmdUpdate = new Ressources.RC(UpdateHand, param => true);
        }

        public void UpdateHand(object obj)
        {
            Hand hand = (Hand)obj;
            string query = $"UPDATE pokerking SET Remember={hand.Remember}, Notiz='{hand.Notiz}' WHERE HandID='{hand.PokerKingID}'";
            this.DBStatus = (this.DB.SaveOrUpdate(query)) ? "Update ok." : "Update fehlgeschlagen: " + this.DB.Error;
        }
    }
}
