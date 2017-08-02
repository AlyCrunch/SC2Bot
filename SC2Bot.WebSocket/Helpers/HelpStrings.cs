using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC2Bot.WebSocket.Helpers
{
    public static class HelpStrings
    {
        public static class Aligulac
        {
            public const string predictSummary = "Prédit l'avenir d'un match entre deux joueurs";
            public const string predictRemarks = "Utilise le système de prédiction d'Aligulac (http://aligulac.com/inference/).\n" +
                "La syntaxe à utiliser est : \n```!predict Player_A Player_B (Optionnel BO_nb)```\n\n" +
                "Example :\n```!predict ByuN Dark 3```";

            public const string topSummary = "Top des joueurs";
            public const string topRemarks = "Utilise le système de classement d'Aligulac.\nLa syntaxe à utiliser est : \n```!top```";

            public const string opSummary = "Domination de race dans le top player";
            public const string opRemarks = "";

            public const string earnSummary = "Top 40 des plus gros gains remporté par un joueur en tournoi";
            public const string earnRemarks = "";
        }

        public static class Quote
        {
            public const string quoteSummary = "Retourne [une quote SC2](http://www.sc2quoteoftheday.com/)";
            public const string quoteRemarks = "Exemple :\n```\n# Récupère les quotes de l'auteur cité\n!quote Day9\n\n# Retourne tous les auteurs de quotes (en message privé)\n!quote -all```";
        }

        public static class Liquipedia
        {
            public const string transferSummary = "Liste des derniers transferts de joueurs";
            public const string transferRemarks = "";

            public const string liveSummary = "Tous les évenements en live";
            public const string liveRemarks = "";

            public const string eventSummary = "Tous les événements SC2 présent dans le calendrier TeamLiquid";
            public const string eventRemarks = "Obtients les événements de la journée ou de la semaine selon l'attribut passé.Exemple :\n```!event week(Optionnel = day)```";
        }

        public static class General
        {
            public const string cmdSummary = "Liste toutes les commandes";
            public const string cmdRemarks = "";

            public const string helpSummary = "Obtient une aide specifique pour chaque commande";
            public const string helpRemarks = "La syntaxe à utiliser est : \n```!help NomDeCommande```";

            public const string yesnoSummary = "Obtient Oui / Non aléatoirement";
            public const string yesnoRemarks = "";

            public const string iamSummary = "Assigner une race";
            public const string iamRemarks = "Vous pouvez vous assigner des races en utilisant cette commande, " +
                                             "vous pouvez vous assigner au maximum deux races dont `protoss, terran, zerg, random`" +
                                              "\n\nExemple:\n```!iam Zerg Terran(Optionnel)```";
            
            public const string wsplSummary = "Liste les personnes connectées qui sont actuellement entrain de jouer";
            public const string wsplRemarks = "Il est possible d'avoir la liste de toutes les personnes entrain de jouer, " +
                                              "groupées par jeu ou de rechercher seulement pour un seul jeu. " +
                                              "\n\nExemple:\n```!wspl NomDuJeu(Optionnel)```";

            public const string ourSummary = "Ouranos is our";
            public const string ourRemarks = "Cette commande a été crée pour <@!141677768206188544>\nLa syntaxe à utiliser est : " +
                                             "\n```!our mot (*anus* est fortement recommandé)```";
        }
    }
}
