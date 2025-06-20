using System.Security.Cryptography.X509Certificates;

namespace ProjektJidelnicek
{
    public class Jidlo
    {
        public Recept SamotneJidlo { get; private set; }
        public Recept Priloha { get; private set; }
        public Jidlo(Recept samotneJidlo)
        {
            SamotneJidlo = samotneJidlo;
        }
        public Jidlo(Recept samotneJidlo, Recept priloha)
        {
            SamotneJidlo = samotneJidlo;
            Priloha = priloha;
        }
        public static List<Jidlo> jidelnicek = [];

        public static string oddelovac = "------------------------------------------------------";

        private static string nakupnimSeznamSoubor = @"C:\C_Sharp\czechitas_jaro_25\projekt_jidelnicek\ProjektJidelnicek\nakupni_seznam";

        // Slovnik obsahujici kategorie pro nakupni sezanm
        private static Dictionary<string, int> slovnikNakupniSeznam = new Dictionary<string, int>
        {
            { "ano", 1 },
            { "ne", 2 },
        };

        // Kategorie pro nakupni seznam
        public static Kategorie kategorieNakupniSeznam = new Kategorie(slovnikNakupniSeznam);

        public static void PridejJidlo()
        // Metoda se zepta na nazev jidla a prida ho do jidelnicku
        // Pokud ma jidlo prilohu, zepta se i na ni a prida ji
        {
            Console.WriteLine("Zadejte nazev jidla, ktere chcete pridat. Muzete vybirat z techto moznosti:");
            Recept.VypisRecepty(Recept.vsechno.Where(x => x.Kategorie != 4).Select(x => x).ToList());
            string vstup = Console.ReadLine();

            if (!Recept.ZjistiJestliJeReceptVSeznamu(vstup))
            {
                Console.WriteLine("Nezname jidlo, neni mozne ho pridat");
                return;
            }

            Recept noveJidlo = Recept.NajdiReceptDleNazvu(vstup);

            if (noveJidlo.MaPrilohu)
            {
                Console.WriteLine("Zadejte nazev prilohy, muzete vybirat z techto moznosti:");
                Recept.VypisRecepty(Recept.vsechno.Where(x => x.Kategorie == 4).Select(x => x).ToList());
                string vstupPriloha = Console.ReadLine();

                if (!Recept.ZjistiJestliJeReceptVSeznamu(vstupPriloha))
                {
                    Console.WriteLine("Neznama priloha, jidlo s prilohou nebylo pridano");
                    return;
                }

                if (Recept.NajdiReceptDleNazvu(vstupPriloha).Kategorie != 4)
                {
                    Console.WriteLine("Neznama priloha, jidlo s prilohou nebylo pridano");
                    return;
                }

                jidelnicek.Add(new Jidlo(noveJidlo, Recept.NajdiReceptDleNazvu(vstupPriloha)));
                Console.WriteLine($"Jidlo '{noveJidlo.Nazev}' bylo pridano do jidelnicku");
            }
            else
            {
                jidelnicek.Add(new Jidlo(noveJidlo));
                Console.WriteLine($"Jidlo '{noveJidlo.Nazev}' bylo pridano do jidelnicku");
            }
        }

        public static void SmazJidlo()
        // Metoda se zepta na nazev jidla a smaze ho z jidelnicku
        {
            if (jidelnicek.Count == 0)
            {
                Console.WriteLine("Jidelnicek neobsahuje zadna jidla");
                return;
            }

            Console.WriteLine("Zadejte nazev jidla, ktere chcete smazat. Muzete vybirat z techto moznosti:");
            VypisJidlaNaJidelnicku();
            string vstup = Console.ReadLine();

            if (!ZjistiJestliJeJidloVJidelnicku(vstup))
            {
                Console.WriteLine("Nezname jidlo, neni mozne ho smazat");
                return;
            }

            Jidlo jidloKeSmazani = NajdiJidloDleNazvu(vstup);
            jidelnicek.Remove(jidloKeSmazani);
            Console.WriteLine($"Jidlo '{jidloKeSmazani.SamotneJidlo.Nazev}' bylo smazano z jidelnicku");
        }

        public static void VypisJidlaNaJidelnicku()
        // Metoda vypise jidla na jidelnicku
        {
            Console.WriteLine(oddelovac);
            foreach (Jidlo jidlo in jidelnicek)
            {
                if (jidlo.SamotneJidlo.MaPrilohu)
                {
                    Console.WriteLine($"{jidlo.SamotneJidlo.Nazev} & {jidlo.Priloha.Nazev}");
                }
                else
                {
                    Console.WriteLine($"{jidlo.SamotneJidlo.Nazev}");
                }
            }
            Console.WriteLine(oddelovac);
        }

