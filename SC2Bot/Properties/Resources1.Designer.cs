﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SC2Bot.Properties {
    using System;
    
    
    /// <summary>
    ///   Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    /// </summary>
    // Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    // à l'aide d'un outil, tel que ResGen ou Visual Studio.
    // Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    // avec l'option /str ou régénérez votre projet VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SC2Bot.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
        ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Utilise le site Aligulac (http://aligulac.com/periods/)\nPermet de voir la race dominante et dominée. \n```# Renvoie les 10 derniers résultats\n!balance```\n\n```# Renvoie les résultats depuis une date, si le -to est renseigné, les résultats sont compris entre ces deux dates\n!balance -from 02/06/2013 -to 23/12/2013```\n\n```# Donne un résumé des races OP\n!balance -op\n\n# Donne un résumé des races faibles\n!balance -weak```\n\n```# Condense et fait la moyenne du resultat\n!balance -avg```.
        /// </summary>
        internal static string BalanceCommandHelp {
            get {
                return ResourceManager.GetString("BalanceCommandHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Il y a plusieurs commandes disponibles : `!player` `!top` `!predict` `!quote` `!balance`\nIl suffit d&apos;utiliser le mot clé \&quot;**-help**\&quot; pour plus d&apos;information, example :\n`!predict -help`.
        /// </summary>
        internal static string HelpCommand {
            get {
                return ResourceManager.GetString("HelpCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Recherche un joueur sur Aligulac, si les informations sont disponibles, la team ainsi que la page liquipedia est retournée.\nLa syntaxe à utiliser est : \n ```!player pseudo```.
        /// </summary>
        internal static string PlayerCommandHelp {
            get {
                return ResourceManager.GetString("PlayerCommandHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Il manque des paramètres.\nLa syntaxe correcte est : \n ```!search mot_clé```.
        /// </summary>
        internal static string PlayerCommandMissing {
            get {
                return ResourceManager.GetString("PlayerCommandMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Utilise le système de prédiction d&apos;Aligulac (http://aligulac.com/inference/).\nLa syntaxe à utiliser est : \n```!predict Player_A Player_B (Optionnel BO_nb)```\n\nExample :\n```!predict ByuN Dark 3```.
        /// </summary>
        internal static string PredictCommandHelp {
            get {
                return ResourceManager.GetString("PredictCommandHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Il manque des paramètres.\nLa syntaxe correcte est :\n```!predict Player_A Player_B (Optionnel BO_nb)```\n\nExample :\n ```!predict ByuN Dark 3```.
        /// </summary>
        internal static string PredictCommandMissing {
            get {
                return ResourceManager.GetString("PredictCommandMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à My life for Aiur !.
        /// </summary>
        internal static string ProtossQuote {
            get {
                return ResourceManager.GetString("ProtossQuote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Retourne une quote SC2 via le site sc2quoteoftheday.com (et plus)\nLa syntaxe à utiliser est : `!quote (optionnel auteur) (optionnel -count)`\nExemple :\n```\n# Récupère les quotes de l&apos;auteur cité\n!quote Day9\n\n# Retourne le nombre de quote pour l&apos;auteur cité\n!quote Day9 -count\n\n# Retourne tous les auteurs de quotes (en message privé)\n!quote -all```.
        /// </summary>
        internal static string QuoteCommandHelp {
            get {
                return ResourceManager.GetString("QuoteCommandHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Je n&apos;ai pas de quote pour **{0}**\nMais celle là est pas mal quand même :\n.
        /// </summary>
        internal static string QuoteCommandNotFound {
            get {
                return ResourceManager.GetString("QuoteCommandNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Liste envoyée en MP..
        /// </summary>
        internal static string QuoteCommandSendMP {
            get {
                return ResourceManager.GetString("QuoteCommandSendMP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Pour les hommes !.
        /// </summary>
        internal static string RandomQuote {
            get {
                return ResourceManager.GetString("RandomQuote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Hello !\nTu as rejoint le Discord **SC2 France** mais tu n&apos;as toujours pas renseigné ta race, quelle race joues-tu ?\nécris : **Zerg**, **Terran**, **Protoss** ou **Random**.
        /// </summary>
        internal static string RappelRaceStr {
            get {
                return ResourceManager.GetString("RappelRaceStr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Kaboom Baby !.
        /// </summary>
        internal static string TerranQuote {
            get {
                return ResourceManager.GetString("TerranQuote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Utilise le système de classement d&apos;Aligulac.\nLa syntaxe à utiliser est : \n```!top```.
        /// </summary>
        internal static string TopCommandHelp {
            get {
                return ResourceManager.GetString("TopCommandHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Bienvenue !\nQuelle race joues-tu ?\nécris : **Zerg**, **Terran**, **Protoss** ou **Random**.
        /// </summary>
        internal static string WelcomeStr {
            get {
                return ResourceManager.GetString("WelcomeStr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à For the Swarm ♥.
        /// </summary>
        internal static string ZergQuote {
            get {
                return ResourceManager.GetString("ZergQuote", resourceCulture);
            }
        }
    }
}