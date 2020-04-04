using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace OPIT72o.Model.Observable
{
    class Bankroll : ObservableCollection<Model.Bankroll>
    {
        public Bankroll() : base()
        {
            Ressources.Database db = new Ressources.Database();
            string query = $"SELECT * FROM Bankroll";
            db.Select(query);

            foreach (dynamic b in db.Data)
            {
                decimal netto = decimal.Parse(b.Betrag) - decimal.Parse(b.Startbetrag);

                Add(new Model.Bankroll()
                {
                    Bankroll7 = int.Parse(b.Bankroll7),
                    Bezeichnung = b.Bezeichnung,
                    Startbetrag = decimal.Parse(b.Startbetrag),
                    Betrag = (b.Betrag == null || b.Betrag == "") ? 0 : decimal.Parse(b.Betrag),
                    Aktiv = bool.Parse(b.Aktiv),
                    Trend = decimal.Round((netto / decimal.Parse(b.Startbetrag)) * 100, 2, MidpointRounding.AwayFromZero),
                    Winrate = (b.Winrate == null || b.Winrate == "") ? 0 : decimal.Parse(b.Winrate),
                    Netto = netto,
                    Datum = DateTime.Parse(b.Datum)
                });
            }

        }
    }
}