        public static void VypisPocetJidelDleKategorii()
        // Metoda vypise pocet jidel v jednotlivych kategoriich
        {
            Console.WriteLine($"Pocet jidel s masem: {jidelnicek.Where(x => x.SamotneJidlo.Kategorie == Recept.kategorieRecept.Slovnik["recept s masem"])
                                                                .Select(x => x).Count()}");
            Console.WriteLine($"Pocet jidel bez masa: {jidelnicek.Where(x => x.SamotneJidlo.Kategorie == Recept.kategorieRecept.Slovnik["recept bez masa"])
                                                                 .Select(x => x).Count()}");
            Console.WriteLine($"Pocet sladkych jidel: {jidelnicek.Where(x => x.SamotneJidlo.Kategorie == Recept.kategorieRecept.Slovnik["recept na sladke jidlo"])
                                                                 .Select(x => x).Count()}");
            Console.WriteLine($"Pocet receptu na peceni: {jidelnicek.Where(x => x.SamotneJidlo.Kategorie == Recept.kategorieRecept.Slovnik["recept na peceni"])
                                                                    .Select(x => x).Count()}");
            Console.WriteLine(oddelovac);
        }

        public static void VypisZeleninuVJidelnicku()
        // Metoda vypise druhy zeleniny obsazene v jidlech
        {
            string druhyZeleniny = String.Join(", ", jidelnicek.SelectMany(x => x.SamotneJidlo.SeznamSurovin.Where(x => x.Kategorie == 4)).Select(x => x.Nazev).Distinct());
            Console.WriteLine($"Jidelnicek obsahuje tyto druhy zeleniny: {druhyZeleniny}");
            Console.WriteLine(oddelovac);
        }

        public static void VypisNakupniSeznam()
        // Metoda se zepta uzivatele, zda si preje ulozit nakupni seznam
        // Pokud ano, ulozi do souboru seznam jidel a nakupni seznam
        {
            List<string> text = [];
            Dictionary<int, List<string>> nakupniSeznam = new Dictionary<int, List<string>>
                {
                    { 1, [] },
                    { 2, [] },
                    { 3, [] },
                    { 4, [] },
                    { 5, [] },
                    { 6, [] },
                    { 7, [] },
                    { 8, [] },
                    { 9, [] },
                    { 10, [] },
                };

            Console.WriteLine("Prejete si ulozit nakupni seznam do souboru?");
            kategorieNakupniSeznam.VypisKategorie();
            int ulozitNakupniSeznamCislo = kategorieNakupniSeznam.NactiCisloKategorie();
            bool ulozitNakupniSeznam = ulozitNakupniSeznamCislo.Equals(1);

            if (ulozitNakupniSeznam)
            {
                File.WriteAllLines(nakupnimSeznamSoubor, PrevedJidlaNaJidelnickuNaSeznam());
                File.AppendAllLines(nakupnimSeznamSoubor, [oddelovac]);

                foreach (Jidlo jidlo in jidelnicek)
                {
                    foreach (Surovina surovina in jidlo.SamotneJidlo.SeznamSurovin)
                    {
                        if (!nakupniSeznam[surovina.Kategorie].Any(x => x == surovina.Nazev))
                        {
                            nakupniSeznam[surovina.Kategorie].Add(surovina.Nazev);
                        }
                    }
                    if (jidlo.SamotneJidlo.MaPrilohu)
                    {
                        foreach (Surovina surovina in jidlo.Priloha.SeznamSurovin)
                        {
                            if (!nakupniSeznam[surovina.Kategorie].Any(x => x == surovina.Nazev))
                            {
                                nakupniSeznam[surovina.Kategorie].Add(surovina.Nazev);
                            }
                        }
                    }
                }

                foreach (var s in nakupniSeznam)
                {
                    text.Add(Surovina.kategorieSurovina.Slovnik.FirstOrDefault(x => x.Value == s.Key).Key.ToUpper());
                    foreach (string surovina in nakupniSeznam[s.Key])
                    {
                        text.Add(surovina);
                    }
                    text.Add("");
                }

                File.AppendAllLines(nakupnimSeznamSoubor, text);
                Console.WriteLine("Nakupni seznam byl ulozen do souboru 'nakupni_seznam'");
            }
        }

        public static List<string> PrevedJidlaNaJidelnickuNaSeznam()
        // Metoda prevede nazvy jidel na jidelnicku na seznam
        {
            List<string> text = [];
            foreach (Jidlo jidlo in jidelnicek)
            {
                if (jidlo.SamotneJidlo.MaPrilohu)
                {
                    text.Add(jidlo.SamotneJidlo.Nazev + " & " + jidlo.Priloha.Nazev);
                }
                else
                {
                    text.Add(jidlo.SamotneJidlo.Nazev);
                }
            }
            return text;
        }

        public static bool ZjistiJestliJeJidloVJidelnicku(string nazev)
        // Metoda zjisti, jestli uz je jidlo ulozene v jidelnicku
        {
            return jidelnicek.Any(x => x.SamotneJidlo.Nazev == nazev);
        }

        public static Jidlo NajdiJidloDleNazvu(string nazevJidla)
        // Metoda najde jidlo v seznamu podle nazvu
        {
            return jidelnicek.Where(x => x.SamotneJidlo.Nazev == nazevJidla).Select(x => x).ToList()[0];
        }

        public static void VypisInfo()
        // Metoda vypise informace o jidelnicku
        {
            // Vypise seznam jidel
            if (jidelnicek.Count == 0)
            {
                Console.WriteLine("Jidelnicek neobsahuje zadna jidla");
                return;
            }

            Console.WriteLine("Jidelnicek obsahuje tato jidla:");
            VypisJidlaNaJidelnicku();

            VypisPocetJidelDleKategorii();

            VypisZeleninuVJidelnicku();

            VypisNakupniSeznam();
        }
    }
}