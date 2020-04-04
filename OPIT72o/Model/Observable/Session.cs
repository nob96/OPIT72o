using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace OPIT72o.Model.Observable
{
    class Session : ObservableCollection<Model.Session>
    {
        public Session()
        {
            Ressources.Database db = new Ressources.Database();
            string query = $"SELECT s.* FROM Session s WHERE s.Buchen = FALSE";
            db.Select(query);

            foreach (dynamic s in db.Data)
            {
                Add(new Model.Session()
                {
                    Session7 = int.Parse(s.Session7),
                    Bankroll8 = int.Parse(s.Bankroll8),
                    BuyIn = decimal.Parse(s.BuyIn),
                    CashOut = (s.CashOut == null || s.CashOut == "") ? 0 : decimal.Parse(s.CashOut),
                    Location = s.Location,
                    Start = s.Start,
                    Ende = (s.Ende == null || s.Ende == "") ? "0001-01-01" : s.Ende,
                    BigBlind = decimal.Parse(s.BigBlind),
                    Gebucht = bool.Parse(s.Buchen)
                });
            }
        }
    }
}
