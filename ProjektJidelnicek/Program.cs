namespace ProjektJidelnicek;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Vita vas program na planovani jidelnicku!");
        Jidlo.NactiJidlaZeSouboru();
        Jidlo.PridejJidlo();

        Jidlo.VypisJidla();
        Surovina.VypisSuroviny();
        kategorieAkce.VypisKategorie();
        int cisloAkce = kategorieAkce.NactiCisloKategorie();
        switch (cisloAkce)
        {
            case 1:
                Jidlo.PridejJidlo();
                break;
            case 2:
                // Jidlo.SmazJidlo();
                Console.WriteLine("Smazani jidla bude pridano");
                break;
            case 3:
                // Jidlo.VyhledejJidlo();
                Console.WriteLine("Vyhledani jidla bude pridano");
                break;
            case 4:
                // Jidlo.VypisJidla();
                Console.WriteLine("Vypsani jidel bude pridano");
                break;
            case 5:
                // Jidelnicek.PridejJidlo();
                Console.WriteLine("Pridani jidla do jidelnicku bude pridano");
                break;
            case 6:
                // Jidelnicek.SmazJidlo();
                Console.WriteLine("Smazani jidla z jidelnicku bude pridano");
                break;
            case 7:
                // Jidelnicek.VypisInfo();
                Console.WriteLine("Vypsani informaci k jidelnicku bude pridano");
                break;
        }

        while (true)
        {


        }
    }

    // Slovnik obsahujici akce
    public static Dictionary<string, int> slovnikAkce = new Dictionary<string, int>
        {
            { "pridat jidlo", 1},
            { "smazat jidlo", 2},
            { "vyhledat jidlo", 3},
            { "vypsat jidlo v kategorii", 4},
            { "pridat jidlo do jidelnicku", 5},
            { "odebrat jidlo z jidelnicku", 6 },
            { "vypsat info k jidelnicku", 7 },
        };
    private static Kategorie kategorieAkce = new Kategorie(slovnikAkce);
}