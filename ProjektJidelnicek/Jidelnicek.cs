using System.Security.Cryptography.X509Certificates;

namespace ProjektJidelnicek
{
    public class Jidlo
    {
        public Recept SamotneJidlo { get; }
        public Recept Priloha { get; }
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
                if (!Recept.ZjistiJestliJeReceptVSeznamu(vstup))
                {
                    Console.WriteLine("Nezname priloha, jidlo s prilohou nebylo pridano");
                    return;
                }
                jidelnicek.Add(new Jidlo(noveJidlo, Recept.NajdiReceptDleNazvu(vstupPriloha)));
            }
            else
            {
                jidelnicek.Add(new Jidlo(noveJidlo));
            }
            Console.WriteLine($"Jidlo '{noveJidlo.Nazev}' bylo pridano do jidelnicku");
        }

        public static void SmazJidlo()
        // Metoda se zepta na nazev jidla a smaze ho z jidelnicku
        {
            if (jidelnicek.Count == 0)
            {
                Console.WriteLine("Jidelnik neobsahuje zadna jidla");
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
            if (jidelnicek.Count == 0)
            {
                Console.WriteLine("Jidelnik neobsahuje zadna jidla");
                return;
            }

            Console.WriteLine("Jidelnicek obsahuje tato jidla:");
            VypisJidlaNaJidelnicku();
            Console.WriteLine($"Pocet jidel s masem: {jidelnicek.Where(x => x.SamotneJidlo.Kategorie == Recept.kategorieRecept.Slovnik["recept s masem"])
                                                                .Select(x => x).Count()}");
            Console.WriteLine($"Pocet jidel bez masa: {jidelnicek.Where(x => x.SamotneJidlo.Kategorie == Recept.kategorieRecept.Slovnik["recept bez masa"])
                                                                 .Select(x => x).Count()}");
            Console.WriteLine($"Pocet sladkych jidel: {jidelnicek.Where(x => x.SamotneJidlo.Kategorie == Recept.kategorieRecept.Slovnik["recept na sladke jidlo"])
                                                                 .Select(x => x).Count()}");
            Console.WriteLine($"Pocet receptu na peceni: {jidelnicek.Where(x => x.SamotneJidlo.Kategorie == Recept.kategorieRecept.Slovnik["recept na peceni"])
                                                                    .Select(x => x).Count()}");
            Console.WriteLine(oddelovac);
            string druhyZeleniny = String.Join(", ", jidelnicek.SelectMany(x => x.SamotneJidlo.SeznamSurovin.Where(x => x.Kategorie == 4)).Select(x => x.Nazev).Distinct());
            Console.WriteLine($"Jidelnicek obsahuje tyto druhy zeleniny: {druhyZeleniny}");
            Console.WriteLine(oddelovac);

            // Pridat moznost ulozit si nakupni seznam do souboru
        }
    }
}