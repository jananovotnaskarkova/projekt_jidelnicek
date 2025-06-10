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
                    Recept.PridejRecept();
                    break;
                case 2:
                    Recept.SmazRecept();
                    break;
                case 3:
                    Recept.VyhledejRecept();
                    break;
                case 4:
                    Jidlo.PridejJidlo();
                    break;
                case 5:
                    // Jidelnicek.SmazJidlo();
                    Console.WriteLine("Smazani jidla z jidelnicku bude pridano");
                    break;
                case 6:
                    Jidlo.VypisInfo();
                    break;
                case 7:
                    return;
            }
        }
    }

    // Slovnik obsahujici akce
    public static Dictionary<string, int> slovnikAkce = new Dictionary<string, int>
        {
            { "pridat recept", 1 },
            { "smazat recept", 2 },
            { "vyhledat recept", 3 },
            { "pridat jidlo do jidelnicku", 4 },
            { "odebrat jidlo z jidelnicku", 5 },
            { "vypsat info k jidelnicku", 6 },
            { "ukoncit program", 7 }
        };
    private static Kategorie kategorieAkce = new Kategorie(slovnikAkce);
}