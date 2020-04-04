using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace OPIT72o.ViewModel
{
    class Main
    {
        public Ressources.RC CmdShowBNeuV { get; set; }
        public Ressources.RC CmdShowBUbersichtV { get; set; }
        public Ressources.RC CmdShowSNeuV { get; set; }
        public Ressources.RC CmdShowSUbersichtV { get; set; }
        public Ressources.RC CmdShowLSUbersichtV { get; set; }
        public Ressources.RC CmdShowTBenitoV { get; set; }
        public ContentPresenter Presenter { get; set; }


        public Main()
        {
            this.Presenter = new ContentPresenter();

            this.CmdShowBNeuV = new Ressources.RC(ShowBNeuV, param => true);
            this.CmdShowBUbersichtV = new Ressources.RC(ShowBUbersichtV, param => true);
            this.CmdShowSNeuV = new Ressources.RC(ShowSNeuV, param => true);
            this.CmdShowSUbersichtV = new Ressources.RC(ShowSUbersichtV, param => true);
            this.CmdShowLSUbersichtV = new Ressources.RC(ShowLSUbersichtV, param => true);
            this.CmdShowTBenitoV = new Ressources.RC(ShowTBenitoV, param => true);
        }

        public void ShowBNeuV(object obj)
        {
            this.Presenter.Content = new UserControl.Bankroll.Neu();
        }

        public void ShowBUbersichtV(object obj)
        {
            this.Presenter.Content = new UserControl.Bankroll.Ubersicht();
        }

        public void ShowSNeuV(object obj)
        {
            this.Presenter.Content = new UserControl.Session.Neu();
        }

        public void ShowSUbersichtV(object obj)
        {
            this.Presenter.Content = new UserControl.Session.Ubersicht();
        }

        public void ShowLSUbersichtV(object obj)
        {
            this.Presenter.Content = new UserControl.Tracker.Ubersicht();
        }

        public void ShowTBenitoV(object obj)
        {

        }
    }
}
