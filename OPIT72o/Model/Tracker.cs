using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OPIT72o.Model
{
    class Tracker : Ressources.Binder
    {
        private string _pfadLokal;
        private string _nickname;
        private int _session8;
        private Model.Observable.Tracker _list;
        private string _dbStatus;

        public string PfadLokal { get { return this._pfadLokal; } set { this._pfadLokal = value; this.OnPropertyChanged("PfadLokal"); } }
        public int Session8 { get { return this._session8; } set { this._session8 = value; this.OnPropertyChanged("Session8"); } }
        public string Nickname { get { return this._nickname; } set { this._nickname = value; this.OnPropertyChanged("Nickname"); } }
        public Model.Observable.Tracker List { get { return this._list; } set { this._list = value; this.OnPropertyChanged("List"); } }
        public Ressources.Database DB { get; set; }
        public string DBStatus { get { return this._dbStatus; } set { this._dbStatus = value; this.OnPropertyChanged("DBStatus"); } }



        public Tracker() : base()
        {
            this.DB = new Ressources.Database();
            Observable.Session sessionList = new Observable.Session();
            this.List = new Observable.Tracker();
            

            try
            {
                this.Session8 = sessionList.First(session => session.Gebucht == false).Session7;

            }
            catch (InvalidOperationException e)
            {
                this.Session8 = -1;
            }
        }

        public void Watch()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(Work);
            worker.RunWorkerAsync();
        }

        public void Work(object sender, DoWorkEventArgs e)
        {
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = this.PfadLokal;
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.FileName;
                watcher.Filter = "*.txt";
                watcher.Changed += (s, ev) => SaveNewHand(ev.FullPath, ev, this.PfadLokal, this.Nickname);
                watcher.Created += (s, ev) => SaveNewHand(ev.FullPath, ev, this.PfadLokal, this.Nickname);
                watcher.Deleted += (s, ev) => SaveNewHand(ev.FullPath, ev, this.PfadLokal, this.Nickname);
                watcher.Renamed += (s, ev) => SaveNewHand(ev.FullPath, ev, this.PfadLokal, this.Nickname);
                watcher.EnableRaisingEvents = true;
                while (true) ;
            }
        }

        private void SaveNewHand(object source, FileSystemEventArgs e, string pfadLokal, string nickname)
        {
            Parser parser = new Parser(pfadLokal, nickname);
            string query = $"INSERT INTO pokerking VALUES " +
                            $"(0, " +
                            $"{this.Session8}, " +
                            $"'{parser.Hand.PokerKingID}', " +
                            $"'{parser.Hand.Holecard1.FullName}', " +
                            $"'{parser.Hand.Holecard2.FullName}', " +
                            $"'{parser.Hand.Flop1.FullName}', " +
                            $"'{parser.Hand.Flop2.FullName}', " +
                            $"'{parser.Hand.Flop3.FullName}', " +
                            $"'{parser.Hand.Turn.FullName}', " +
                            $"'{parser.Hand.River.FullName}', " +
                            $"'{parser.Hand.Pot}', " +
                            $"'{parser.Hand.Rake}', " +
                            $"'{parser.Hand.Ergebnis}', " +
                            $"'{parser.Hand.ErgebnisDetail}', " +
                            $"NOW(), " +
                            $"0, " +
                            $"0, " +
                            $"'{Regex.Escape(pfadLokal)}', " +
                            $"''" +
                            $")";

            this.DB.SaveOrUpdate(query);
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => {this.List.Add(new Hand() { Remember = false, Notiz = null, PokerKingID = parser.Hand.PokerKingID }); }));
        }
    }
}
