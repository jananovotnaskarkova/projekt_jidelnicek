namespace ProjektJidelnicek
{
    public class Jidelnicek
    {
        public Recept Jidlo;
        public Recept Priloha;
        public Jidelnicek(Recept jidlo)
        {
            Jidlo = jidlo;
        }
        public Jidelnicek(Recept jidlo, Recept priloha)
        {
            Jidlo = jidlo;
            Priloha = priloha;
        }
        public static List<Jidelnicek> vsechno = [];

        public static string oddelovac = "------------------------------------------------------";

        public static void PridejJidlo()
        // Metoda se zepta na nazev jidla a prida ho do jidelnicku
        {
            Console.WriteLine("Zadejte nazev jidla, muzete vybirat z techto moznosti:");
            Console.WriteLine(oddelovac);
            Recept.VypisRecepty(Recept.vsechno.Where(x => x.Kategorie != 4).Select(x => x).ToList());
            Console.WriteLine(oddelovac);
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
                Console.WriteLine(oddelovac);
                Recept.VypisRecepty(Recept.vsechno.Where(x => x.Kategorie == 4).Select(x => x).ToList());
                Console.WriteLine(oddelovac);
                string vstupPriloha = Console.ReadLine();
                if (!Recept.ZjistiJestliJeReceptVSeznamu(vstup))
                {
                    Console.WriteLine("Nezname priloha, jidlo s prilohou nebylo pridano");
                    return;
                }
                vsechno.Add(new Jidelnicek(noveJidlo, Recept.NajdiReceptDleNazvu(vstupPriloha)));
            }
            else
            {
                vsechno.Add(new Jidelnicek(noveJidlo));
            }
        }

        public static void VypisInfo()
        {
            Console.WriteLine("Jidelnicek obsahuje tato jidla:");
            Console.WriteLine(oddelovac);
            foreach (Jidelnicek jidelnicek in vsechno)
            {
                if (jidelnicek.Jidlo.MaPrilohu)
                {
                    Console.WriteLine($"{jidelnicek.Jidlo.Nazev} & {jidelnicek.Priloha.Nazev}");
                }
                else
                {
                    Console.WriteLine($"{jidelnicek.Jidlo.Nazev}");
                }
            }
            Console.WriteLine(oddelovac);
        }
    }
}