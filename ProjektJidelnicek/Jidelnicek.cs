namespace ProjektJidelnicek
{
    public class Jidelnicek
    {
        public Jidlo Jidlo;
        public Jidlo Priloha;
        public Jidelnicek(Jidlo jidlo, Jidlo priloha)
        {
            Jidlo = jidlo;
            Priloha = priloha;
        }
        List<Jidelnicek> vsechno = [];
    }
}