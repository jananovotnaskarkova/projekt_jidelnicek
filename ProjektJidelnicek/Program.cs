namespace ProjektJidelnicek;

class Program
{
    static void Main(string[] args)
    {
        Recept.NactiReceptyZeSouboru();
        Console.WriteLine("Vita vas program na planovani jidelnicku!");

        while (true)
        {
            kategorieAkce.VypisKategorie();
            int cisloAkce = kategorieAkce.NactiCisloKategorie();
            switch (cisloAkce)
            {
                case 1:
                    Surovina.VypisSurovinyDleKategorie();
                    break;
                case 2:
                    Recept.PridejRecept();
                    break;
                case 3:
                    Recept.SmazRecept();
                    break;
                case 4:
                    Recept.VyhledejRecept();
                    break;
                case 5:
                    Jidlo.PridejJidlo();
                    break;
                case 6:
                    // Jidelnicek.SmazJidlo();
                    Console.WriteLine("Smazani jidla z jidelnicku bude pridano");
                    break;
                case 7:
                    Jidlo.VypisInfo();
                    break;
                case 8:
                    return;
            }
        }
    }

    // Slovnik obsahujici akce
    public static Dictionary<string, int> slovnikAkce = new Dictionary<string, int>
        {
            { "vypsat suroviny dle kategorie", 1 },
            { "pridat recept", 2 },
            { "smazat recept", 3 },
            { "vyhledat recept", 4 },
            { "pridat jidlo do jidelnicku", 5 },
            { "odebrat jidlo z jidelnicku", 6 },
            { "vypsat info k jidelnicku", 7 },
            { "ukoncit program", 8 }
        };
    private static Kategorie kategorieAkce = new Kategorie(slovnikAkce);
}