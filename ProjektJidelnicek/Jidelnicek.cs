using System.Security.Cryptography.X509Certificates;

namespace ProjektJidelnicek
{
    public class Jidlo
    {
        public Recept SamotneJidlo;
        public Recept Priloha;
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
        }

        public static void SmazJidlo()
        // Metoda se zepta na nazev jidla a smaze ho z jidelnicku
        {
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
            Console.WriteLine("Jidlo bylo smazano.");
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
            Console.WriteLine("Jidelnicek obsahuje tato jidla:");
            VypisJidlaNaJidelnicku();
        }
    }
}