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
            Console.WriteLine("Zadejte nazev jidla, muzete vybirat z techto moznosti:");
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

        public static void VypisInfo()
        {
            Console.WriteLine("Jidelnicek obsahuje tato jidla:");
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
    }
}